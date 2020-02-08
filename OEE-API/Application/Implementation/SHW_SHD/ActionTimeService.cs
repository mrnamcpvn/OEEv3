using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OEE_API.Application.Interfaces.SHW_SHD;
using OEE_API.Data.Interfaces;
using OEE_API.Models.SHW_SHD;
using OEE_API.Utilities;

namespace OEE_API.Application.Implementation.SHW_SHD
{
    public class ActionTimeService : IActionTimeService
    {
        private readonly IRepositorySHW_SHD<ActionTime, int> _repositorySHW_SHD;
        private readonly IRepositorySHW_SHD<ShiftTime, string> _repositoryShiftTime;

        public ActionTimeService(
            IRepositorySHW_SHD<ActionTime, int> repositorySHW_SHD,
            IRepositorySHW_SHD<ShiftTime, string> repositoryShiftTime
            )
        {
            _repositorySHW_SHD = repositorySHW_SHD;
            _repositoryShiftTime = repositoryShiftTime;
        }

        // Lấy ra tất cả building của bảng ActionTime theo factory
        public async Task<List<string>> GetListBuildingActionTime(string factory)
        {

            var data = await _repositorySHW_SHD.FindAll(x => x.factory_id == factory)
                        .GroupBy(x => x.building_id)
                        .OrderBy(g => g.Max(m => m.building_id))
                        .Select(x => x.Key.Trim())
                        .ToListAsync();
            return data;
        }

        // Lấy ra tất cả machine của bảng ActionTime theo factory và building
        public async Task<List<string>> GetListMachineActionTime(string factory, string building)
        {

            var data = await _repositorySHW_SHD.FindAll(x => x.factory_id == factory && building == "other" ? x.building_id == null : x.building_id == building)
                        .GroupBy(x => x.machine_id)
                        .OrderBy(g => g.Max(m => m.machine_id))
                        .Select(x => x.Key.Trim())
                        .ToListAsync();

            return data;
        }

        public List<ActionTime> GetDuration(string factory, string machine, string shift, string date)
        {
            DateTime? day = Convert.ToDateTime(date);
            factory = factory == "SHW" ? "SHC" : "CB";

            var data = _repositorySHW_SHD.FindAll(x =>
                x.factory_id == factory &&
                x.machine_id == machine.Trim() &&
                shift == "0" ? 1 == 1 : x.shift == shift &&
                x.shiftdate == day
            ).ToList();
            return data;
        }
    }
}