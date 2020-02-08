using Microsoft.AspNetCore.Mvc;
using OEE_API.Application.Interfaces;

namespace OEE_API.Controllers
{
    public class DowntimeReasonsController : ApiController
    {
        private readonly IDowntimeReasonsService _downtimeReasonsService;
        public DowntimeReasonsController(IDowntimeReasonsService downtimeReasonsService)
        {
            _downtimeReasonsService = downtimeReasonsService;
        }

        [HttpGet]
        public IActionResult GetDowntimeReasons(string factory, string machine, string shift, string date)
        {
            var data = _downtimeReasonsService.GetDuration(factory, machine, shift, date);
            return Ok(data);
        }
    }
}