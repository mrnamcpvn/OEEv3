using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OEE_API.Application.Interfaces.SHW_SHD;
using OEE_API.Application.ViewModels;
using OEE_API.Data.Interfaces;
using OEE_API.Models.SHW_SHD;
using OEE_API.Utilities;
using Itenso.TimePeriod;
using CloneExtensions;

namespace OEE_API.Application.Implementation.SHW_SHD
{
    public class DowntimeDetailService : IDownTimeDetailService
    {
        private readonly IRepositorySHW_SHD<ActionTime, int> _repositorySHW_SHD;

        private readonly IRepositorySHW_SHD<ShiftTime, string> _repositoryShiftTime;
        private readonly IRepositorySHW_SHD<RestTime, string> _repositoryRestTime;
        private readonly IRepositorySHW_SHD<MaintenanceTime, string> _repositoryMaintenanceTime;
        private readonly IRepositorySHW_SHD<DowntimeReason, string> _repositoryDowntimeReason;
        private readonly IRepositorySHW_SHD<DowntimeDetail, string> _repositoryDowntimeDetail;
        private readonly MapperConfiguration _configureMapper;
        private readonly IMapper _mapper;
        public DowntimeDetailService(
         IRepositorySHW_SHD<ActionTime, int> repositorySHW_SHD,
         IRepositorySHW_SHD<ShiftTime, string> repositoryShiftTime,
         IRepositorySHW_SHD<RestTime, string> repositoryRestTime,
         IRepositorySHW_SHD<MaintenanceTime, string> repositoryMaintenanceTime,
         IRepositorySHW_SHD<DowntimeReason, string> repositoryDowntimeReason,
         IRepositorySHW_SHD<DowntimeDetail, string> repositoryDowntimeDetail,
         MapperConfiguration configureMapper,
         IMapper mapper
         )
        {
            _repositorySHW_SHD = repositorySHW_SHD;
            _repositoryShiftTime = repositoryShiftTime;
            _repositoryRestTime = repositoryRestTime;
            _repositoryMaintenanceTime = repositoryMaintenanceTime;
            _repositoryDowntimeReason = repositoryDowntimeReason;
            _repositoryDowntimeDetail = repositoryDowntimeDetail;
            _configureMapper = configureMapper;
            _mapper = mapper;
        }
        public async Task<List<ReasonAnalysis>> GetListReason()
        {
            return await _repositoryDowntimeReason.FindAll().Select(x=> new ReasonAnalysis {
                id = x.id,
                reason_1 = x.reason_1,
                reason_2 = x.reason_2,
                duration = 0
            }).ToListAsync();
        }
        public async Task<List<ReasonAnalysis>> GetDownTimeAnalysis(string factory, string building, string machine, string shift, string date)
        {
            Dictionary<string, int> dic = new Dictionary<string, int>();
            // DbFunctions dfunc = null;
            DateTime day = Convert.ToDateTime(date);
            factory = factory == "SHW" ? "SHC" : "CB";
            try
            {
            //  var data = await _ActionTimeServiceSHW_SHD.GetDuration(factory, building, machine, shift, date);
            var result = await _repositoryDowntimeDetail.FindAll(x => 1 == 1
                                                            && (factory == "ALL" ? 1 == 1 : x.factory_id == factory)
                                                            && (building == "ALL" ? 1 == 1 : x.building_id == building)
                                                            && (machine == "ALL" ? 1 == 1 : x.machine_id == machine)
                                                            && (shift == "0" ? 1 == 1 : x.shift_id == shift)
                                                            && (day == null ? 1 == 1 : x.shiftdate == day))
                                                        .Select(z =>
                                                               new ReasonAnalysis
                                                               {
                                                                   id = z.reason_id,
                                                                   duration = z.duration
                                                               }
                                                            ).OrderBy(x=> x.duration).ToListAsync();
                var list = await GetListReason();
                foreach(var item in list){
                    var num = result.FindAll(x=> x.id  == item.id).Select(x=> x.duration).Sum();
                    item.duration = num == null ? 0 : num;
                }
                return list.OrderByDescending(x=> x.duration).ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}