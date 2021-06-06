using IdentityModel.Client;
using IdentityServer.WebClient1.Models.Settings;
using IdentityServer.WebClient1.Models.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer.WebClient1.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ClientSettings _clientSettings;

        public UserController(IOptions<ClientSettings> clientSettings)
        {
            _clientSettings = clientSettings.Value;
        }

        [HttpGet]
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

        [HttpGet]
        public async Task<IActionResult> GetRefreshToken()
        {
            HttpClient httpClient = new();
            var discoveryDocumentResponse = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5000"); //IdentityServer'ın tüm endpointlerini alıyorum.
            if (discoveryDocumentResponse.IsError)
            {
                //log
            }

            string refreshToken = await HttpContext.GetTokenAsync(OpenIdConnectParameterNames.RefreshToken);

            var tokenResponse = await httpClient.RequestRefreshTokenAsync(new RefreshTokenRequest
            {
                ClientId = _clientSettings.Id,
                ClientSecret = _clientSettings.Secret,
                RefreshToken = refreshToken,
                Address = discoveryDocumentResponse.TokenEndpoint
            });

            if (tokenResponse.IsError)
            {
                //log
            }

            List<AuthenticationToken> authenticationTokens = new()
            {
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.IdToken,
                    Value = tokenResponse.IdentityToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.AccessToken,
                    Value = tokenResponse.AccessToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.RefreshToken,
                    Value = tokenResponse.RefreshToken
                },
                new AuthenticationToken
                {
                    Name = OpenIdConnectParameterNames.ExpiresIn,
                    Value = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn).ToString("o", CultureInfo.InvariantCulture)
                }
            };

            var authenticationResult = await HttpContext.AuthenticateAsync();
            authenticationResult.Properties.StoreTokens(authenticationTokens);

            await HttpContext.SignInAsync("WebClient1Cookie", authenticationResult.Principal, authenticationResult.Properties);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminIndex()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Customer, Admin")]
        public IActionResult CustomerIndex()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
