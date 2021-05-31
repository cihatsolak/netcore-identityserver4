﻿using IdentityModel.Client;
using IdentityServer.WebClient1.Models.DTOs;
using IdentityServer.WebClient1.Models.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace IdentityServer.WebClient1.Controllers
{
    public class VehiclesController : Controller
    {
        private readonly ClientSettings _clientSettings;
        private readonly ILogger<VehiclesController> _logger;

        public VehiclesController(IOptions<ClientSettings> clientSettings, ILogger<VehiclesController> logger)
        {
            _clientSettings = clientSettings.Value;
            _logger = logger;
        }

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
    }
}
