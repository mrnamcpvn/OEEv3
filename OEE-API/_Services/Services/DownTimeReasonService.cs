using System;
using System.Collections.Generic;
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
        public DownTimeReasonService(   IActionTimeForOEERepository repoDownTimeReson,
                                        IMachineInformationRepository repoMachineInformation) {
            _repoDownTimeReson = repoDownTimeReson;
            _repoMachineInformation = repoMachineInformation;
        }
        public async Task<PageListUtility<ChartReason>> GetDataChart(DownTimeReasonParamModel param)
        {
            // throw new System.NotImplementedException();
            var dataAll = await _repoDownTimeReson.GetAll()
            .Where(x => x.shift_date == Convert.ToDateTime(param.date))
            .ToListAsync();
            if(param.factory != "ALL") {
                dataAll = dataAll.Where(x => x.factory_id.Trim() == param.factory.Trim()).ToList();
            }
            if(param.building != "ALL") {
                dataAll = dataAll.Where(x => x.factory_id.Trim() == param.factory.Trim() &&
                                        x.building_id.Trim() == param.building.Trim()).ToList();
            }
            if(param.machine_id != "ALL") {
                dataAll = dataAll.Where(x => x.factory_id.Trim() == param.factory.Trim() &&
                                        x.building_id.Trim() == param.building.Trim() &&
                                        x.machine_id.Trim() == param.machine_id.Trim()).ToList();
            }
            DbFunctions dfunc = null;
            DateTime day = Convert.ToDateTime(param.date);
            var machineModel = await this.GetFirstMachine(param);
            var machineName = machineModel.machine_name;
            var data = dataAll.Where(x => x.machine_id.Trim() == machineModel.machine_id.Trim()).Select(x => new ChartReason() {
                title = x.is_work_time == true? "RUN" : "IDLE",
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
            return await PageListUtility<ChartReason>.PageListAsyncChartReason(data, dataTable, machineName, 1);
        }

        public async Task<M_MachineInformation> GetFirstMachine(DownTimeReasonParamModel param)
        {
            var machines = await _repoMachineInformation.FindAll().ToListAsync();
            var machineModel = new M_MachineInformation();
            if(param.factory.Trim() != "ALL") {
                machineModel = machines
                    .Where(x => x.factory_id.Trim() == param.factory.Trim()).FirstOrDefault();
            }
            if(param.building.Trim() != "ALL") {
                machineModel = machines
                    .Where(x => x.factory_id.Trim() == param.factory.Trim() &&
                            x.building_id.Trim() == param.building.Trim()).FirstOrDefault();
            }
            if(param.machine_type.Trim() != "ALL") {
                machineModel = machines
                    .Where(x => x.factory_id.Trim() == param.factory.Trim() &&
                            x.building_id.Trim() == param.building.Trim() &&
                            x.machine_type.ToString() == param.machine_type.Trim()).FirstOrDefault();
            }

            if(param.machine_id != "ALL") {
                machineModel = machines
                    .Where(x => x.machine_id.Trim() == param.machine_id.Trim()).FirstOrDefault();
            }
            return machineModel;
        }
    }
}