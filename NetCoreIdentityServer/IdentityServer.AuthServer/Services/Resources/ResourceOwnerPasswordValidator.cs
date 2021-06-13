using IdentityModel;
using IdentityServer.AuthServer.Services.Users;
using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace IdentityServer.AuthServer.Services.Resources
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly ICustomUserService _customUserService;

        public ResourceOwnerPasswordValidator(ICustomUserService customUserService)
        {
            _customUserService = customUserService;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            bool isSuccess = await _customUserService.ValidatePasswordAsync(context.UserName, context.Password);
            if (isSuccess)
            {
                var user = await _customUserService.FindByEmailAsync(context.UserName); //Username bizde Email'e denk geliyor.
                context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
            }
        }
    }
}
