using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OEE_API.Application.Interfaces.SHB;
using OEE_API.Data.Interfaces;
using OEE_API.Models.SHB;
using OEE_API.Utilities;
using DbSHW_SHD = OEE_API.Models.SHW_SHD;

namespace OEE_API.Application.Implementation.SHB
{
    public class Cell_OEEService : ICell_OEEService
    {
        private readonly IRepositorySHB<Cell_OEE, int> _repositorySHB;
        private readonly IRepositorySHW_SHD<DbSHW_SHD.ShiftTime, string> _repositoryShiftTime;
        public Cell_OEEService(IRepositorySHB<Cell_OEE, int> repositorySHB, IRepositorySHW_SHD<DbSHW_SHD.ShiftTime, string> repositoryShiftTime)
        {
            _repositorySHB = repositorySHB;
            _repositoryShiftTime = repositoryShiftTime;
        }

        // Lấy ra tất cả Cell_OEE theo ngày
        public async Task<List<Cell_OEE>> GetAllCellOEEByDate(DateTime dateFrom, DateTime dateTo)
        {
            var data = await _repositorySHB.FindAll(x =>
               x.Time.Value.Date >= dateFrom.Date &&
                x.Time.Value.Date <= dateTo.Date.AddDays(1)
            ).ToListAsync();

            return data;
        }

        // Lấy ra tất cả Cell_OEE theo tháng
        public async Task<List<Cell_OEE>> GetAllCellOEEByMonth(int month)
        {
            var data = await _repositorySHB.FindAll(x =>
             x.Time.Value.Month >= month && x.Time.Value.Month <= (month + 1) && x.Time.Value.Year == DateTime.Now.Year
            ).ToListAsync();

            return data;
        }

        // Lấy ra tất cả Cell_OEE theo năm
        public async Task<List<Cell_OEE>> GetAllCellOEEByYear()
        {
            var data = await _repositorySHB.FindAll(x => x.Time.Value.Year == DateTime.Now.Year
            ).ToListAsync();

            return data;
        }

        // Lấy ra tất cả building theo factory
        public async Task<List<string>> GetListBuildingByFactoryId(string factory)
        {
            var data = await _repositorySHB.FindAll(x => x.Factory == factory).GroupBy(x => x.Building).Select(x => x.Key).ToListAsync();

            return data;
        }

        // Lấy ra tất cả machine theo factory và building
        public async Task<List<string>> GetListMachineByFactoryId(string factory, string building = null)
        {
            var data = await _repositorySHB.FindAll(x => x.Factory == factory && (building != null ? x.Building == building : 1 == 1))
                        .GroupBy(x => x.Machine)
                        .OrderBy(g => g.Max(m => m.Machine))
                        .Select(x => x.Key)
                        .ToListAsync();

            return data;
        }

        public async Task<int?> GetAvailability(List<Cell_OEE> data, string factory, string building, string machine, string time, string shift)
        {
            // Lấy ra khoảng thời gian nghỉ theo từng nhà máy 
            var shift1 = await _repositoryShiftTime.FindAll(x => x.factory_id == "SPC" && x.shift_id == "1").FirstOrDefaultAsync();

            var shift2 = await _repositoryShiftTime.FindAll(x => x.factory_id == "SPC" && x.shift_id == "2").FirstOrDefaultAsync();

            DateTime today = Convert.ToDateTime(time);
            DateTime tomorrow = today.AddDays(1);

            Cell_OEE modelShiftDay = null;
            Cell_OEE modelShiftNight = null;

            // Tính theo thời gian làm việc ca ngày
            if (shift1 != null && (shift == "1" || shift == "0"))
            {
                modelShiftDay = data.Where(
                x => x.Factory == factory &&
                    (building != null ? x.Building == building : 1 == 1) &&
                    (machine != null ? x.Machine == machine : 1 == 1) &&
                    x.Time.Value.Date == today.Date &&
                    (x.Time.Value.TimeOfDay >= shift1.start_time && x.Time.Value.TimeOfDay < shift1.end_time)
                ).OrderByDescending(x => x.id).FirstOrDefault();
            }
            //Tính theo thời gian làm việc theo ca tối 
            if (shift2 != null && (shift == "2" || shift == "0"))
            {
                modelShiftNight = data.Where(
                x => x.Factory == factory &&
                    (building != null ? x.Building == building : 1 == 1) &&
                    (machine != null ? x.Machine == machine : 1 == 1) &&
                    ((x.Time.Value.Date == today.Date && x.Time.Value.TimeOfDay >= shift2.start_time)
                    || (x.Time.Value.Date == tomorrow.Date && x.Time.Value.TimeOfDay < shift2.end_time))
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
                // Nếu dữ liệu lấy ra null là ngày đó máy không hoạt động
                return null;
            }
        }

        public async Task<int> GetAvailabilityByRangerDate(List<Cell_OEE> data, string factory, string building, string machine, string shift, string date, string dateTo)
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

                    // Nếu trường hợp tính availability theo factory, building va machine null
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
                    // Trường hợp building khác null, machine null
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
