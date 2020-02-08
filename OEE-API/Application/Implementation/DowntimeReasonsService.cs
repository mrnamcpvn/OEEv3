using ServiceSHW_SHD = OEE_API.Application.Interfaces.SHW_SHD;
using ServiceSHB = OEE_API.Application.Interfaces.SHB;
using ServiceSYF = OEE_API.Application.Interfaces.SYF;
using OEE_API.Utilities;
using OEE_API.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OEE_API.Application.Interfaces;
using OEE_API.Models.SHW_SHD;

namespace OEE_API.Application.Implementation
{
    public class DowntimeReasonsService : IDowntimeReasonsService
    {
        ServiceSHW_SHD.IActionTimeService _ActionTimeServiceSHW_SHD;
        ServiceSHB.ICell_OEEService _Cell_OEEServiceSHB;
        ServiceSYF.ICell_OEEService _Cell_OEEServiceSYF;

        public DowntimeReasonsService(
           ServiceSHW_SHD.IActionTimeService ActionTimeServiceSHW_SHD,
           ServiceSHB.ICell_OEEService Cell_OEEServiceSHB,
           ServiceSYF.ICell_OEEService Cell_OEEServiceSYF)
        {
            _ActionTimeServiceSHW_SHD = ActionTimeServiceSHW_SHD;
            _Cell_OEEServiceSHB = Cell_OEEServiceSHB;
            _Cell_OEEServiceSYF = Cell_OEEServiceSYF;
        }

        public List<ActionTime> GetDuration(string factory, string machine, string shift, string date) {
            var data = _ActionTimeServiceSHW_SHD.GetDuration(factory, machine, shift, date);
            return data;
        }
    }
}