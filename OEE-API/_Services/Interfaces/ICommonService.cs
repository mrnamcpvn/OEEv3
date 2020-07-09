using System.Collections.Generic;
using System.Threading.Tasks;
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
    }
}