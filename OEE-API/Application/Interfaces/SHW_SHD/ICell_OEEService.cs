using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OEE_API.Models.SHW_SHD;

namespace OEE_API.Application.Interfaces.SHW_SHD
{
    public interface ICell_OEEService
    {
        Task<List<OeeReport_test>> GetAllCellOEEByDate(string factory,DateTime dateFrom, DateTime dateTo);
        Task<List<OeeReport_test>> GetAllCellOEEByMonth(string factory,int month);
        Task<List<OeeReport_test>> GetAllCellOEEByYear();
        Task<List<string>> GetListBuildingByFactoryId(string factory);
        Task<List<string>> GetListMachineByFactoryId(string factory, string building = null, string machine_type = null);
               Task<List<string>> GetListMachineType(string factory, string building, string machine_type );
        Task<int?> GetAvailability(List<OeeReport_test> data, string factory, string building, string machine,  string machine_type, string time, string shift, bool isToday);
        Task<int> GetAvailabilityByRangerDate(List<OeeReport_test> data, string factory, string building, string machine, string machine_type, string shift, string date, string dateTo);
    }
}