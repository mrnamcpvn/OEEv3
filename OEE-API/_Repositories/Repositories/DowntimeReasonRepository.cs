using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class DowntimeReasonRepository : OEERepository<M_DowntimeReason>, IDowntimeReasonRepository
    {
        private readonly DataContext _context;
        public DowntimeReasonRepository(DataContext context) : base(context) {
            _context = context;
        }
    }
}