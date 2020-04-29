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
    public class ActionTimeService : IActionTimeService
    {
        private readonly IRepositorySHW_SHD<ActionTime, int> _repositorySHW_SHD;

        private readonly IRepositorySHW_SHD<ShiftTime, string> _repositoryShiftTime;
        private readonly IRepositorySHW_SHD<RestTime, string> _repositoryRestTime;
        private readonly IRepositorySHW_SHD<MaintenanceTime, string> _repositoryMaintenanceTime;
        private readonly IRepositorySHW_SHD<DowntimeReason, string> _repositoryDowntimeReason;
        private readonly IRepositorySHW_SHD<DowntimeDetail, string> _repositoryDowntimeDetail;
        private readonly MapperConfiguration _configureMapper;
        private readonly IMapper _mapper;

        public ActionTimeService(
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

        // Lấy ra machine đầu tiên
        public async Task<ActionTime> GetFirstMachineActionTime(string factory, string building, string shift, string machine)
        {
            // factory = factory == "SHW" ? "SHC" : "CB";
            if (machine == null)
            {
                var data = await _repositorySHW_SHD.FindAll(x => x.factory_id == factory && x.building_id == building).FirstOrDefaultAsync();
                return data;
            }
            else
            {
                var data = await _repositorySHW_SHD.FindAll(x => x.factory_id == factory && x.building_id == building && x.machine_id == machine).FirstOrDefaultAsync();
                return data;
            }
        }
        public async Task<List<string>> GetDowntimeReasonDetail(string reason_1)
        {
            // factory = factory == "SHW" ? "SHC" : "CB";
            if (reason_1 == "0" || reason_1 == null)
            {
                return await _repositoryDowntimeReason.FindAll().Select(x => x.reason_1.Trim()).Distinct().ToListAsync();

            }
            else if (reason_1 != "0" && reason_1 != null)
            {
                return await _repositoryDowntimeReason.FindAll(x => x.reason_1 == reason_1.Trim()).Select(x => x.reason_2).ToListAsync();
            }
            else return null;
        }
        public async Task<PageListUtility<ChartReason>> GetDuration(string factory, string building, string machine, string shift, string date, int page = 1)
        {
            try
            {
                DbFunctions dfunc = null;
                DateTime day = Convert.ToDateTime(date);
                factory = factory == "SHW" ? "SHC" : "CB";
                var firstMachine = this.GetFirstMachineActionTime(factory, building, shift, machine);
                var abc = firstMachine.Result.machine_id;

                var data = _repositorySHW_SHD.FindAll(x =>
                   x.factory_id == firstMachine.Result.factory_id &&
                   x.machine_id == firstMachine.Result.machine_id.Trim() &&
                   (shift == "0" ? 1 == 1 : x.shift == shift) &&
                   x.shiftdate == day
                ).Select(x =>
                    new ChartReason
                    {
                        id = x.id,
                        title = x.status.ToUpper(),
                        start_time = x.start_time,
                        end_time = x.end_time,
                        diffTime = SqlServerDbFunctionsExtensions.DateDiffMinute(dfunc, x.start_time, x.end_time),
                        factory_id = x.factory_id,
                        machine_id = x.machine_id,
                        building_id = x.building_id,
                        shift_id = x.shift,
                        shift_date = x.shiftdate
                    }
                ).OrderByDescending(x => x.title);
                var dataTable = data.Where(x => x.title == "IDLE");
                return await PageListUtility<ChartReason>.PageListAsyncChartReason(data, dataTable, page);

                // return data.GetEnumerator();
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }
        private bool Check_Involved(DowntimeDetail a, DowntimeDetail b)
        {
            //Return rest time
            var result = new List<DowntimeDetail>();
            TimePeriodCollection subPeriod = new TimePeriodCollection { };
            TimePeriodCollection sourcePeriods = new TimePeriodCollection { };
            if (a.start_time.HasValue && a.end_time.HasValue)
            {
                sourcePeriods.Add(new TimeRange(a.start_time.Value, a.end_time.Value));
            }
            // Get involved time

            subPeriod.Add(new TimeRange(a.start_time.Value, a.end_time.Value));
            // Save the whole rest time without maint time
            TimePeriodSubtractor<TimeRange> subtractor = new TimePeriodSubtractor<TimeRange>();
            ITimePeriodCollection subtractedPeriod = subtractor.SubtractPeriods(sourcePeriods, subPeriod);
            if (subtractedPeriod.Count > 0)
            {
                return true;
            }
            else return false;
        }
        private void Check_Rest_Maint(DowntimeDetail a, List<DowntimeDetail> list, string note)
        {
            //Return rest time
            var result = new List<DowntimeDetail>();
            TimePeriodCollection subPeriod = new TimePeriodCollection { };
            TimePeriodCollection sourcePeriods = new TimePeriodCollection { };
            if (a.start_time.HasValue && a.end_time.HasValue)
            {
                sourcePeriods.Add(new TimeRange(a.start_time.Value, a.end_time.Value));
            }
            // Get involved time
            for (int i = 0; i < list.Count; i++)
            {
                var model = new DowntimeDetail();

                // end_time middle of rest time
                if ((list[i].end_time <= a.end_time && list[i].end_time >= a.start_time) ||
                    (list[i].start_time >= a.start_time && list[i].start_time <= a.end_time))
                {
                    subPeriod.Add(new TimeRange(list[i].start_time.Value, list[i].end_time.Value));
                }
            }
            // Save the whole rest time without maint time
            if (subPeriod.Count == 0)
            {
                a.remark = "R";
                a.duration = (Int32)(a.end_time - a.start_time).Value.Minutes;
                _repositoryDowntimeDetail.Add(a);
            }
            // Subtract Maint Time- Rest time, get the subtracted and save.
            else
            {
                TimePeriodSubtractor<TimeRange> subtractor = new TimePeriodSubtractor<TimeRange>();
                ITimePeriodCollection subtractedPeriod = subtractor.SubtractPeriods(sourcePeriods, subPeriod);
                foreach (TimeRange subtracted in subtractedPeriod)
                {
                    var res = new DowntimeDetail();
                    res = a;
                    res.start_time = subtracted.Start;
                    res.end_time = subtracted.End;
                    res.duration = subtracted.Duration.Minutes;
                    res.notes = "R";
                    _repositoryDowntimeDetail.Add(res);
                }
            }
        }
        private List<DowntimeDetail> CheckAbnormal(DowntimeDetail a, List<DowntimeDetail> list, string note)
        {
            // List:  Rest, Maint, Shift Time
            // Result: Idle Time - List
            DateTime moment = new DateTime(2020, 4, 20);

            var result = new List<DowntimeDetail>();
            TimePeriodCollection sourcePeriods = new TimePeriodCollection
            {
            };
            TimePeriodCollection subPeriod = new TimePeriodCollection { };
            if (a.start_time.HasValue && a.end_time.HasValue)
            {
                sourcePeriods.Add(new TimeRange(a.start_time.Value, a.end_time.Value));
            }
            else return null;
            // Get involved time
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    var model = new DowntimeDetail();
                    subPeriod.Add(new TimeRange(list[i].start_time.Value, list[i].end_time.Value));
                }
                TimePeriodSubtractor<TimeRange> subtractor = new TimePeriodSubtractor<TimeRange>();
                ITimePeriodCollection subtractedPeriod = subtractor.SubtractPeriods(sourcePeriods, subPeriod);
                if (subtractedPeriod.Count > 0)
                {
                    foreach (TimeRange subtracted in subtractedPeriod)
                    {
                        var res = new DowntimeDetail();
                        res = a;
                        res.start_time = subtracted.Start;
                        res.end_time = subtracted.End;
                        res.notes = "Abnormal";
                        res.duration = subtracted.Duration.Minutes;
                        result.Add(res);
                    }
                    return result;
                }
                else return null;
            }
            // Save the whole rest time without maint time
            else
            {
                a.remark = "Abnormal";
                a.duration = (Int32)(a.end_time - a.start_time).Value.Minutes;
                _repositoryDowntimeDetail.Add(a);
                //   _repositoryDowntimeDetail.SaveAll();
                return null;
            }
            // Subtract Maint Time- Rest time-ShiftTime, get the subtracted and save. 
           
        }

        public async Task<bool> AddDowntimeReason(ChartReason chartReason)
        {
            // var test = _mapper.Map<DowntimeDetail>(chartReason);
            var a = chartReason;
            var id = await _repositoryDowntimeReason.FindSingle(x => x.reason_1 == chartReason.reason_1 && x.reason_2 == chartReason.reason_2);
            var model = new DowntimeDetail();
            var list = new List<DowntimeDetail>();
            model.machine_id = chartReason.machine_id;
            model.factory_id = chartReason.factory_id;
            model.start_time = chartReason.start_time;
            model.end_time = chartReason.end_time;
            model.notes = chartReason.reason_note;
            model.actionTime_id = chartReason.id;
            model.reason_id = id.id;
           // TimeSpan? dur = model.start_time - model.end_time;
            model.duration = (Int32)((TimeSpan)(a.end_time - a.start_time)).TotalMinutes;
            model.shiftdate = chartReason.shift_date;

            var factory = a.factory_id == "SHW" ? "SHC" : "CB";
            // Get all periods
            var restTime = await _repositoryRestTime.FindAll(x => (x.building_id == a.machine_id && x.factory_id == a.factory_id)).FirstOrDefaultAsync();

            var maintTime = await _repositoryMaintenanceTime.FindAll(x => (x.start_time >= a.start_time &&
                                                                  x.start_time.Value <= a.end_time && x.machine_id == a.machine_id)
                                                                  ||
                                                               (x.end_time >= a.start_time &&
                                                                 x.end_time.Value <= a.end_time && x.machine_id == a.machine_id)).ToListAsync();

            var shiftTime = await _repositoryShiftTime.FindAll(x => x.factory_id == factory && x.building_id == a.building_id).FirstOrDefaultAsync();
            //Split Maint-Rest
            if (maintTime.Count > 0)
            {
                foreach (var maint in maintTime)
                {
                    var m = model.GetClone();
                    m.start_time = maint.start_time;
                    m.end_time = maint.end_time;
                    m.duration = ((TimeSpan)(m.end_time.Value - m.start_time.Value)).Minutes;
                    m.remark = "M";
                    _repositoryDowntimeDetail.Add(m);
                    list.Add(m);
                }
                if (restTime != null)
                {
                    var r = model.GetClone();
                    r.start_time = r.shiftdate + restTime.start_time;
                    r.end_time = r.shiftdate + restTime.end_time;
                    r.remark = "R";
                    if (Check_Involved(model, r))
                    {
                        Check_Rest_Maint(r, list, "R");
                    }
                    else { };
                }
                if (shiftTime != null)
                {
                    var s = model.GetClone();
                    s.start_time = s.shiftdate + shiftTime.start_time;
                    s.end_time = s.shiftdate + shiftTime.end_time;
                }

            }
            else
            {
                if (restTime != null)
                {
                    var r = model.GetClone();
                    r.start_time = r.shiftdate + restTime.start_time;
                    r.end_time = r.shiftdate + restTime.end_time;
                    r.remark = "R";
                    if (Check_Involved(model, r))
                    {
                        list.Add(r);
                    }
                    else { };
                    // list.Add(result);
                }
            }
            try
            {
                CheckAbnormal(model, list, "Abnormal");
                await _repositoryDowntimeDetail.SaveAll();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}