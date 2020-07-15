using System.Collections.Generic;
using System.Threading.Tasks;
using OEE_API.Dtos;
using OEE_API.ViewModels;

namespace OEE_API._Services.Interfaces
{
    public interface ITrendService
    {
        List<ChartTrendViewModel> DataChartWeek(List<string> names, TrendParamModel param, string nameType, List<M_OEE_Dto> data, List<string> listDate);
        List<ChartTrendViewModel> DataChartMonth(List<string> names, TrendParamModel param, string nameType, List<M_OEE_Dto> data, List<WeekViewModel> weeks);
        Task<object> DataCharTrendWeek (TrendParamModel param);
        Task<object> DataCharTrendMonth (TrendParamModel param);  
    }
}