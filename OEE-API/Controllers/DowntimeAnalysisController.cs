using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OEE_API._Services.Interfaces;

namespace OEE_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DowntimeAnalysisController : ControllerBase
    {
        private IDowntimeAnalysisService _downtimeAnalysisService;

        public DowntimeAnalysisController(IDowntimeAnalysisService downtimeAnalysisService)
        {
            _downtimeAnalysisService = downtimeAnalysisService;
        }

        [HttpGet("getDowntimeAnalysis")]
        public async Task<IActionResult> GetDowntimeAnalysis(string factory, string building, int? machine_type, string machine, int? shift, string date, string dateTo)
        {

            var data = await _downtimeAnalysisService.GetDownTimeAnalysis(factory, building, machine_type, machine, shift, date, dateTo);
            return Ok(data);
        }
    }
}