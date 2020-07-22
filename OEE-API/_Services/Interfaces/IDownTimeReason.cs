using System.Collections.Generic;
using System.Threading.Tasks;
using OEE_API.Helpers;
using OEE_API.Models;
using OEE_API.ViewModels;

namespace OEE_API._Services.Interfaces
{
    public interface IDownTimeReasonService
    {
        Task<PageListUtility<ChartReason>> GetDataChart(string factory, string building, string machine, string machine_type, string shift, string date, int page = 1);
        Task<object> GetReason(int Id);
        Task<List<string>> GetDowntimeReasonDetail(string reason_1);
         Task<List<string>> GetDowntimeReasonType();
         Task<bool> AddDowntimeReasonType(ChartReason chartReason);
    }
}