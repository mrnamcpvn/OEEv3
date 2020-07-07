using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OEE_API.Application.ViewModels;
using OEE_API.Models.SHW_SHD;
using OEE_API.Utilities;

namespace OEE_API.Application.Interfaces.SHW_SHD
{
    public interface IActionTimeService
    {
        Task<List<string>> GetDowntimeReasonDetail(string reason_1);
                Task<List<string>> GetListBuildingActionTime(string factory);
        Task<List<string>> GetListMachineActionTime(string factory, string building, string machine_type);
              Task<List<string>> GetListMachineType(string factory, string building);
        Task<PageListUtility<ChartReason>> GetDuration(string factory, string building, string machine, string machine_type, string shift, string date, int page = 1);
        Task<ActionTime> GetFirstMachineActionTime(string factory, string building, string shift, string machine, string machine_type);
        Task<bool> AddDowntimeReason(ChartReason chartReason);
        Task<ReasonDetail> GetReasons(int item);

    }
}