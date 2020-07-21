using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OEE_API._Services.Interfaces;
using OEE_API.ViewModels;

namespace OEE_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DownTimeReasonController : ControllerBase
    {
        private readonly IDownTimeReasonService _server;
        public DownTimeReasonController(IDownTimeReasonService server) {
            _server = server;
        }

        [HttpPost("getDataChart")]
        public async Task<IActionResult> GetDataChart([FromBody]DownTimeReasonParamModel param) {
            var data = await _server.GetDataChart(param);
            return Ok(data);
        }
    }
}