using System.Threading.Tasks;
using OEE_API.Helpers;
using OEE_API.Models;
using OEE_API.ViewModels;

namespace OEE_API._Services.Interfaces
{
    public interface IDownTimeReasonService
    {
        Task<PageListUtility<ChartReason>> GetDataChart(DownTimeReasonParamModel param);
    }
}