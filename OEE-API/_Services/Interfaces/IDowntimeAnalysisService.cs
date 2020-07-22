using System.Threading.Tasks;

namespace OEE_API._Services.Interfaces
{
    public interface IDowntimeAnalysisService
    {
          Task<object> GetDownTimeAnalysis(string factory, string building, int? machine_type, string machine, int? shift, string date, string dateTo);
    }
}