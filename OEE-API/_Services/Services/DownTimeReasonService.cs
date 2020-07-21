using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OEE_API._Repositories.Interfaces;
using OEE_API._Services.Interfaces;
using OEE_API.Helpers;
using OEE_API.Models;
using OEE_API.ViewModels;

namespace OEE_API._Services.Services
{
    public class DownTimeReasonService : IDownTimeReasonService
    {
        private readonly IActionTimeForOEERepository _repoDownTimeReson;
        private readonly IMachineInformationRepository _repoMachineInformation;
        private readonly ICommonService _serverCommon;
        public DownTimeReasonService(IActionTimeForOEERepository repoDownTimeReson,
                                        IMachineInformationRepository repoMachineInformation,
                                        ICommonService serverCommon)
        {
            _repoDownTimeReson = repoDownTimeReson;
            _repoMachineInformation = repoMachineInformation;
            _serverCommon = serverCommon;
        }
        public async Task<PageListUtility<ChartReason>> GetDataChart(string factory, string building, string machine, string machine_type, string shift, string date, int page = 1)
        {
            if (factory.Trim() == "ALL")
            {
                return null;
            }
            else
            {
                var machineFirst = new M_ActionTimeForOEE();
                var dataAll = await _repoDownTimeReson.GetAll()
                    .Where(x => x.shift_date == Convert.ToDateTime(date))
                    .ToListAsync();
                if (shift.Trim() != "0")
                {
                    dataAll = dataAll.Where(x => x.shift_id.ToString() == shift.ToString()).ToList();
                }
                if (factory != "ALL")
                {
                    dataAll = dataAll.Where(x => x.factory_id.Trim() == factory.Trim()).ToList();
                    machineFirst = dataAll.FirstOrDefault();
                }
                if (building != "ALL")
                {
                    dataAll = dataAll.Where(x => x.factory_id.Trim() == factory.Trim() &&
                                            x.building_id.Trim() == building.Trim()).ToList();
                    machineFirst = dataAll.FirstOrDefault();
                }

                if (machine_type != "ALL")
                {
                    var machines = await _serverCommon.ListMachineID(factory, building, machine_type);
                    dataAll = dataAll.Where(x => x.factory_id.Trim() == factory.Trim() &&
                                            x.building_id.Trim() == building.Trim() &&
                                            machines.Contains(x.machine_id.Trim())).ToList();
                    machineFirst = dataAll.Where(x => x.factory_id.Trim() == factory.Trim() &&
                                            x.building_id.Trim() == building.Trim() &&
                                            machines.Contains(x.machine_id.Trim())).FirstOrDefault();
                }
                if (machine != "ALL")
                {
                    dataAll = dataAll.Where(x => x.factory_id.Trim() == factory.Trim() &&
                                            x.building_id.Trim() == building.Trim() &&
                                            x.machine_id.Trim() == machine.Trim()).ToList();
                    machineFirst = dataAll.FirstOrDefault();
                }
                if (dataAll.Count() > 0)
                {
                    DbFunctions dfunc = null;
                    DateTime day = Convert.ToDateTime(date);
                    var machineName = machineFirst.machine_id;
                    var data = dataAll.Where(x => x.machine_id.Trim() == machineFirst.machine_id.Trim()).Select(x => new ChartReason()
                    {
                        id = x.id,
                        title = x.is_work_time == true ? "RUN" : "IDLE",
                        start_time = x.start_time,
                        end_time = x.end_time,
                        diffTime = SqlServerDbFunctionsExtensions.DateDiffMinute(dfunc, x.start_time, x.end_time),
                        factory_id = x.factory_id,
                        machine_id = x.machine_id,
                        building_id = x.building_id,
                        shift_id = x.shift_id.ToString(),
                        shift_date = x.shift_date,
                        isEdit = false,
                    }).OrderByDescending(x => x.title).ToList();
                    var dataTable = data.Where(x => x.title == "IDLE").ToList();
                    return await PageListUtility<ChartReason>.PageListAsyncChartReason(data, dataTable, machineName, page);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}