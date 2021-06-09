using IdentityServer.AuthServer.Models;
using IdentityServer.AuthServer.Services.Users;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.AuthServer.Services.Profiles
{
    public class CustomProfileService : IProfileService
    {
        private readonly ICustomUserService _customUserService;

        public CustomProfileService(ICustomUserService customUserService)
        {
            _customUserService = customUserService;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            int userId = int.Parse(context.Subject.GetSubjectId());
            CustomUser user = await _customUserService.FindByIdAsync(userId);

            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("City", user.City)
            };

            if (user.Id == 1)
            {
                claims.Add(new Claim("Role", "Admin"));
            }
            else
            {
                claims.Add(new Claim("Role", "Customer"));
            }

            context.AddRequestedClaims(claims);
            //context.IssuedClaims = claims; -> Claimleri token'a eklemek istersek
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            int userId = int.Parse(context.Subject.GetSubjectId());
            CustomUser user = await _customUserService.FindByIdAsync(userId);

            context.IsActive = user != null;
        }
    }
}
