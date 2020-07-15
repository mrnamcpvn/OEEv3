using System.Collections.Generic;
using System.Threading.Tasks;
using OEE_API.ViewModels;

namespace OEE_API._Services.Interfaces
{
    public interface IDashBoardService
    {
        Task<List<ChartDashBoardViewModel>> DataChartDashBoard(DashBoardParamModel param);
    }
}