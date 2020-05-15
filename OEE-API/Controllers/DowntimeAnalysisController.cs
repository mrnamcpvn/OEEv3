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
        public async Task<IActionResult> GetDowntimeAnalysis(string factory, string building, string machine, string shift, string date)
        {
            try
            {
                var data = await _downtimeAnalysisService.GetDownTimeAnalysis(factory, building, machine, shift, date);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}