using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceSHW_SHD = OEE_API.Application.Interfaces.SHW_SHD;
using ServiceSHB = OEE_API.Application.Interfaces.SHB;
using ServiceSYF = OEE_API.Application.Interfaces.SYF;
using OEE_API.Application.Interfaces;
using System;
using Microsoft.Data.SqlClient;
using System.Data;
using OEE_API.Models.SHW_SHD;
using Microsoft.EntityFrameworkCore;

namespace OEE_API.Application.Implementation
{
    public class AvailabilityService : IAvailabilityService
    {
        ServiceSHW_SHD.ICell_OEEService _Cell_OEEServiceSHW_SHD;
        ServiceSHB.ICell_OEEService _Cell_OEEServiceSHB;
        ServiceSYF.ICell_OEEService _Cell_OEEServiceSYF;

        ServiceSHW_SHD.ICell_OEEService _Cell_OEEService;
        public readonly DBContextSHW_SHD _context;

        public AvailabilityService(
            ServiceSHW_SHD.ICell_OEEService Cell_OEEServiceSHW_SHD,
            ServiceSHB.ICell_OEEService Cell_OEEServiceSHB,
            ServiceSYF.ICell_OEEService Cell_OEEServiceSYF,
            ServiceSHW_SHD.ICell_OEEService Cell_OEEServices
            ,DBContextSHW_SHD context)


        {
            _Cell_OEEServiceSHW_SHD = Cell_OEEServiceSHW_SHD;
            _Cell_OEEServiceSHB = Cell_OEEServiceSHB;
            _Cell_OEEServiceSYF = Cell_OEEServiceSYF;
            _Cell_OEEService = Cell_OEEServices;
            _context = context;
        }

        public async Task<Dictionary<string, int>> GetListAvailabilityAsync(string factory, string building, string machine_type,string shift, string date, string dateTo)
        {
            DateTime now = DateTime.Now;
 
            // EXEC Store Procedure
            if(now.Date.CompareTo(Convert.ToDateTime(date).Date) >= 0 && now.Date.CompareTo(Convert.ToDateTime(dateTo).Date) >= 0)
            {
                // Call STORE PROCEDURE -- GET Today_Data
                var return_value =   _context.Row.FromSqlRaw("EXEC [dbo].[SP_get_Today_RealTime_OEE_data]").ToString();

            }

            Dictionary<string, int> model = new Dictionary<string, int>();
            // Service lớn gọi tới service con 
            if (factory == "ALL")
            {
                // var data = await _Cell_OEEService.GetAllCellOEEByDate(Convert.ToDateTime(date), Convert.ToDateTime(dateTo));
                var dataSHW_SHD = await _Cell_OEEService.GetAllCellOEEByDate("", Convert.ToDateTime(date), Convert.ToDateTime(dateTo));
                var dataSHB = await _Cell_OEEService.GetAllCellOEEByDate("SHB", Convert.ToDateTime(date), Convert.ToDateTime(dateTo));
                var dataSYF = await _Cell_OEEService.GetAllCellOEEByDate("SHY", Convert.ToDateTime(date), Convert.ToDateTime(dateTo));

                // Add Availability SHW 
                var availabilitySHW = await _Cell_OEEServiceSHW_SHD.GetAvailabilityByRangerDate(dataSHW_SHD, "SHW", null, null, machine_type, shift, date, dateTo);
                model.Add("SHW", availabilitySHW);
                //Add Availability SHD
                var availabilitySHD = await _Cell_OEEServiceSHW_SHD.GetAvailabilityByRangerDate(dataSHW_SHD, "SHD", null, null, machine_type, shift, date, dateTo);
                model.Add("SHD", availabilitySHD);
                //Add Availability SHB
                var availabilitySHB = await _Cell_OEEServiceSHW_SHD.GetAvailabilityByRangerDate(dataSHB, "SHB", null, null, machine_type, shift, date, dateTo);
                model.Add("SHB", availabilitySHB);
                //Add Availability SY2
                var availabilitySY2 = await _Cell_OEEServiceSHW_SHD.GetAvailabilityByRangerDate(dataSYF, "SY2", null, null,machine_type, shift, date, dateTo);
                model.Add("SY2", availabilitySY2);
            }
            else if (factory != "ALL" && building == "ALL")
            {
                var dataSHW_SHD = await _Cell_OEEServiceSHW_SHD.GetAllCellOEEByDate("", Convert.ToDateTime(date), Convert.ToDateTime(dateTo));
            
                var dataSHB = await _Cell_OEEServiceSHW_SHD.GetAllCellOEEByDate("SHB", Convert.ToDateTime(date), Convert.ToDateTime(dateTo));
                var dataSYF = await _Cell_OEEServiceSHW_SHD.GetAllCellOEEByDate("SYF", Convert.ToDateTime(date), Convert.ToDateTime(dateTo));

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
                            int totalAvaibility = await _Cell_OEEServiceSHW_SHD.GetAvailabilityByRangerDate(dataSHW_SHD, factory, itemBuilding, null, machine_type, shift, date, dateTo);
                            model.Add(itemBuilding, totalAvaibility);
                        }
                    }
                }
                else if (factory == "SHB")
                {
                    var machines = await _Cell_OEEServiceSHW_SHD.GetListMachineByFactoryId(factory);
                    foreach (var item in machines)
                    {
                        if (item != null)
                        {
                            var availability = await _Cell_OEEServiceSHW_SHD.GetAvailabilityByRangerDate(dataSHB, factory, "SHB", item, machine_type, shift, date, dateTo);
                            model.Add(item, availability);
                        }
                    }
                }
                else if (factory == "SY2")
                {
                    var machines = await _Cell_OEEServiceSHW_SHD.GetListMachineByFactoryId(factory);
                    foreach (var item in machines)
                    {
                        if (item != null)
                        {
                            var availability = await _Cell_OEEServiceSHW_SHD.GetAvailabilityByRangerDate(dataSYF, factory, "SY2", item, machine_type, shift, date, dateTo);
                            model.Add(item, availability);
                        }
                    }
                }
            }
            else if (factory != "ALL" && building != "ALL")
            {
                var dataSHW_SHD = await _Cell_OEEServiceSHW_SHD.GetAllCellOEEByDate(factory, Convert.ToDateTime(date), Convert.ToDateTime(dateTo));
                var machines = new List<string>();
                //Find Machine Type if existed
                if(machine_type != "ALL")
                {
                     machines = await _Cell_OEEServiceSHW_SHD.GetListMachineType(factory,building, machine_type);
                }
                else {
                      machines = await _Cell_OEEServiceSHW_SHD.GetListMachineByFactoryId(factory, building);
                }
                foreach (var item in machines)
                {
                    if (item != null)
                    {
                        var availability = await _Cell_OEEServiceSHW_SHD.GetAvailabilityByRangerDate(dataSHW_SHD, factory, building, item, machine_type, shift, date, dateTo);
                        model.Add(item, availability);
                    }
                }
            }

            return model;
        }
    }
}