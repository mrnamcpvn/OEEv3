using System.Threading.Tasks;
using OEE_API.ViewModels;

namespace OEE_API._Services.Interfaces
{
    public interface ITrendService
    {
        Task<object> DataCharTrend (TrendParamModel param);  
    }
}