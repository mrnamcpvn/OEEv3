using System.Threading.Tasks;

namespace OEE_API.Application.Interfaces
{
    public interface ITrendService
    {
        Task<object> GetTrendByWeek(string factory, string building, string shift, int numberWeek);
        Task<object> GetTrendByMonth(string factory, string building, string shift, int numberMonth);
        Task<object> GetTrendByYear(string factory, string building, string shift);
    }
}