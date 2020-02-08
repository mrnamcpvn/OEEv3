using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OEE_API.Models.SHW_SHD;

namespace OEE_API.Application.Interfaces.SHW_SHD
{
    public interface ICell_OEEService
    {
        Task<List<Cell_OEE>> GetAllCellOEEByDate(DateTime dateFrom, DateTime dateTo);
        Task<List<Cell_OEE>> GetAllCellOEEByMonth(int month);
        Task<List<Cell_OEE>> GetAllCellOEEByYear();
        Task<List<string>> GetListBuildingByFactoryId(string factory);
        Task<List<string>> GetListMachineByFactoryId(string factory, string building = null);
        Task<int?> GetAvailability(List<Cell_OEE> data, string factory, string building, string machine, string time, string shift);
        Task<int> GetAvailabilityByRangerDate(List<Cell_OEE> data, string factory, string building, string machine, string shift, string date, string dateTo);
    }
}