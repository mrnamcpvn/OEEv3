using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OEE_API._Services.Interfaces;
using OEE_API.ViewModels;

namespace OEE_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashBoardController : ControllerBase
    {
        private readonly IAvailabilityService _serviceAvailability;
        public DashBoardController(IAvailabilityService serviceAvailability) {
            _serviceAvailability = serviceAvailability;
        }
        
        [HttpPost("loadDataChart")]
        public async Task<IActionResult> LoadDataChart([FromBody]DashBoardParamModel param) {
            var data = await _serviceAvailability.LoadDataChart(param);
            return Ok(data);
        }
    }
}