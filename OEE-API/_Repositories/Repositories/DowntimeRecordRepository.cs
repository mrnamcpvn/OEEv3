using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class DowntimeRecordRepository : OEERepository<M_DowntimeRecord>, IDowntimeRecordRepository
    {
        private readonly DataContext _context;
        public DowntimeRecordRepository(DataContext context) : base(context) {
            _context = context;
        }
    }
}