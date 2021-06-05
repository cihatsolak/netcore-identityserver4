using IdentityServer.WebClient1.Models.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer.WebClient1
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
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "WebClient1Cookie"; //Web cookie ismi (Ýsim rastgele Verilmiþtir.)
                options.DefaultChallengeScheme = "OpenIdConnectCookie"; //IdentityServer projesinden gelecek cookie (Ýsim rastgele Verilmiþtir.)
            })
            .AddCookie("WebClient1Cookie")
            .AddOpenIdConnect("OpenIdConnectCookie", options =>
            {
                options.SignInScheme = "WebClient1Cookie";
                options.Authority = "https://localhost:5000"; //Token daðýtan yer, yetkili kim?
                options.ClientId = "SampleClient3";
                options.ClientSecret = "SampleClientSecret";
                options.ResponseType = "code id_token"; //Response'da ne istiyorum?
            });

            services.AddControllersWithViews();

            services.Configure<ClientSettings>(Configuration.GetSection(nameof(ClientSettings)));
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
