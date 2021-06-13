using IdentityAPI.AuthServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace IdentityAPI.AuthServer.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize(LocalApi.PolicyName)]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserRegisterViewModel model)
        {
            var identityResult = await _userManager.CreateAsync(new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                City = model.City
            }, model.Password);

            if (identityResult.Succeeded)
            {
                var errors = identityResult.Errors.Select(p => p.Description);
                return BadRequest(errors);
            }

            return Ok("Registered!");
        }
    }
}
