using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OEE_API.Application.Interfaces;

namespace OEE_API.Controllers
{
    public class AvailabilityController : ApiController
    {
        private readonly IAvailabilityService _availabilityService;
        public AvailabilityController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;
        }   

        [HttpGet("GetAvailability")]
        public async Task<IActionResult> GetListAvailabilityAsync(string factory, string building, string shift, string date, string dateTo)
        {
            var data = await _availabilityService.GetListAvailabilityAsync(factory, building, shift, date, dateTo);

            return Ok(data);
        }
    }
}