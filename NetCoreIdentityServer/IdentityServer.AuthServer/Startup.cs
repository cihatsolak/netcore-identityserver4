using IdentityServer.AuthServer.Models;
using IdentityServer.AuthServer.Services.Profiles;
using IdentityServer.AuthServer.Services.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer.AuthServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<CustomDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("Default"));
            });

            services.AddScoped<ICustomUserService, CustomUserService>();

            services.AddIdentityServer()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                //.AddTestUsers(Config.GetTestUsers()) //Geli�tirme i�in test userlar� ekliyorum.
                .AddDeveloperSigningCredential() //Development esnas�nda kullanabilece�im bir public key ve Private key olu�turur.
                .AddProfileService<CustomProfileService>(); //Claim tan�mlamalar� 

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer(); //IdentityServer middleware olarak ekliyorum.
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
