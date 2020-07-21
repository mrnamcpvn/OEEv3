using System.Security.Cryptography.X509Certificates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OEE_API._Repositories.Interfaces;
using OEE_API._Services.Interfaces;
using OEE_API.Dtos;

namespace OEE_API._Services.Services
{
    public class DowntimeAnalysisService : IDowntimeAnalysisService
    {
        private readonly IActionTimeForOEERepository _actionTimeForOEERepository;
        private readonly IDowntimeReasonRepository _downtimeReasonRepository;
        private readonly IDowntimeRecordRepository _downtimeRecordRepository;
        private readonly IMachineInformationRepository _machineInformationRepository;

        public DowntimeAnalysisService(IActionTimeForOEERepository actionTimeForOEERepository, IDowntimeReasonRepository downtimeReasonRepository, IDowntimeRecordRepository downtimeRecordRepository, IMachineInformationRepository machineInformationRepository)
        {
            _actionTimeForOEERepository = actionTimeForOEERepository;
            _downtimeReasonRepository = downtimeReasonRepository;
            _downtimeRecordRepository = downtimeRecordRepository;
            _machineInformationRepository = machineInformationRepository;
        }

        public async Task<object> GetDownTimeAnalysis(string factory, string building, int? machine_type, string machine, int? shift, string date, string dateTo)
        {
            DateTime timeFrom = Convert.ToDateTime(date);
            DateTime timeTo = Convert.ToDateTime(dateTo);
            var queryActionTime = _actionTimeForOEERepository.FindAll().Where(x => timeFrom <= x.shift_date && timeTo >= x.shift_date);
            var queryDowtimeRecord = _downtimeRecordRepository.FindAll();


            if (factory != null || factory != "ALL")
                queryActionTime = queryActionTime.Where(x => x.factory_id.Trim() == factory.Trim());

            if (building != null || building != "ALL")
                queryActionTime = queryActionTime.Where(x => x.building_id.Trim() == building.Trim());

            if (shift != 0)
                queryActionTime = queryActionTime.Where(x => x.shift_id == shift);


            if (machine_type != 0)
            {
                var listMachines = await _machineInformationRepository.FindAll().Where(x => x.machine_type == machine_type).Select(x => x.machine_id).ToListAsync();
                if (machine != "ALL" && machine != null)
                    listMachines = listMachines.Where(x => x.Trim() == machine.Trim()).ToList();

                queryActionTime = queryActionTime.Where(x => listMachines.Contains(x.machine_id));
            }

            //Join table ActionTime vs DowTimeRecord  => id,duration , idDowtime
            var queryJoin = queryActionTime.Join(queryDowtimeRecord, x => x.id, y => y.action_time_id, (x, y) => new
            {
                x.id,
                y.downtime_reason_id,
                x.duration_sec
            }
            ).ToList();

            //GetData Reason
            var data = await GetListReason();


            foreach (var item in data)
            {
                decimal? num = queryJoin.Where(x => x.downtime_reason_id == item.id).Sum(x => x.duration_sec);
                item.duration = num;
            }

            //Group by Reason1
            var dataGroup = data.GroupBy(x => x.reason_1).ToList();
            List<ReasonAnalysis_Dto> result = new List<ReasonAnalysis_Dto>();
            foreach (var item in dataGroup)
            {
                ReasonAnalysis_Dto rea_2 = new ReasonAnalysis_Dto();
                var num = item.Sum(x => x.duration);
                rea_2.reason_1 = item.Key;
                rea_2.duration = num.Value;
                result.Add(rea_2);
            }
            return new
            {
                resA = data.OrderByDescending(x => x.duration).Take(10),
                resB = result
            };
        }


        //Get All Reason
        private async Task<List<ReasonAnalysis_Dto>> GetListReason()
        {
            return await _downtimeReasonRepository.FindAll().Select(x => new ReasonAnalysis_Dto
            {
                id = x.id,
                reason_1 = x.reason.Trim(),
                reason_2 = x.reason_type.Trim(),
                duration = 0
            }).ToListAsync();
        }
    }
}