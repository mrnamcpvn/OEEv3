using System.Collections.Generic;
using System.Threading.Tasks;
using OEE_API.Models;
using OEE_API.ViewModels;

namespace OEE_API._Services.Interfaces
{
    public interface IAvailabilityService
    {
        Task<List<ChartDashBoardViewModel>> LoadDataChart(DashBoardParamModel param);
    }
}