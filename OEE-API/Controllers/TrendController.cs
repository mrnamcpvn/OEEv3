using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OEE_API._Services.Interfaces;
using OEE_API.ViewModels;

namespace OEE_API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class TrendController : ControllerBase
    {
        private readonly ITrendService _server;
        public TrendController(ITrendService server) {
            _server = server;
        }

        [HttpPost("loadDataChart")]
        public async Task<IActionResult> LoadDataChart(TrendParamModel param) {
            var data = await _server.DataCharTrend(param);
            return Ok(data);
        }
    }
}