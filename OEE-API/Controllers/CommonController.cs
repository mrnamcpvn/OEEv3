using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OEE_API._Services.Interfaces;
using OEE_API.Helpers;

namespace OEE_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommonController : ControllerBase
    {
        private readonly ICommonService _serviceCommon;
        public CommonController(ICommonService serviceCommon) {
            _serviceCommon = serviceCommon;
        }

        [HttpGet("getListFactory")]
        public async Task<IActionResult> GetListFactory() {
            var data = await _serviceCommon.GetListFactory();
            return Ok(data);
        }

        [HttpGet("getListShift")]
        public async Task<IActionResult> GetListShift() {
            var data = await _serviceCommon.GetListShift();
            return Ok(data);
        }

        [HttpGet("getListBuilding/{factory}")]
        public async Task<IActionResult> GetListBuilding(string factory) {
            var data = await _serviceCommon.GetListBuilding(factory);
            return Ok(data);
        }

        [HttpGet("getListMachine/{factory}/{building}")]
        public async Task<IActionResult> GetListMachineType(string factory, string building) {
            var data = await _serviceCommon.GetListMachineType(factory, building);
            return Ok(data);
        }

         // Call class utility to take out all week of year 
        [HttpGet("WeekInYear")]
        public IActionResult GetWeekInYear()
        {
            var weeks = Util.ListWeekOfYear();
            return Ok(weeks);
        }
    }
}