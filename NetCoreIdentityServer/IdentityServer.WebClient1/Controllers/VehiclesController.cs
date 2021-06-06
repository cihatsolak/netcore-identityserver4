using IdentityModel.Client;
using IdentityServer.WebClient1.Models.DTOs;
using IdentityServer.WebClient1.Models.Settings;
using IdentityServer.WebClient1.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace IdentityServer.WebClient1.Controllers
{
    [Authorize]
    public class VehiclesController : Controller
    {
        private readonly ClientSettings _clientSettings;
        private readonly ILogger<VehiclesController> _logger;
        private readonly IHttpClientService _httpClientService;

        public VehiclesController(IOptions<ClientSettings> clientSettings, ILogger<VehiclesController> logger, IHttpClientService httpClientService)
        {
            _clientSettings = clientSettings.Value;
            _logger = logger;
            _httpClientService = httpClientService;
        }

        /// <summary>
        /// Önce token alıp sonra istek yaptıgımız hayali bir senaryo
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            HttpClient httpClient = new();

            var discoveryDocumentResponse = await httpClient.GetDiscoveryDocumentAsync("https://localhost:5000"); //IdentityServer'ın tüm endpointlerini alıyorum.
            if (discoveryDocumentResponse.IsError)
            {
                _logger.LogError("Endpointler listelenirken hata oluştu.");
                return View();
            }

            //Üyelik sistemim yok clientId ve clientSecret ile token al.
            var tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                ClientId = _clientSettings.Id,
                ClientSecret = _clientSettings.Secret,
                Address = discoveryDocumentResponse.TokenEndpoint
            });

            if (tokenResponse.IsError)
            {
                _logger.LogError("Token alınırken hata oluştu.");
                return View();
            }

            httpClient.SetBearerToken(tokenResponse.AccessToken); //Bearer token

            var httpResponseMessage = await httpClient.GetAsync("https://localhost:5003/api/vehicles/getvehicles");
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                _logger.LogWarning("Apiye istek atılamadı!");
                return View();
            }

            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            var vehiclesDTO = JsonConvert.DeserializeObject<List<VehicleDTO>>(content);

            return View(vehiclesDTO);
        }

        /// <summary>
        /// Token almama gerek yok, uygulamaya giriş yapmış kullanıcının token'ı ile işlem yapıyorum. 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index2()
        {
            var httpClient = await _httpClientService.GetHttpClientAsync();

            var httpResponseMessage = await httpClient.GetAsync("https://localhost:5003/api/vehicles/getvehicles");
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                _logger.LogWarning("Apiye istek atılamadı!");
                return View();
            }

            var content = await httpResponseMessage.Content.ReadAsStringAsync();
            var vehiclesDTO = JsonConvert.DeserializeObject<List<VehicleDTO>>(content);

            return View(vehiclesDTO);
        }
    }
}
