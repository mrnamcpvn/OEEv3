using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OEE_API.Application.Interfaces.SYF;
using OEE_API.Data.Interfaces;
using OEE_API.Models.SYF;
using DbSHW_SHD = OEE_API.Models.SHW_SHD;

namespace OEE_API.Application.Implementation.SYF
{
    public class ActionTimeService : IActionTimeService
    {
        private readonly IRepositorySYF<ActionTime, int> _repositorySYF;
        private readonly IRepositorySHW_SHD<DbSHW_SHD.ShiftTime, string> _repositoryShiftTime;
        public ActionTimeService(
            IRepositorySYF<ActionTime, int> repositorySYF,
            IRepositorySHW_SHD<DbSHW_SHD.ShiftTime, string> repositoryShiftTime
            )
        {
            _repositoryShiftTime = repositoryShiftTime;
            _repositorySYF = repositorySYF;
        }

        // Lấy ra tất cả machine của bảng ActionTime theo factory
        public async Task<List<string>> GetListMachineActionTime(string factory)
        {
            var data = await _repositorySYF.FindAll(x => x.Factory_ID == factory)
                        .GroupBy(x => x.Machine_ID)
                        .OrderBy(g => g.Max(m => m.Machine_ID))
                        .Select(x => x.Key.Trim())
                        .ToListAsync();

            return data;
        }

        public List<ActionTime> GetDuration(string factory, string machine, string shift, string date)
        {
            DateTime? day = Convert.ToDateTime(date);

            var data = _repositorySYF.FindAll(x =>
                x.Factory_ID == factory &&
                x.Machine_ID == machine.Trim() &&
                shift == "0" ? 1 == 1 : x.Shift == Convert.ToInt32(shift) &&
                x.Shiftdate == day
            ).ToList();
            return data;
        }
    }
}