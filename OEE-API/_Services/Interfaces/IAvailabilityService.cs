using System.Collections.Generic;
using System.Threading.Tasks;

namespace OEE_API._Services.Interfaces
{
    public interface IAvailabilityService
    {
         Task<Dictionary<string, int>> GetListAvailability(string factory,string building,string machine_type,
        string shift,string date,string dateTo);
    }
}