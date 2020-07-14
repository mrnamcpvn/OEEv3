using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OEE_API._Services.Interfaces;
namespace OEE_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class AvailabilityController : ControllerBase
    {
        private readonly IAvailabilityService _availabilityService;
        public AvailabilityController(IAvailabilityService availabilityService)
        {
            _availabilityService = availabilityService;

        }
    //     [HttpGet("GetAvailability")]
    //     public async Task<IActionResult> GetListAvailability(string factory, string building, string machine_type,
    //    string shift, string date, string dateTo)
    //     {
    //         var data = await _availabilityService.GetListAvailability( factory,  building,
    //           machine_type, shift,  date,  dateTo);
    //         return Ok(data);
    //     }
    }
}