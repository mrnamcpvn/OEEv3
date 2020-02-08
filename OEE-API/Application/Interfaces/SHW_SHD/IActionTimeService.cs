using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OEE_API.Models.SHW_SHD;

namespace OEE_API.Application.Interfaces.SHW_SHD
{
    public interface IActionTimeService
    {
        Task<List<string>> GetListBuildingActionTime(string factory);
        Task<List<string>> GetListMachineActionTime(string factory, string building);
        List<ActionTime> GetDuration(string factory, string machine, string shift, string date);
    }
}