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
using OEE_API.Helpers;
using OEE_API.ViewModels;

namespace OEE_API._Services.Services
{
    
    public class TrendService : ITrendService
    {
        private readonly IOEE_VNRepository _repoOee_VN;
        private readonly IOEE_MMRepository _repoOee_MM;
        private readonly IOEE_IDRepository _repoOee_ID;
        private readonly ICommonService _serverCommon;
        private readonly IMachineInformationRepository _repoMachineInfomation;
        private readonly IFactoryRepository _repofactory;
        private readonly IMapper _mapper;
        private readonly MapperConfiguration _configMapper;
        public TrendService(    IOEE_VNRepository repoOee_VN,
                                IOEE_MMRepository repoOee_MM,
                                IOEE_IDRepository repoOee_ID,
                                ICommonService serverCommon,
                                IMachineInformationRepository repoMachineInfomation,
                                IFactoryRepository repofactory,
                                IMapper mapper,
                                MapperConfiguration configMapper) {
            _repoOee_VN = repoOee_VN;
            _repoOee_MM = repoOee_MM;
            _repoOee_ID = repoOee_ID;
            _serverCommon = serverCommon;
            _repoMachineInfomation = repoMachineInfomation;
            _repofactory = repofactory;
            _mapper = mapper;
            _configMapper = configMapper;
        }

        public  List<ChartTrendViewModel> DataChartWeek(List<string> names, TrendParamModel param, string nameType, List<M_OEE_Dto> data, List<string> listDate)
        {
            var dataChart = new List<ChartTrendViewModel>();
            foreach (var nameItem in names)
            {
                var dataOf = new List<M_OEE_Dto>();
                    if (nameType == "all") {
                        dataOf = data.Where(x => x.factory_id.Trim() == nameItem).ToList();
                    }
                    else if (nameType == "factory") {
                        dataOf = data.Where(x => x.factory_id.Trim() == param.factory.Trim() &&
                            x.building_id.Trim() == nameItem.Trim()).ToList();
                    } else if(nameType == "building" || nameType == "machine") {
                        dataOf = data.Where(x => x.factory_id.Trim() == param.factory.Trim() &&
                            x.building_id.Trim() == param.building.Trim() &&
                            x.machine_id.Trim() == nameItem.Trim()).ToList();
                    }
                    var itemTrendChart = new ChartTrendViewModel();
                    itemTrendChart.name = nameItem;
                    var dataCharts = new List<decimal>();
                    foreach (var dateItem in listDate)
                    {
                        var dataItem = dataOf.Where(x => x.shift_date == Convert.ToDateTime(dateItem)).ToList();
                        if(dataItem.Count() > 0) {
                            var dataItem1 = dataItem.GroupBy(x => x.shift_date).Select(x => new {
                                value = Math.Round(x.Sum(cl => cl.oee_rate)/(x.Count()),0)}).FirstOrDefault();
                            dataCharts.Add(dataItem1.value);
                        } else {
                                dataCharts.Add(0);
                        }
                    }
                    itemTrendChart.data = dataCharts;
                    dataChart.Add(itemTrendChart);
            }
            return dataChart;
        }

        public List<ChartTrendViewModel> DataChartMonth(List<string> names, TrendParamModel param, string nameType, List<M_OEE_Dto> data, List<WeekViewModel> weeks)
        {
            var dataChart = new List<ChartTrendViewModel>();
            var dataOf = new List<M_OEE_Dto>();
            foreach (var nameItem in names)
            {
                if(nameType == "all") {
                    dataOf = data.Where(x => x.factory_id.Trim() == nameItem.Trim()).ToList();
                } else if(nameType == "factory") {
                    dataOf = data.Where(x => x.factory_id.Trim() == param.factory.Trim() &&
                                        x.building_id.Trim() == nameItem.Trim()).ToList();
                } else if(nameType == "building" || nameType == "machine") {
                    dataOf = data.Where(x => x.factory_id.Trim() == param.factory.Trim() &&
                                        x.building_id.Trim() == param.building.Trim() &&
                                        x.machine_id.Trim() == nameItem.Trim()).ToList();
                }
                var itemTrendChart = new ChartTrendViewModel();
                itemTrendChart.name = nameItem;
                var dataCharts = new List<decimal>();
                foreach (var dateItem in weeks)
                {
                    var dataItem = dataOf.Where(x => x.shift_date >= dateItem.weekStart &&
                        x.shift_date <= dateItem.weekFinish).ToList();
                    if(dataItem.Count() > 0) {
                        var dataItem1 = dataItem.GroupBy(x => x.shift_date).Select(x => new {
                        value = Math.Round(x.Sum(cl => cl.oee_rate)/(x.Count()),0)}).FirstOrDefault();
                        dataCharts.Add(dataItem1.value);
                    } else {
                        dataCharts.Add(0);
                    }
                }
                itemTrendChart.data = dataCharts;
                dataChart.Add(itemTrendChart);
            }
            return dataChart;
        }
        public async Task<object> DataCharTrendMonth(TrendParamModel param)
        {
                // -------------------------------- Data ListTime------------------------------------//
                var numberMonth = param.numberTime != string.Empty ? Convert.ToInt32(param.numberTime) : 1;
                var weeks = Util.ListWeekOfYear().FindAll(x => x.weekStart.Value.Month == numberMonth);

                List<string> listTime = new List<string>();
                List<ChartTrendViewModel> dataChart = new List<ChartTrendViewModel>();

                foreach (var week in weeks)
                {
                    listTime.Add(week.weekStart.Value.ToString("MM/dd") + " to " + week.weekFinish.Value.ToString("MM/dd") + "\n (Week " + week.weekNum + ")");
                }

                 // -------------------------------- DataChart---------------------------------------//
                var dataAll = await _serverCommon.GetListOEE(param.factory);
                if (param.shift.ToString() != "0") {
                    dataAll = dataAll.Where(x => x.shift_id.ToString() == param.shift).ToList();
                }
                if(param.factory.Trim() == "ALL") {
                    var factorys = await _repofactory.FindAll().GroupBy(x => x.factory_id).Select(x => x.Key).ToListAsync();
                    dataChart = this.DataChartMonth(factorys,param,"all",dataAll,weeks);
                }
                if(param.factory != "ALL") {
                    dataChart = new List<ChartTrendViewModel>();
                    var buildings = await _serverCommon.GetListBuilding(param.factory);
                    dataChart = this.DataChartMonth(buildings,param,"factory",dataAll,weeks);
                }
                if(param.building != "ALL") {
                    dataChart = new List<ChartTrendViewModel>();
                    var machines  = await _serverCommon.ListMachineID(param.factory, param.building);
                    dataChart = this.DataChartMonth(machines,param,"building",dataAll,weeks);
                }
                if(param.machine_type != "ALL") {
                    dataChart = new List<ChartTrendViewModel>();
                    var machines  = await _serverCommon.ListMachineID(param.factory,param.building,param.machine_type);
                    dataChart = this.DataChartMonth(machines,param,"machine",dataAll,weeks);
                }

                var result = new {
                    dataChart,
                    listTime
                };
                return result;
        }

        public async Task<object> DataCharTrendWeek(TrendParamModel param)
        {
                var numberWeek = param.numberTime != string.Empty ? Convert.ToInt32(param.numberTime) : 1;
                // take out a week to follow 'numberWeek
                var week = Util.ListWeekOfYear().Find(x => x.weekNum == numberWeek);
                DateTime dateStart = week.weekStart.Value;
                DateTime dateEnd = week.weekFinish.Value;

                // Take out list day of week
                List<DateTime> rangerDate = Util.GetRangerDates(dateStart, dateEnd);
                List<ChartTrendViewModel> dataChart = new List<ChartTrendViewModel>();

                // Reformat the list of dates to display outside
                List<string> listTime = rangerDate.ConvertAll(x => x.ToString("MM/dd"));
                List<string> listDate = Util.ListDate(listTime);

                var dataAll = await _serverCommon.GetListOEE(param.factory);
                if (param.shift.ToString() != "0") {
                    dataAll = dataAll.Where(x => x.shift_id.ToString() == param.shift).ToList();
                }
                if(param.factory.Trim() == "ALL") {
                    var factorys = await _repofactory.FindAll().GroupBy(x => x.factory_id).Select(x => x.Key).ToListAsync();
                    dataChart = this.DataChartWeek(factorys,param,"all",dataAll,listDate);
                }
                if(param.factory != "ALL") {
                    dataChart = new List<ChartTrendViewModel>();
                    var buildings = await _serverCommon.GetListBuilding(param.factory);
                    dataChart = this.DataChartWeek(buildings,param,"factory",dataAll,listDate);
                }
                if(param.building != "ALL") {
                    dataChart = new List<ChartTrendViewModel>();
                    var machines  = await _serverCommon.ListMachineID(param.factory, param.building);
                    dataChart = this.DataChartWeek(machines,param,"building",dataAll,listDate);
                }
                if(param.machine_type != "ALL") {
                    dataChart = new List<ChartTrendViewModel>();
                    var machines  = await _serverCommon.ListMachineID(param.factory, param.building,param.machine_type);
                    dataChart = this.DataChartWeek(machines,param,"machine",dataAll,listDate);
                }
                var result = new {
                    dataChart,
                    listTime
                };
                return result;
        }

    }
}