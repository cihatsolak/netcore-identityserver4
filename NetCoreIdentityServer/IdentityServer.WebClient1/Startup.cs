using IdentityServer.WebClient1.Models.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

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
                options.DefaultScheme = "WebClient1Cookie"; //Web cookie ismi (�sim rastgele Verilmi�tir.)
                options.DefaultChallengeScheme = "OpenIdConnectCookie"; //IdentityServer projesinden gelecek cookie (�sim rastgele Verilmi�tir.)
            })
            .AddCookie("WebClient1Cookie", options =>
            {
                options.AccessDeniedPath = new PathString("/User/AccessDenied");
            })
            .AddOpenIdConnect("OpenIdConnectCookie", options =>
            {
                options.SignInScheme = "WebClient1Cookie";
                options.Authority = "https://localhost:5000"; //Token da��tan yer, yetkili kim?
                options.ClientId = "SampleClient3";
                options.ClientSecret = "SampleClientSecret";
                options.ResponseType = "code id_token"; //Response'da ne istiyorum?
                options.GetClaimsFromUserInfoEndpoint = true; //Claimde eklemi� oldugum user bilgilerini cookie'e dahil et.
                options.SaveTokens = true; //Ba�ar�l� giri�te access/refresh token'� kaydet.
                options.Scope.Add("IdentityServer.API1.Read"); //Api1 i�in okuma izni istiyorum. (Daha �nce auth server'da tan�mland�)
                options.Scope.Add("offline_access"); //Refresh token scope'unu talep ediyorum. (Daha �nce auth server'da tan�mland�)
                options.Scope.Add("email"); // email scope'unu talep ediyorum. (Daha �nce auth server'da tan�mland�)

                options.Scope.Add("CountryAndCityCustomResource"); //CountryAndCityCustomResource scope'unu talep ediyorum. (Daha �nce auth server'da tan�mland�)
                options.ClaimActions.MapUniqueJsonKey("Country", "Country"); //AuthServerdan gelen claim ismiyle buradaki claim'i maple
                options.ClaimActions.MapUniqueJsonKey("City", "City"); //AuthServerdan gelen claim ismiyle buradaki claim'i maple

                options.Scope.Add("RolesCustomResource"); //Role scope'unu talep ediyorum. (Daha �nce auth server'da tan�mland�)
                options.ClaimActions.MapUniqueJsonKey("Role", "Role"); //AuthServerdan gelen claim ismiyle buradaki claim'i maple
                options.TokenValidationParameters = new TokenValidationParameters //Role bazl� yetkilendirme yapt���m� bildiriyorum
                {
                    RoleClaimType = "Role", //Role claim'den bilgiyi ald�r�yorum
                    NameClaimType = "Name"
                };
            });

            services.AddControllersWithViews();
            services.AddHttpContextAccessor();

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

            app.UseAuthentication();
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
