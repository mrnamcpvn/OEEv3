using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OEE_API.Models.SYF;

namespace OEE_API.Application.Interfaces.SYF
{
    public interface ICell_OEEService
    {
        Task<List<OeeReport_test>> GetAllCellOEEByDate(DateTime dateFrom, DateTime dateTo);
        Task<List<OeeReport_test>> GetAllCellOEEByMonth(int month);
        Task<List<OeeReport_test>> GetAllCellOEEByYear();
        Task<List<string>> GetListBuildingByFactoryId(string factory);
        Task<List<string>> GetListMachineByFactoryId(string factory, string building = null);
        Task<int?> GetAvailability(List<OeeReport_test> data, string factory, string building, string machine, string time, string shift);
        Task<int> GetAvailabilityByRangerDate(List<OeeReport_test> data, string factory, string building, string machine, string shift, string date, string dateTo);
    }
}