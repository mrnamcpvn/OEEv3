using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceSHW_SHD = OEE_API.Application.Interfaces.SHW_SHD;
using ServiceSHB = OEE_API.Application.Interfaces.SHB;
using ServiceSYF = OEE_API.Application.Interfaces.SYF;
using OEE_API.Application.Interfaces;
using System;

namespace OEE_API.Application.Implementation
{
    public class AvailabilityService : IAvailabilityService
    {
        ServiceSHW_SHD.ICell_OEEService _Cell_OEEServiceSHW_SHD;
        ServiceSHB.ICell_OEEService _Cell_OEEServiceSHB;
        ServiceSYF.ICell_OEEService _Cell_OEEServiceSYF;

        public AvailabilityService(
            ServiceSHW_SHD.ICell_OEEService Cell_OEEServiceSHW_SHD,
            ServiceSHB.ICell_OEEService Cell_OEEServiceSHB,
            ServiceSYF.ICell_OEEService Cell_OEEServiceSYF)
        {
            _Cell_OEEServiceSHW_SHD = Cell_OEEServiceSHW_SHD;
            _Cell_OEEServiceSHB = Cell_OEEServiceSHB;
            _Cell_OEEServiceSYF = Cell_OEEServiceSYF;
        }

        public async Task<Dictionary<string, int>> GetListAvailabilityAsync(string factory, string building, string shift, string date, string dateTo)
        {
            Dictionary<string, int> model = new Dictionary<string, int>();

            if (factory == "ALL")
            {
                var dataSHW_SHD = await _Cell_OEEServiceSHW_SHD.GetAllCellOEEByDate(Convert.ToDateTime(date), Convert.ToDateTime(dateTo));
                var dataSHB = await _Cell_OEEServiceSHB.GetAllCellOEEByDate(Convert.ToDateTime(date), Convert.ToDateTime(dateTo));
                var dataSYF = await _Cell_OEEServiceSYF.GetAllCellOEEByDate(Convert.ToDateTime(date), Convert.ToDateTime(dateTo));

                //Add Availability SHW
                var availabilitySHW = await _Cell_OEEServiceSHW_SHD.GetAvailabilityByRangerDate(dataSHW_SHD, "SHW", null, null, shift, date, dateTo);
                model.Add("SHW", availabilitySHW);
                //Add Availability SHD
                var availabilitySHD = await _Cell_OEEServiceSHW_SHD.GetAvailabilityByRangerDate(dataSHW_SHD, "SHD", null, null, shift, date, dateTo);
                model.Add("SHD", availabilitySHD);
                //Add Availability SHB
                var availabilitySHB = await _Cell_OEEServiceSHB.GetAvailabilityByRangerDate(dataSHB, "SHB", null, null, shift, date, dateTo);
                model.Add("SHB", availabilitySHB);
                //Add Availability SY2
                var availabilitySY2 = await _Cell_OEEServiceSYF.GetAvailabilityByRangerDate(dataSYF, "SY2", null, null, shift, date, dateTo);
                model.Add("SY2", availabilitySY2);
            }
            else if (factory != "ALL" && building == "ALL")
            {
                var dataSHW_SHD = await _Cell_OEEServiceSHW_SHD.GetAllCellOEEByDate(Convert.ToDateTime(date), Convert.ToDateTime(dateTo));
                var dataSHB = await _Cell_OEEServiceSHB.GetAllCellOEEByDate(Convert.ToDateTime(date), Convert.ToDateTime(dateTo));
                var dataSYF = await _Cell_OEEServiceSYF.GetAllCellOEEByDate(Convert.ToDateTime(date), Convert.ToDateTime(dateTo));

                if (factory != "SHB" && factory != "SY2")
                {
                    var buildings = await _Cell_OEEServiceSHW_SHD.GetListBuildingByFactoryId(factory);
                    foreach (var itemBuilding in buildings)
                    {
                        // var machines = await _Cell_OEEServiceSHW_SHD.GetListMachineByFactoryId(factory, itemBuilding);

                        // double totalAvaibility = 0;

                        // foreach (var itemMachine in machines)
                        // {
                        //     if (itemMachine != null)
                        //     {
                        //         totalAvaibility += await _Cell_OEEServiceSHW_SHD.GetAvailabilityByRangerDate(dataSHW_SHD, factory, itemBuilding, itemMachine, shift, date, dateTo);
                        //     }
                        // }

                        // model.Add(itemBuilding, machines.Count == 0 ? 0 : (int)Math.Ceiling(totalAvaibility / machines.Count));

                        if (itemBuilding != null)
                        {
                            int totalAvaibility = await _Cell_OEEServiceSHW_SHD.GetAvailabilityByRangerDate(dataSHW_SHD, factory, itemBuilding, null, shift, date, dateTo);
                            model.Add(itemBuilding, totalAvaibility);
                        }
                    }
                }
                else if (factory == "SHB")
                {
                    var machines = await _Cell_OEEServiceSHB.GetListMachineByFactoryId(factory);
                    foreach (var item in machines)
                    {
                        if (item != null)
                        {
                            var availability = await _Cell_OEEServiceSHB.GetAvailabilityByRangerDate(dataSHB, factory, "SHB", item, shift, date, dateTo);
                            model.Add(item, availability);
                        }
                    }
                }
                else if (factory == "SY2")
                {
                    var machines = await _Cell_OEEServiceSYF.GetListMachineByFactoryId(factory);
                    foreach (var item in machines)
                    {
                        if (item != null)
                        {
                            var availability = await _Cell_OEEServiceSYF.GetAvailabilityByRangerDate(dataSYF, factory, "SY2", item, shift, date, dateTo);
                            model.Add(item, availability);
                        }
                    }
                }
            }
            else if (factory != "ALL" && building != "ALL")
            {
                var dataSHW_SHD = await _Cell_OEEServiceSHW_SHD.GetAllCellOEEByDate(Convert.ToDateTime(date), Convert.ToDateTime(dateTo));

                var machines = await _Cell_OEEServiceSHW_SHD.GetListMachineByFactoryId(factory, building);
                foreach (var item in machines)
                {
                    if (item != null)
                    {
                        var availability = await _Cell_OEEServiceSHW_SHD.GetAvailabilityByRangerDate(dataSHW_SHD, factory, building, item, shift, date, dateTo);
                        model.Add(item, availability);
                    }
                }
            }

            return model;
        }
    }
}