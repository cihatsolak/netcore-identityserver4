using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityServer.WebClient1.Models.Users
{
    public class UserConfigureViewModel
    {
        public List<Claim> Claims { get; set; }
        public Dictionary<string, string> AuthenticationItems { get; set; }
    }
}
