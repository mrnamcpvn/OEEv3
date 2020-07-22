using System;
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

        [HttpGet("getDataChart")]
        public async Task<IActionResult> GetDataChart(string factory, string building, string machine, string machine_type, string shift, string date, int page = 1) {
            var data = await _server.GetDataChart(factory, building, machine,machine_type,shift,date, page);
            return Ok(data);
        }
        [HttpGet("getReasons")]
        public async Task<IActionResult> GetReasons(int item)
        {
            var data = await _server.GetReason(item);
            return Ok(data);
        }
        [HttpGet("getDowntimeReasonDetail")]
        public async Task<IActionResult> GetDowntimeReasonDetail(string reason_1)
        {
            var data = await _server.GetDowntimeReasonDetail(reason_1);
            return Ok(data);
        }
         [HttpGet("getDowntimeReasonType")]
         public async Task<IActionResult> getDowntimeReasonType()
         {
            var data = await _server.GetDowntimeReasonType();
            return Ok(data);
         }
        [HttpPost("addDowntimeReason")]
        public async Task<IActionResult> AddDowntimeReason(ChartReason chartReason)
        {
            if (await _server.AddDowntimeReasonType(chartReason))
            {
                return NoContent();
            }
            else
            {
                throw new Exception("Creating the Reason failed on save");
            }
        }
    }
}