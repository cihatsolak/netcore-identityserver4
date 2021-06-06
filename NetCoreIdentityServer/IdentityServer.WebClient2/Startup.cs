using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace IdentityServer.WebClient2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "WebClient2Cookie"; //Web cookie ismi (Ýsim rastgele Verilmiþtir.)
                options.DefaultChallengeScheme = "OpenIdConnectCookie"; //IdentityServer projesinden gelecek cookie (Ýsim rastgele Verilmiþtir.)
            })
            .AddCookie("WebClient2Cookie", options =>
            {
                options.AccessDeniedPath = new PathString("/User/AccessDenied");
            })
            .AddOpenIdConnect("OpenIdConnectCookie", options =>
            {
                options.SignInScheme = "WebClient2Cookie";
                options.Authority = "https://localhost:5000"; //Token daðýtan yer, yetkili kim?
                options.ClientId = "SampleClient4";
                options.ClientSecret = "SampleClientSecret";
                options.ResponseType = "code id_token"; //Response'da ne istiyorum?
                options.GetClaimsFromUserInfoEndpoint = true; //Claimde eklemiþ oldugum user bilgilerini cookie'e dahil et.
                options.SaveTokens = true; //Baþarýlý giriþte access/refresh token'ý kaydet.
                options.Scope.Add("IdentityServer.API1.Read"); //Api1 için okuma izni istiyorum. (Daha önce auth server'da tanýmlandý)
                options.Scope.Add("offline_access"); //Refresh token scope'unu talep ediyorum. (Daha önce auth server'da tanýmlandý)

                options.Scope.Add("CountryAndCityCustomResource"); //CountryAndCityCustomResource scope'unu talep ediyorum. (Daha önce auth server'da tanýmlandý)
                options.ClaimActions.MapUniqueJsonKey("Country", "Country"); //AuthServerdan gelen claim ismiyle buradaki claim'i maple
                options.ClaimActions.MapUniqueJsonKey("City", "City"); //AuthServerdan gelen claim ismiyle buradaki claim'i maple

                options.Scope.Add("RolesCustomResource"); //Role scope'unu talep ediyorum. (Daha önce auth server'da tanýmlandý)
                options.ClaimActions.MapUniqueJsonKey("Role", "Role"); //AuthServerdan gelen claim ismiyle buradaki claim'i maple
                options.TokenValidationParameters = new TokenValidationParameters //Role bazlý yetkilendirme yaptýðýmý bildiriyorum
                {
                    RoleClaimType = "Role" //Role claim'den bilgiyi aldýrýyorum
                };
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
