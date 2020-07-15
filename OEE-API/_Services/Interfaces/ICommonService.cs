using System.Collections.Generic;
using System.Threading.Tasks;
using OEE_API.Dtos;
using OEE_API.Models;
using OEE_API.ViewModels;

namespace OEE_API._Services.Interfaces
{
    public interface ICommonService
    {
        Task<List<M_Factory>> GetListFactory();
        Task<List<M_Shift>> GetListShift();
        Task<List<string>> GetListBuilding(string factory);
        Task<List<MachineViewModel>> GetListMachineType(string factory, string building);
        Task<List<string>> ListMachineID(string factory, string building);
        Task<List<string>> ListMachineID(string factory, string building, string machine_type);
        Task<List<M_OEE_Dto>> GetListOEE(string factory);
    }
}