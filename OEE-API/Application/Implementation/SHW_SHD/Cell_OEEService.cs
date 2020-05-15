using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OEE_API.Application.Interfaces.SHW_SHD;
using OEE_API.Data.Interfaces;
using OEE_API.Models.SHW_SHD;
using OEE_API.Utilities;

namespace OEE_API.Application.Implementation.SHW_SHD
{
    public class Cell_OEEService : ICell_OEEService
    {
        private readonly IRepositorySHW_SHD<Cell_OEE, int> _repositorySHW_SHD;
        private readonly IRepositorySHW_SHD<ShiftTime, string> _repositoryShiftTime;

        private readonly IRepositorySHW_SHD<OeeReport_test, string> _repositoryReport;

        public Cell_OEEService(
            
            IRepositorySHW_SHD<Cell_OEE, int> repositorySHW_SHD,
            IRepositorySHW_SHD<ShiftTime, string> repositoryShiftTime,
            IRepositorySHW_SHD<OeeReport_test, string> repositoryReport
            )

        {
            _repositorySHW_SHD = repositorySHW_SHD;
            _repositoryShiftTime = repositoryShiftTime;
            _repositoryReport = repositoryReport;
            
        }

        // Lấy ra tất cả Cell_OEE  từ ngày bắt đầu đến ngày kết thúc 
        public async Task<List<OeeReport_test>> GetAllCellOEEByDate(string factory, DateTime dateFrom, DateTime dateTo)
        {
            // var data = await _repositorySHW_SHD.FindAll(x =>
            //    x.Time.Value.Date >= dateFrom.Date &&
            //     x.Time.Value.Date <= dateTo.Date.AddDays(1)
            // ).ToListAsync();

            // return data;
            var test =  _repositoryReport.FindAll(x=> x.Time != null);
            var test1 = test.FirstOrDefault().Shiftdate.Value.Date;
              var data =  _repositoryReport.FindAll(x =>
               x.Shiftdate >= dateFrom &&
                x.Shiftdate <= dateTo.AddDays(1));
            if(factory == "")
            {
             data = data.Where(x=> x.Factory == "SHD" || x.Factory == "SHW");
            }
            else data =  data.Where(x=> x.Factory == factory);
            
            return await data.ToListAsync();
        }
        // public async Task<List<OeeReport_test>> GetAllCellOEEByDate(string factory, DateTime dateFrom, DateTime dateTo)
        // {
        //     var data = await _repositoryReport.FindAll(x =>
        //        x.Time.Value.Date >= dateFrom.Date &&
        //         x.Time.Value.Date <= dateTo.Date.AddDays(1)
        //         && x.Factory == factory
        //     ).ToListAsync();

        //     return data;
        // }
        // Lấy ra tất cả Cell_OEE theo tháng
        public async Task<List<OeeReport_test>> GetAllCellOEEByMonth(string factory,int month)
        {
            if(factory != "")
            {
            var data = await _repositoryReport.FindAll(x => x.Factory == factory &&
             x.Time.Value.Month >= month && x.Time.Value.Month <= (month + 1) && x.Time.Value.Year == DateTime.Now.Year
            ).ToListAsync();
             return data;
            }
            else {
                var data = await _repositoryReport.FindAll(x => 
             x.Time.Value.Month >= month && x.Time.Value.Month <= (month + 1) && x.Time.Value.Year == DateTime.Now.Year
            ).ToListAsync(); 
             return data;
            }
           
        }

        // Lấy ra tất cả Cell_OEE theo năm
        public async Task<List<OeeReport_test>> GetAllCellOEEByYear()
        {
            var data = await _repositoryReport.FindAll(x => x.Time.Value.Year == DateTime.Now.Year
            ).ToListAsync();

            return data;
        }

        // Lấy ra tất cả building theo factory 
        public async Task<List<string>> GetListBuildingByFactoryId(string factory)
        {
            var data = await _repositoryReport.FindAll(x => x.Factory == factory).GroupBy(x => x.Building).Select(x => x.Key).ToListAsync();

            return data;
        }

        // Lấy ra tất cả machine theo factory và building 
        public async Task<List<string>> GetListMachineByFactoryId(string factory, string building = null)
        {
            var data = await _repositoryReport.FindAll(x => x.Factory == factory && (building != null ? x.Building == building : 1 == 1))
                        .GroupBy(x => x.Machine)
                        .OrderBy(g => g.Max(m => m.Machine))
                        .Select(x => x.Key)
                        .ToListAsync();

            return data;
        }

        public async Task<int?> GetAvailability(List<OeeReport_test> data, string factory, string building, string machine, string time, string shift)
        {
            // Lấy ra khoảng thời gian nghỉ theo từng nhà máy 
            var shift1 = await _repositoryShiftTime.FindAll(x => (factory == "SHW" ? x.factory_id == "SHC" : x.factory_id == "CB") && x.building_id == building && x.shift_id == "1").FirstOrDefaultAsync();

            var shift2 = await _repositoryShiftTime.FindAll(x => (factory == "SHW" ? x.factory_id == "SHC" : x.factory_id == "CB") && x.building_id == building && x.shift_id == "2").FirstOrDefaultAsync();

            DateTime today = Convert.ToDateTime(time);
            DateTime tomorrow = today.AddDays(1);

            OeeReport_test modelShiftDay = null;
            OeeReport_test modelShiftNight = null;

            // Tính theo thời gian làm việc ca ngày 
            if (shift1 != null && (shift == "1" || shift == "0"))
            {
                modelShiftDay = data.Where(
                x => x.Factory == factory &&
                    (building != null ? x.Building.Trim() == building.Trim() : 1 == 1) &&
                    (machine != null ? x.Machine == machine : 1 == 1) &&
                    x.Shiftdate == today &&
                   x.Shift_ID == "1"
                ).OrderByDescending(x => x.id).FirstOrDefault();
            }
            //Tính theo thời gian làm việc theo ca tối 
            if (shift2 != null && (shift == "2" || shift == "0"))
            {
                modelShiftNight = data.Where(
                x => x.Factory == factory &&
                    (building != null ? x.Building.Trim() == building.Trim() : 1 == 1) &&
                    (machine != null ? x.Machine == machine : 1 == 1) 
                    // ((x.Time.Value.Date == today.Date && x.Time.Value.TimeOfDay >= shift2.start_time)
                    // || (x.Time.Value.Date == tomorrow.Date && x.Time.Value.TimeOfDay < shift2.end_time))
                    && (x.Shiftdate == today && x.Shift_ID == "2")
                ).OrderByDescending(x => x.id).FirstOrDefault();
            }

            // Nếu thời gian làm việc ca sáng và ca tối có dữ liệu
            if (modelShiftDay != null && modelShiftNight != null)
            {
                double avg = ((double)(modelShiftDay.Availability + modelShiftNight.Availability) / 2);
                double avgCelling = Math.Ceiling(avg);

                return Convert.ToInt32(avgCelling.ToString());

            }
            // Nếu chỉ thời gian làm việc ca sáng có dữ liệu 
            else if (modelShiftDay != null && modelShiftNight == null)
            {
                return Convert.ToInt32(modelShiftDay.Availability.ToString());
            }
            //Nếu thời gian làm việc chỉ ca tối có dữ liệu
            else if (modelShiftDay == null && modelShiftNight != null)
            {
                return Convert.ToInt32(modelShiftNight.Availability.ToString());
            }
            else
            {
                return null;
            }
        }

        // Tính theo cách tính:
        // Chọn ALl: Tính trung bình tât cả Machine trong 1 building, sau đó tính trung bình building trong 1 Factory
        // Chon Factory: Tính trung bình tât cả Machine trong 1 building
        // Chọn Building: Lấy số liệu machine cuối cùng .
        public async Task<int> GetAvailabilityByRangerDate(List<OeeReport_test> data, string factory, string building, string machine, string shift, string date, string dateTo)
        {
            DateTime timeFrom = Convert.ToDateTime(date);
            DateTime timeTo = Convert.ToDateTime(dateTo);

            var listRangeDay = Util.GetRangerDates(timeFrom, timeTo);

            double totalAvailability = 0;

            int lengthAvailability = 0;

            foreach (var itemDate in listRangeDay)
            {
                // tính trung bình availability theo từng ngày 
                if (itemDate.Date <= DateTime.Now.Date)
                {
                    lengthAvailability += 1;

                    double totalAvailabilityBuilding = 0;

                    int lengthAvailabilityBuilding = 0;

                    // Nếu trường hợp tính availability theo factory
                    if (building == null && machine == null)
                    {
                        var buildings = await GetListBuildingByFactoryId(factory);
                        lengthAvailabilityBuilding = buildings.Count;

                        foreach (var itemBuilding in buildings)
                        {
                            //tính trung bình availability của building theo tổng availability của machine 

                            var machines = await GetListMachineByFactoryId(factory, itemBuilding);
                            var lengthMachine = 0;
                            double totalMachine = 0;

                            foreach (var itemMachine in machines)
                            {
                                //tính tổng availability theo machine

                                if (itemMachine != null)
                                {
                                    var availability = await GetAvailability(data, factory, itemBuilding, itemMachine, itemDate.ToString(), shift);
                                    if (availability != null)
                                    {
                                        lengthMachine++;
                                        totalMachine += Convert.ToInt32(availability);
                                    }
                                }
                            }
                            totalAvailabilityBuilding += lengthMachine == 0 ? 0 : (int)Math.Ceiling(totalMachine / lengthMachine);
                        }
                    }
                    else if (building != null && machine == null)
                    {
                        var machines = await GetListMachineByFactoryId(factory, building);

                        foreach (var itemMachine in machines)
                        {
                            var availability = await GetAvailability(data, factory, building, itemMachine, itemDate.ToString(), shift);
                            if (availability != null)
                            {
                                lengthAvailabilityBuilding++;
                                totalAvailabilityBuilding += Convert.ToInt32(availability);
                            }
                        }
                    }
                    else
                    {
                        var availability = await GetAvailability(data, factory, building, machine, itemDate.ToString(), shift);

                        if (availability != null)
                        {
                            lengthAvailabilityBuilding++;
                            totalAvailabilityBuilding += Convert.ToInt32(availability);
                        }
                    }

                    totalAvailability += lengthAvailabilityBuilding == 0 ? 0 : (int)Math.Ceiling(totalAvailabilityBuilding / lengthAvailabilityBuilding);
                }
                else break;
            }

            return lengthAvailability == 0 ? 0 : (int)Math.Ceiling(totalAvailability / lengthAvailability);
        }
    }
}