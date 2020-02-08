using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OEE_API.Application.Interfaces;
using OEE_API.Utilities;
using ServiceSHW_SHD = OEE_API.Application.Interfaces.SHW_SHD;
using ServiceSHB = OEE_API.Application.Interfaces.SHB;
using ServiceSYF = OEE_API.Application.Interfaces.SYF;
using System.Collections.Generic;

namespace OEE_API.Controllers
{
    // Controller Shared for all servers
    public class CommonController : ApiController
    {
        ServiceSHW_SHD.IActionTimeService _actionTimeServiceSHW_SHD;
          ServiceSYF.IActionTimeService _actionTimeServiceSYF;
        ServiceSHW_SHD.ICell_OEEService _Cell_OEEServiceSHW_SHD;
        ServiceSHB.ICell_OEEService _Cell_OEEServiceSHB;
        ServiceSYF.ICell_OEEService _Cell_OEEServiceSYF;

        public CommonController(
            ServiceSHW_SHD.ICell_OEEService Cell_OEEServiceSHW_SHD,
            ServiceSHB.ICell_OEEService Cell_OEEServiceSHB,
            ServiceSYF.ICell_OEEService Cell_OEEServiceSYF,
              ServiceSHW_SHD.IActionTimeService actionTimeServiceSHW_SHD,
                ServiceSYF.IActionTimeService actionTimeServiceSYF
           )
        {
            _Cell_OEEServiceSHW_SHD = Cell_OEEServiceSHW_SHD;
            _Cell_OEEServiceSHB = Cell_OEEServiceSHB;
            _Cell_OEEServiceSYF = Cell_OEEServiceSYF;
            _actionTimeServiceSHW_SHD = actionTimeServiceSHW_SHD;
            _actionTimeServiceSYF = actionTimeServiceSYF;
        }

        // Take out list building of table Cell_OEE
        [HttpGet("{factory}", Name = "GetBuilding")]
        public async Task<IActionResult> GetAllBuilding(string factory)
        {
            List<string> data = null;
            if (factory == "SHW")
            {
                data = await _Cell_OEEServiceSHW_SHD.GetListBuildingByFactoryId(factory);
            }
            if (factory == "SHD")
            {
                data = await _Cell_OEEServiceSHW_SHD.GetListBuildingByFactoryId(factory);
            }
            if (factory == "SHB")
            {
                data = await _Cell_OEEServiceSHB.GetListBuildingByFactoryId(factory);
            }
            if (factory == "SY2")
            {
                data = await _Cell_OEEServiceSYF.GetListBuildingByFactoryId(factory);
            }

            return Ok(data);
        }

        // Take out list machine of table Cell_OEE
        [HttpGet("{factory}/GetMachine/{building}")]
        public async Task<IActionResult> GetAllMachine(string factory, string building)
        {
            List<string> data = null;
            if (factory == "SHW")
            {
                data = await _Cell_OEEServiceSHW_SHD.GetListMachineByFactoryId(factory, building);
            }
            if (factory == "SHD")
            {
                data = await _Cell_OEEServiceSHW_SHD.GetListMachineByFactoryId(factory, building);
            }
            if (factory == "SHB")
            {
                data = await _Cell_OEEServiceSHB.GetListMachineByFactoryId(factory, building);
            }
            if (factory == "SY2")
            {
                data = await _Cell_OEEServiceSYF.GetListMachineByFactoryId(factory, building);
            }
            return Ok(data);
        }

        // Take out list machine of table ActionTime
        [HttpGet("GetAllBuildingActionTime/{factory}")]
        public async Task<IActionResult> GetAllBuildingActionTime(string factory)
        {
            List<string> data = null;
            if (factory == "SHW")
                data = await _actionTimeServiceSHW_SHD.GetListBuildingActionTime("SHC");

            if (factory == "SHD")
                data = await _actionTimeServiceSHW_SHD.GetListBuildingActionTime("CB");

            if (factory == "SHB")
            {

            }
            if (factory == "SY2")
            {

            }
            return Ok(data);
        }

        // Take out list machine of table ActionTime
        [HttpGet("GetAllMachineActionTime")]
        public async Task<IActionResult> GetAllMachineActionTime(string factory, string building)
        {
            List<string> data = null;
            if (factory == "SHW")
                data = await _actionTimeServiceSHW_SHD.GetListMachineActionTime("SHC", building);

            if (factory == "SHD")
                data = await _actionTimeServiceSHW_SHD.GetListMachineActionTime("CB", building);

            if (factory == "SHB")
            {

            }
            if (factory == "SY2")
            {
                data = await _actionTimeServiceSYF.GetListMachineActionTime("SY2");
            }
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