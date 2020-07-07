using System.Threading.Tasks;

namespace OEE_API.Application.Interfaces
{
    public interface ITrendService
    {
      //   Task<object> GetTrendByDate(string factory, string building, string machine_type, string shift, string numberDate);

        Task<object> GetTrendByWeek(string factory, string building, string machine_type, string shift, int numberWeek);
        Task<object> GetTrendByMonth(string factory, string building, string machine_type,string shift, int numberMonth);
        Task<object> GetTrendByYear(string factory, string building,string machine_type, string shift);
    }
}