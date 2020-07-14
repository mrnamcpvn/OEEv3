using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OEE_API._Repositories.Interfaces;
using OEE_API._Services.Interfaces;
using OEE_API.Models;
using OEE_API.ViewModels;

namespace OEE_API._Services.Services
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IOEE_VNRepository _repoOee_VN;
        private readonly ICommonService _serverCommon;
        private readonly IMachineInformationRepository _repoMachineInfomation;
        public AvailabilityService( IOEE_VNRepository repoOee_VN,
                                    ICommonService serverCommon,
                                    IMachineInformationRepository repoMachineInfomation) {
            _repoOee_VN = repoOee_VN;
            _serverCommon = serverCommon;
            _repoMachineInfomation = repoMachineInfomation;
        }

        public async Task<List<ChartDashBoardViewModel>> LoadDataChart(DashBoardParamModel param)
        {
            var dataAll = await _repoOee_VN.FindAll().ToListAsync();
            if(param.date != null && param.dateTo != null) {
                dataAll = dataAll
                    .Where(x => x.shift_date >= Convert.ToDateTime(param.date) &&
                    x.shift_date <= Convert.ToDateTime(param.dateTo)).ToList();
            }
            if (param.shift_id.ToString() != "0") {
                dataAll = dataAll.Where(x => x.shift_id.ToString() == param.shift_id).ToList();
            }
            if (param.month.ToString() != null) {
                dataAll = dataAll.Where(x => x.month.ToString() == param.month).ToList();
            }

            var data = new List<ChartDashBoardViewModel>();
            if(param.factory == "ALL") {
                data = dataAll.GroupBy(x => new {x.factory_id})
                .Select(x => new ChartDashBoardViewModel() {
                    key = x.First().factory_id,
                    value = Math.Round(100*(x.Sum(cl => cl.run_time_sec - cl.maintenance_run_time_sec))/(x.Sum(cl => cl.work_time_sec - cl.maintenance_work_time_sec)),0)
                }).ToList();
                var factorys = await _serverCommon.GetListFactory();
                var data1 = data.Select(x => x.key).ToList();
                foreach (var item in factorys)
                {
                    if(!data1.Contains(item.factory_id)) {
                        var item1 = new ChartDashBoardViewModel();
                        item1.key = item.factory_id; item1.value = null;
                        data.Add(item1);
                    }
                }
            }
            if(param.factory != "ALL") {
                data = dataAll.Where(x => x.factory_id.Trim() == param.factory.Trim())
                .GroupBy(x => new {x.factory_id, x.building_id})
                .Select(x => new ChartDashBoardViewModel() {
                    key = x.First().building_id,
                    value = Math.Round(100*(x.Sum(cl => cl.run_time_sec - cl.maintenance_run_time_sec))/(x.Sum(cl => cl.work_time_sec - cl.maintenance_work_time_sec)),0)
                }).ToList();
                var builds = await _serverCommon.GetListBuilding(param.factory);
                var data1 = data.Select(x => x.key).ToList();
                foreach (var item in builds)
                {
                    if(!data1.Contains(item)) {
                        var item1 = new ChartDashBoardViewModel();
                        item1.key = item; item1.value = null;
                        data.Add(item1);
                    }
                }
            }
            if (param.building != "ALL") {
                data = dataAll.Where(x => x.factory_id.Trim() == param.factory.Trim() && x.building_id.Trim() == param.building.Trim())
                .GroupBy(x => new {x.factory_id, x.building_id, x.machine_id})
                .Select(x => new ChartDashBoardViewModel() {
                    key = x.First().machine_id,
                    value = Math.Round(100*(x.Sum(cl => cl.run_time_sec - cl.maintenance_run_time_sec))/(x.Sum(cl => cl.work_time_sec - cl.maintenance_work_time_sec)),0)
                }).ToList();
            }
            if (param.machine_id != "ALL") {
                var listMachine = await _repoMachineInfomation.FindAll()
                                    .Where(x => x.machine_type.ToString().Trim() == param.machine_id.Trim())
                                    .Select(x => x.machine_id.Trim()).ToListAsync();
                data = dataAll.Where(x => x.factory_id.Trim() == param.factory.Trim() &&
                    x.building_id.Trim() == param.building.Trim() &&
                    listMachine.Contains(x.machine_id))
                .GroupBy(x => new {x.factory_id, x.building_id, x.machine_id})
                .Select(x => new ChartDashBoardViewModel() {
                    key = x.First().machine_id,
                    value = Math.Round(100*(x.Sum(cl => cl.run_time_sec - cl.maintenance_run_time_sec))/(x.Sum(cl => cl.work_time_sec - cl.maintenance_work_time_sec)),0)
                }).ToList();
            }
            return data;
        }
    }
}