using IdentityModel.Client;
using IdentityServer.WebClient1.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.WebClient1.Controllers
{
    [AutoValidateAntiforgeryToken]
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {
            var httpClient = new HttpClient();

            var discoveryDocumentResponse = await httpClient.GetDiscoveryDocumentAsync(_configuration["AuthServerUrl"]);
            if (discoveryDocumentResponse.IsError)
            {
                return View(loginViewModel);
            }

            var tokenResponse = await httpClient.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                UserName = loginViewModel.Email,
                Password = loginViewModel.Password,
                ClientId = _configuration["ClientResourceOwner:ClientId"],
                ClientSecret = _configuration["ClientResourceOwner:Secret"]
            });

            if (tokenResponse.IsError)
            {
                return View(loginViewModel);
            }

            var userInfoResponse = await httpClient.GetUserInfoAsync(new UserInfoRequest
            {
                Token = tokenResponse.AccessToken,
                Address = discoveryDocumentResponse.UserInfoEndpoint
            });

            if (userInfoResponse.IsError)
            {
                return View(loginViewModel);
            }

            ClaimsIdentity claimsIdentity = new(userInfoResponse.Claims, "WebClient1Cookie", "Name", "Role");
            ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

            AuthenticationProperties authenticationProperties = new();
            authenticationProperties.StoreTokens(new List<AuthenticationToken>()
            {
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
            });

            await HttpContext.SignInAsync("WebClient1Cookie", claimsPrincipal, authenticationProperties);

            return RedirectToAction("Index", "User");
        }
    }
}
