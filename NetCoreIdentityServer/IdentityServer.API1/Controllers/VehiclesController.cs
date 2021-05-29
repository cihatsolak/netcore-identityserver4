using IdentityServer.API1.Models.Vehicles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IdentityServer.API1.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetVehicles()
        {
            List<Vehicle> vehicles = new()
            {
                new Vehicle
                {
                    Id = 1,
                    Brand = "Volkswagen",
                    Price = 350000,
                    Stock = 203
                },
                new Vehicle
                {
                    Id = 2,
                    Brand = "Seat",
                    Price = 230000,
                    Stock = 146
                },
                new Vehicle
                {
                    Id = 2,
                    Brand = "Porsche",
                    Price = 890000,
                    Stock = 24
                },
                new Vehicle
                {
                    Id = 2,
                    Brand = "Scania",
                    Price = 624000,
                    Stock = 4
                }
            };

            return Ok(vehicles);
        }
    }
}
