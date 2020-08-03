using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OEE_API._Repositories.Interfaces;
using OEE_API._Services.Interfaces;
using OEE_API.Dtos;
using OEE_API.ViewModels;

namespace OEE_API._Services.Services
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IOEE_VNRepository _repoOee_VN;
        private readonly IOEE_MMRepository _repoOee_MM;
        private readonly IOEE_IDRepository _repoOee_ID;
        private readonly ICommonService _serverCommon;

        private readonly IMachineInformationRepository _repoMachineInfomation;
        private readonly IFactoryRepository _repofactory;
        public DashBoardService( IOEE_VNRepository repoOee_VN,
                                    IOEE_MMRepository repoOee_MM,
                                    IOEE_IDRepository repoOee_ID,
                                    ICommonService serverCommon,
                                    IMachineInformationRepository repoMachineInfomation,
                                    IFactoryRepository repofactory) {
            _repoOee_VN = repoOee_VN;
            _repoOee_MM = repoOee_MM;
            _repoOee_ID = repoOee_ID;
            _serverCommon = serverCommon;
            _repoMachineInfomation = repoMachineInfomation;
            _repofactory = repofactory;
        }
        public async Task<List<ChartDashBoardViewModel>> DataChartDashBoard(DashBoardParamModel param)
        {
            
            var dataAll = await _serverCommon.GetListOEE(param.factory);
            var data = new List<ChartDashBoardViewModel>();
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
            if(param.factory.Trim() == "ALL") {
                data = dataAll.GroupBy(x => new {x.factory_id}).Select(x => new ChartDashBoardViewModel() {
                        key = x.First().factory_id,
                        value = Math.Round(x.Sum(cl => cl.oee_rate)/x.Count(),0)
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
                    var listfactory = await _repofactory.FindAll().ToListAsync();
                    data = (from a in data join b in listfactory on a.key.Trim() equals b.factory_id.Trim()
                            select new ChartDashBoardViewModel() {
                            key = b.customer_name,
                            value = a.value
                    }).ToList();
            }
            if(param.factory != "ALL") {
                data = dataAll.Where(x => x.factory_id.Trim() == param.factory.Trim())
                .GroupBy(x => new {x.factory_id, x.building_id})
                .Select(x => new ChartDashBoardViewModel() {
                    key = x.First().building_id,
                    value = Math.Round(x.Sum(cl => cl.oee_rate)/x.Count(),0)
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
                    value = Math.Round(x.Sum(cl => cl.oee_rate)/x.Count(),0)
                }).ToList();
            }
            if (param.machine_id != "ALL") {
                var listMachine = await _serverCommon.ListMachineID(param.factory, param.building, param.machine_id);
                data = dataAll.Where(x => x.factory_id.Trim() == param.factory.Trim() &&
                                    x.building_id.Trim() == param.building.Trim() &&
                                    listMachine.Contains(x.machine_id))
                                    .GroupBy(x => new {x.factory_id, x.building_id, x.machine_id})
                                    .Select(x => new ChartDashBoardViewModel() {
                                        key = x.First().machine_id,
                                        value = Math.Round(x.Sum(cl => cl.oee_rate)/x.Count(),0)
                                    }).ToList();
            }
            return data;
        }
    }
}