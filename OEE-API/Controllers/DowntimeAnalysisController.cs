using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OEE_API.Application.Interfaces;
using OEE_API.Application.ViewModels;

namespace OEE_API.Controllers
{
    public class DowntimeAnalysisController : ApiController
    {
        private readonly IDowntimeAnalysisService _downtimeAnalysisService;
        public DowntimeAnalysisController(IDowntimeAnalysisService downtimeAnalysisService)
        {
            _downtimeAnalysisService = downtimeAnalysisService;
        }

        [HttpGet("getDowntimeAnalysis")]
        public async Task<IActionResult> GetDowntimeAnalysis(string factory, string building, string machine_type, string machine, string shift, string date, string dateTo)
        {
            try
            {
                var data = await _downtimeAnalysisService.GetDownTimeAnalysis(factory, building, machine_type, machine, shift, date, dateTo);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}