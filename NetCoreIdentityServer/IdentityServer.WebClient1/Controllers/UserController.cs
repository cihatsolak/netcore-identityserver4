using IdentityServer.WebClient1.Models.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
    }
}
