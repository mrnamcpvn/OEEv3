using System.Collections.Generic;
using System.Threading.Tasks;
using OEE_API.Models.SYF;

namespace OEE_API.Application.Interfaces.SYF
{
    public interface IActionTimeService
    {
        Task<List<string>> GetListMachineActionTime(string factory);
        List<ActionTime> GetDuration(string factory, string machine, string shift, string date);
    }
}