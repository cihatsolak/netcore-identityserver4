using IdentityServer.API2.Models.Brands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace IdentityServer.API2.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult GetBrands()
        {
            List<Brand> brands = new()
            {
                new Brand
                {
                    Id = 1,
                    Name = "Volkswagen"
                },
                new Brand
                {
                    Id = 2,
                    Name = "Seat"
                },
                new Brand
                {
                    Id = 3,
                    Name = "Porsche"
                }
            };

            return Ok(brands);
        }
    }
}
