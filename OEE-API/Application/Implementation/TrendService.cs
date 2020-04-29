using ServiceSHW_SHD = OEE_API.Application.Interfaces.SHW_SHD;
using OEE_API.Utilities;
using OEE_API.Application.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OEE_API.Application.Interfaces;

namespace OEE_API.Application.Implementation
{
    public class TrendService : ITrendService
    {
        ServiceSHW_SHD.ICell_OEEService _Cell_OEEService;
        public TrendService(
           ServiceSHW_SHD.ICell_OEEService Cell_OEEService
        )
        {
            _Cell_OEEService = Cell_OEEService;
        }

        // Function take out availability by week of all factory (List by day of the week) 
        public async Task<object> GetTrendByWeek(string factory, string building, string shift, int numberWeek)
        {
            // take out a week to follow 'numberWeek
            var week = Util.ListWeekOfYear().Find(x => x.weekNum == numberWeek);

            DateTime dateStart = week.weekStart.Value;
            DateTime dateEnd = week.weekFinish.Value;

            // Take out list day of week
            List<DateTime> rangerDate = Util.GetRangerDates(dateStart, dateEnd);

            string[] listFactory = new string[4] { "SHW", "SHD", "SHB", "SY2" };
            List<ChartTrendViewModel> dataChart = new List<ChartTrendViewModel>();

            // Reformat the list of dates to display outside
            List<string> listTime = rangerDate.ConvertAll(x => x.ToString("MM/dd"));

            if (factory == "ALL")
            {
                var dataSHW_SHD = await _Cell_OEEService.GetAllCellOEEByDate("",dateStart, dateEnd);
                var dataSHB = await _Cell_OEEService.GetAllCellOEEByDate("SHB",dateStart, dateEnd);
                var dataSYF = await _Cell_OEEService.GetAllCellOEEByDate("SHY",dateStart, dateEnd);

                // Duyệt qua từng factory 
                foreach (var itemFactory in listFactory)
                {
                    // khai báo danh sách data availability
                    List<int> dataChild = new List<int>();
                    ChartTrendViewModel chartTrendModel = new ChartTrendViewModel();

                    if (itemFactory == "SHW")
                    {
                        // Duyệt qua danh sách từng ngày 
                        foreach (var itemDate in rangerDate)
                        {
                            int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHW_SHD, itemFactory, null, null, shift, itemDate.ToString(), itemDate.ToString());

                            dataChild.Add(availability);
                        }

                        chartTrendModel.name = itemFactory;
                        chartTrendModel.data = dataChild;

                        dataChart.Add(chartTrendModel);
                    }
                    if (itemFactory == "SHD")
                    {
                        // Duyệt qua danh sách từng ngày 
                        foreach (var itemDate in rangerDate)
                        {
                            int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHW_SHD, itemFactory, null, null, shift, itemDate.ToString(), itemDate.ToString());

                            dataChild.Add(availability);
                        }

                        chartTrendModel.name = itemFactory;
                        chartTrendModel.data = dataChild;

                        dataChart.Add(chartTrendModel);
                    }
                    if (itemFactory == "SHB")
                    {
                        // Duyệt qua danh sách từng ngày 
                        foreach (var itemDate in rangerDate)
                        {
                            int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHB, itemFactory, null, null, shift, itemDate.ToString(), itemDate.ToString());

                            dataChild.Add(availability);
                        }

                        chartTrendModel.name = itemFactory;
                        chartTrendModel.data = dataChild;

                        dataChart.Add(chartTrendModel);
                    }
                    if (itemFactory == "SY2")
                    {
                        // Duyệt qua danh sách từng ngày 
                        foreach (var itemDate in rangerDate)
                        {
                            int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSYF, itemFactory, null, null, shift, itemDate.ToString(), itemDate.ToString());

                            dataChild.Add(availability);
                        }

                        chartTrendModel.name = itemFactory;
                        chartTrendModel.data = dataChild;

                        dataChart.Add(chartTrendModel);
                    }
                }
            }
            if (factory != "ALL" && building == "ALL")
            {
                // Nếu factory khác All và building bằng All 
                // SHW , SHD avaibalibity được tính theo từng building 
                // SY2, SHB avaibalibity được tính theo từng machine
                var dataSHW_SHD = await _Cell_OEEService.GetAllCellOEEByDate("",dateStart, dateEnd);
                var dataSHB = await _Cell_OEEService.GetAllCellOEEByDate("SHB",dateStart, dateEnd);
                var dataSYF = await _Cell_OEEService.GetAllCellOEEByDate("SHY",dateStart, dateEnd);

                if (factory != "SHB" && factory != "SY2")
                {
                    var buildings = await _Cell_OEEService.GetListBuildingByFactoryId(factory);
                    foreach (var itemBuilding in buildings)
                    {
                        List<int> dataChild = new List<int>();
                        ChartTrendViewModel chartTrendModel = new ChartTrendViewModel();
                        if (itemBuilding != null)
                        {
                            foreach (var itemDate in rangerDate)
                            {
                                int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHW_SHD, factory, itemBuilding, null, shift, itemDate.ToString(), itemDate.ToString());

                                dataChild.Add(availability);
                            }

                            chartTrendModel.name = itemBuilding + " Building";
                            chartTrendModel.data = dataChild;
                            dataChart.Add(chartTrendModel);
                        }
                    }
                }
                else if (factory == "SHB")
                {
                    var machines = await _Cell_OEEService.GetListMachineByFactoryId(factory);
                    foreach (var item in machines)
                    {
                        List<int> dataChild = new List<int>();
                        ChartTrendViewModel chartTrendModel = new ChartTrendViewModel();
                        if (item != null)
                        {
                            foreach (var itemDate in rangerDate)
                            {
                                int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHB, factory, "SHB", item, shift, itemDate.ToString(), itemDate.ToString());

                                dataChild.Add(availability);
                            }

                            chartTrendModel.name = item;
                            chartTrendModel.data = dataChild;
                            dataChart.Add(chartTrendModel);
                        }
                    }
                }
                else if (factory == "SY2")
                {
                    var machines = await _Cell_OEEService.GetListMachineByFactoryId(factory);
                    foreach (var item in machines)
                    {
                        List<int> dataChild = new List<int>();
                        ChartTrendViewModel chartTrendModel = new ChartTrendViewModel();
                        if (item != null)
                        {
                            foreach (var itemDate in rangerDate)
                            {
                                int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSYF, factory, "SY2", item, shift, itemDate.ToString(), itemDate.ToString());

                                dataChild.Add(availability);
                            }

                            chartTrendModel.name = item;
                            chartTrendModel.data = dataChild;
                            dataChart.Add(chartTrendModel);
                        }
                    }
                }
            }
            if (factory != "ALL" && building != "ALL")
            {
                // Nếu factory và building khác ALL , chỉ tính availability SHD, SHW
                var dataSHW_SHD = await _Cell_OEEService.GetAllCellOEEByDate(factory,dateStart, dateEnd);

                var machines = await _Cell_OEEService.GetListMachineByFactoryId(factory, building);
                foreach (var itemMachine in machines)
                {
                    List<int> dataChild = new List<int>();
                    ChartTrendViewModel chartTrendModel = new ChartTrendViewModel();

                    if (itemMachine != null)
                    {
                        foreach (var itemDate in rangerDate)
                        {
                            int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHW_SHD, factory, building, itemMachine, shift, itemDate.ToString(), itemDate.ToString());
                            dataChild.Add(availability);
                        }

                        chartTrendModel.name = itemMachine;
                        chartTrendModel.data = dataChild;
                        dataChart.Add(chartTrendModel);
                    }
                }
            }
            return new { dataChart, listTime };
        }

        public async Task<object> GetTrendByMonth(string factory, string building, string shift, int numberMonth)
        {
            var weeks = Util.ListWeekOfYear().FindAll(x => x.weekStart.Value.Month == numberMonth);

            List<string> listTime = new List<string>();
            List<ChartTrendViewModel> dataChart = new List<ChartTrendViewModel>();

            string[] listMachine = new string[4] { "SHW", "SHD", "SHB", "SY2" };

            foreach (var week in weeks)
            {
                listTime.Add(week.weekStart.Value.ToString("MM/dd") + " to " + week.weekFinish.Value.ToString("MM/dd") + "\n (Week " + week.weekNum + ")");
            }

            if (factory == "ALL")
            {
                var dataSHW_SHD = await _Cell_OEEService.GetAllCellOEEByMonth("",numberMonth);
                var dataSHB = await _Cell_OEEService.GetAllCellOEEByMonth("SHB",numberMonth);
                var dataSYF = await _Cell_OEEService.GetAllCellOEEByMonth("SHY",numberMonth);

                foreach (var itemMachine in listMachine)
                {
                    List<int> dataChild = new List<int>();
                    ChartTrendViewModel chartTrendModel = new ChartTrendViewModel();

                    foreach (var week in weeks)
                    {
                        var date = week.weekStart.Value.ToString("MM-dd-yyyy");
                        var dateTo = week.weekFinish.Value.ToString("MM-dd-yyyy");
                        if (itemMachine == "SHW")
                        {
                            int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHW_SHD, itemMachine, null, null, shift, date, dateTo);
                            dataChild.Add(availability);
                        }
                        //SHD
                        if (itemMachine == "SHD")
                        {
                            int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHW_SHD, itemMachine, null, null, shift, date, dateTo);
                            dataChild.Add(availability);
                        }

                        //Add SHB
                        if (itemMachine == "SHB")
                        {
                            int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHB, itemMachine, null, null, shift, date, dateTo);
                            dataChild.Add(availability);
                        }
                        //Add SY2
                        if (itemMachine == "SY2")
                        {
                            int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSYF, itemMachine, null, null, shift, date, dateTo);
                            dataChild.Add(availability);
                        }
                    }

                    chartTrendModel.name = itemMachine;
                    chartTrendModel.data = dataChild;

                    dataChart.Add(chartTrendModel);
                }
            }
            if (factory != "ALL" && building == "ALL")
            {
                var dataSHW_SHD = await _Cell_OEEService.GetAllCellOEEByMonth(factory,numberMonth);
                var dataSHB = await _Cell_OEEService.GetAllCellOEEByMonth(factory,numberMonth);
                var dataSYF = await _Cell_OEEService.GetAllCellOEEByMonth(factory,numberMonth);

                if (factory != "SHB" && factory != "SY2")
                {
                    var buildings = await _Cell_OEEService.GetListBuildingByFactoryId(factory);
                    foreach (var itemBuilding in buildings)
                    {
                        List<int> dataChild = new List<int>();
                        ChartTrendViewModel chartTrendModel = new ChartTrendViewModel();
                        if (itemBuilding != null)
                        {
                            foreach (var week in weeks)
                            {
                                var date = week.weekStart.Value.ToString("MM-dd-yyyy");
                                var dateTo = week.weekFinish.Value.ToString("MM-dd-yyyy");
                                int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHW_SHD, factory, itemBuilding, null, shift, date, dateTo);
                                dataChild.Add(availability);
                            }
                            chartTrendModel.name = itemBuilding + " Building";
                            chartTrendModel.data = dataChild;
                            dataChart.Add(chartTrendModel);
                        }
                    }
                }
                else if (factory == "SHB")
                {
                    var machines = await _Cell_OEEService.GetListMachineByFactoryId(factory);
                    foreach (var item in machines)
                    {
                        List<int> dataChild = new List<int>();
                        ChartTrendViewModel chartTrendModel = new ChartTrendViewModel();
                        if (item != null)
                        {
                            foreach (var week in weeks)
                            {
                                var date = week.weekStart.Value.ToString("MM-dd-yyyy");
                                var dateTo = week.weekFinish.Value.ToString("MM-dd-yyyy");
                                int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHB, factory, null, item, shift, date, dateTo);
                                dataChild.Add(availability);
                            }

                            chartTrendModel.name = item;
                            chartTrendModel.data = dataChild;
                            dataChart.Add(chartTrendModel);
                        }
                    }
                }
                else if (factory == "SYF")
                {
                    var machines = await _Cell_OEEService.GetListMachineByFactoryId(factory);
                    foreach (var item in machines)
                    {
                        List<int> dataChild = new List<int>();
                        ChartTrendViewModel chartTrendModel = new ChartTrendViewModel();
                        if (item != null)
                        {
                            foreach (var it in weeks)
                            {
                                var date = it.weekStart.Value.ToString("MM-dd-yyyy");
                                var dateTo = it.weekFinish.Value.ToString("MM-dd-yyyy");
                                int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSYF, factory, null, item, shift, date, dateTo);
                                dataChild.Add(availability);
                            }

                            chartTrendModel.name = item;
                            chartTrendModel.data = dataChild;
                            dataChart.Add(chartTrendModel);
                        }
                    }
                }
            }
            if (factory != "ALL" && building != "ALL")
            {
                var dataSHW_SHD = await _Cell_OEEService.GetAllCellOEEByMonth(factory,numberMonth);

                var machines = await _Cell_OEEService.GetListMachineByFactoryId(factory, building);
                foreach (var item in machines)
                {
                    List<int> dataChild = new List<int>();
                    ChartTrendViewModel chartTrendModel = new ChartTrendViewModel();

                    if (item != null)
                    {
                        foreach (var week in weeks)
                        {
                            var date = week.weekStart.Value.ToString("MM-dd-yyyy");
                            var dateTo = week.weekFinish.Value.ToString("MM-dd-yyyy");
                            int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHW_SHD, factory, building, item, shift, date, dateTo);
                            dataChild.Add(availability);
                        }

                        chartTrendModel.name = item;
                        chartTrendModel.data = dataChild;
                        dataChart.Add(chartTrendModel);
                    }
                }

            }
            return new { dataChart, listTime };
        }

        public async Task<object> GetTrendByYear(string factory, string building, string shift)
        {
            string[] listMachine = new string[4] { "SHW", "SHD", "SHB", "SY2" };
            List<MonthViewModel> listMonth = Util.ListMonth();

            List<string> listTime = new List<string>();
            List<ChartTrendViewModel> dataChart = new List<ChartTrendViewModel>();

            listTime = listMonth.ConvertAll(x => x.NameMonth);

            if (factory == "ALL")
            {
                var dataSHW_SHD = await _Cell_OEEService.GetAllCellOEEByYear();
                var dataSHB = await _Cell_OEEService.GetAllCellOEEByYear();
                var dataSYF = await _Cell_OEEService.GetAllCellOEEByYear();

                foreach (var machine in listMachine)
                {
                    List<int> dataChild = new List<int>();
                    ChartTrendViewModel chartTrendModel = new ChartTrendViewModel();

                    if (machine == "SHW")
                    {
                        foreach (var item in listMonth)
                        {
                            var firstDay = new DateTime(DateTime.Now.Year, item.NumberMonth, 1);
                            var lastDay = firstDay.AddMonths(1).AddDays(-1);

                            int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHW_SHD, machine, null, null, shift, firstDay.ToString(), lastDay.ToString());

                            dataChild.Add(availability);
                        }

                        chartTrendModel.name = machine;
                        chartTrendModel.data = dataChild;

                        dataChart.Add(chartTrendModel);
                    }
                    if (machine == "SHD")
                    {
                        foreach (var item in listMonth)
                        {
                            var firstDay = new DateTime(DateTime.Now.Year, item.NumberMonth, 1);
                            var lastDay = firstDay.AddMonths(1).AddDays(-1);

                            int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHW_SHD, machine, null, null, shift, firstDay.ToString(), lastDay.ToString());

                            dataChild.Add(availability);
                        }


                        chartTrendModel.name = machine;
                        chartTrendModel.data = dataChild;

                        dataChart.Add(chartTrendModel);
                    }
                    if (machine == "SHB")
                    {
                        foreach (var item in listMonth)
                        {
                            var firstDay = new DateTime(DateTime.Now.Year, item.NumberMonth, 1);
                            var lastDay = firstDay.AddMonths(1).AddDays(-1);

                            int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHB, machine, null, null, shift, firstDay.ToString(), lastDay.ToString());

                            dataChild.Add(availability);
                        }


                        chartTrendModel.name = machine;
                        chartTrendModel.data = dataChild;

                        dataChart.Add(chartTrendModel);
                    }
                    if (machine == "SY2")
                    {
                        foreach (var item in listMonth)
                        {
                            var firstDay = new DateTime(DateTime.Now.Year, item.NumberMonth, 1);
                            var lastDay = firstDay.AddMonths(1).AddDays(-1);

                            int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSYF, machine, null, null, shift, firstDay.ToString(), lastDay.ToString());

                            dataChild.Add(availability);
                        }

                        chartTrendModel.name = machine;
                        chartTrendModel.data = dataChild;
                        dataChart.Add(chartTrendModel);
                    }
                }
            }
            if (factory != "ALL" && building == "ALL")
            {
                var dataSHW_SHD = await _Cell_OEEService.GetAllCellOEEByYear();
                var dataSHB = await _Cell_OEEService.GetAllCellOEEByYear();
                var dataSYF = await _Cell_OEEService.GetAllCellOEEByYear();

                if (factory != "SHB" && factory != "SY2")
                {
                    var listBuildings = await _Cell_OEEService.GetListBuildingByFactoryId(factory);
                    foreach (var item in listBuildings)
                    {
                        List<int> dataChild = new List<int>();
                        ChartTrendViewModel chartTrendModel = new ChartTrendViewModel();
                        if (item != null)
                        {
                            foreach (var itemMonth in listMonth)
                            {
                                var firstDay = new DateTime(DateTime.Now.Year, itemMonth.NumberMonth, 1);
                                var lastDay = firstDay.AddMonths(1).AddDays(-1);

                                int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHW_SHD, factory, item, null, shift, firstDay.ToString(), lastDay.ToString());

                                dataChild.Add(availability);
                            }
                            chartTrendModel.name = item + " Building";
                            chartTrendModel.data = dataChild;
                            dataChart.Add(chartTrendModel);
                        }
                    }
                }
                else if (factory == "SHB")
                {
                    var machines = await _Cell_OEEService.GetListMachineByFactoryId(factory);
                    foreach (var item in machines)
                    {
                        List<int> dataChild = new List<int>();
                        ChartTrendViewModel chartTrendModel = new ChartTrendViewModel();
                        if (item != null)
                        {
                            foreach (var itemMonth in listMonth)
                            {
                                var firstDay = new DateTime(DateTime.Now.Year, itemMonth.NumberMonth, 1);
                                var lastDay = firstDay.AddMonths(1).AddDays(-1);

                                int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHB, factory, null, item, shift, firstDay.ToString(), lastDay.ToString());

                                dataChild.Add(availability);
                            }
                            chartTrendModel.name = item;
                            chartTrendModel.data = dataChild;
                            dataChart.Add(chartTrendModel);
                        }
                    }
                }
                else if (factory == "SY2")
                {
                    var machines = await _Cell_OEEService.GetListMachineByFactoryId(factory);
                    foreach (var item in machines)
                    {
                        List<int> dataChild = new List<int>();
                        ChartTrendViewModel chartTrendModel = new ChartTrendViewModel();
                        if (item != null)
                        {
                            foreach (var itemMonth in listMonth)
                            {
                                var firstDay = new DateTime(DateTime.Now.Year, itemMonth.NumberMonth, 1);
                                var lastDay = firstDay.AddMonths(1).AddDays(-1);

                                int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSYF, factory, null, item, shift, firstDay.ToString(), lastDay.ToString());

                                dataChild.Add(availability);
                            }
                            chartTrendModel.name = item;
                            chartTrendModel.data = dataChild;
                            dataChart.Add(chartTrendModel);
                        }
                    }
                }
            }
            if (factory != "ALL" && building != "ALL")
            {
                var dataSHW_SHD = await _Cell_OEEService.GetAllCellOEEByYear();

                var machines = await _Cell_OEEService.GetListMachineByFactoryId(factory, building);
                foreach (var item in machines)
                {
                    List<int> dataChild = new List<int>();
                    ChartTrendViewModel chartTrendModel = new ChartTrendViewModel();

                    if (item != null)
                    {
                        foreach (var itemMonth in listMonth)
                        {
                            var firstDay = new DateTime(DateTime.Now.Year, itemMonth.NumberMonth, 1);
                            var lastDay = firstDay.AddMonths(1).AddDays(-1);

                            int availability = await _Cell_OEEService.GetAvailabilityByRangerDate(dataSHW_SHD, factory, building, item, shift, firstDay.ToString(), lastDay.ToString());

                            dataChild.Add(availability);
                        }

                        chartTrendModel.name = item;
                        chartTrendModel.data = dataChild;
                        dataChart.Add(chartTrendModel);
                    }
                }
            }

            return new { dataChart, listTime };
        }
    }
}