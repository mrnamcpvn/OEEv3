using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OEE_API.Application.Interfaces;
using OEE_API.Application.ViewModels;

namespace OEE_API.Controllers
{
    public class DowntimeReasonsController : ApiController
    {
        private readonly IDowntimeReasonsService _downtimeReasonsService;
        public DowntimeReasonsController(IDowntimeReasonsService downtimeReasonsService)
        {
            _downtimeReasonsService = downtimeReasonsService;
        }

        [HttpGet("getDowntimeReasons")]
        public async Task<IActionResult> GetDowntimeReasons(string factory, string building, string machine, string shift, string date, int page)
        {
            try
            {
                var data = await _downtimeReasonsService.GetDuration(factory, building, machine, shift, date, page);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet("getDowntimeReasonDetail")]
        public async Task<IActionResult> GetDowntimeReasonDetail(string reason_1)
        {
            try
            {
                var data = await _downtimeReasonsService.GetDowntimeReasonDetail(reason_1);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost("addDowntimeReason")]
        public async Task<IActionResult> addDowntimeReason(ChartReason chartReason)
        {
            try
            {
                var data = await _downtimeReasonsService.AddDowntimeReason(chartReason);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}