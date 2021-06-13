using IdentityServer.AuthServer.Models;
using IdentityServer.AuthServer.Services.Profiles;
using IdentityServer.AuthServer.Services.Resources;
using IdentityServer.AuthServer.Services.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

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

            string assemblyName = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            services.AddIdentityServer()
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = dbContextOptionsBuilder =>
                    {
                        dbContextOptionsBuilder.UseSqlServer(Configuration.GetConnectionString("Default"), sqlServerOptionsAction =>
                        {
                            sqlServerOptionsAction.MigrationsAssembly(assemblyName);
                        });
                    };
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = dbContextOptionsBuilder =>
                    {
                        dbContextOptionsBuilder.UseSqlServer(Configuration.GetConnectionString("Default"), sqlServerOptionsAction =>
                        {
                            sqlServerOptionsAction.MigrationsAssembly(assemblyName);
                        });
                    };
                })
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                //.AddTestUsers(Config.GetTestUsers()) //Geliþtirme için test userlarý ekliyorum.
                .AddDeveloperSigningCredential() //Development esnasýnda kullanabileceðim bir public key ve Private key oluþturur.
                .AddProfileService<CustomProfileService>()
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>(); //Claim tanýmlamalarý 

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
