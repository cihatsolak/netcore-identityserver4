using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer.API1
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    //Bu urlden public key alýcak ve token'ý aldýðý public key ile dogrulayacak.
                    options.Authority = "https://localhost:5000"; //Bu token'ý yayýnlayan kim? Yetkili kim?
                    options.Audience = "IdentityServer.API1"; //Gelen token'un Audience property'sinde mutlaka bu alan olmalý.
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ReadVehicles", policy =>
                {
                    policy.RequireClaim("scope", "IdentityServer.API1.Read"); //Token'ýn Header'ýnda bulunan scope içerisinde bu þartý saðlamalý
                });

                options.AddPolicy("UpdateOrCreateVehicles", policy =>
                {
                    policy.RequireClaim("scope", "IdentityServer.API1.Create", "IdentityServer.API1.Update"); //Token'ýn Header'ýnda bulunan scope içerisinde bu þartý saðlamalý
                });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication(); //Kimlik Dogrulama
            app.UseAuthorization(); //Yetkilendirme

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
