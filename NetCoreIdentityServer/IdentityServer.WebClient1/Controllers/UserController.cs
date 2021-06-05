using IdentityServer.WebClient1.Models.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.WebClient1.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            UserConfigureViewModel userConfigureViewModel = new()
            {
                Claims = User.Claims.ToList(),
                AuthenticationItems = HttpContext.AuthenticateAsync().Result.Properties.Items.ToDictionary(p => p.Key, p => p.Value)
            };

            return View(userConfigureViewModel);
        }

        [HttpGet]
        public async Task SignOut()
        {
            await HttpContext.SignOutAsync("WebClient1Cookie"); //Önce siteden çıkış yapalım
            await HttpContext.SignOutAsync("OpenIdConnectCookie"); //OpenId den çıkış yapalım
        }
    }
}
