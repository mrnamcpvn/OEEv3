using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class RowIndexRepository : OEERepository<M_RowIndex>, IRowIndexRepository
    {
        private readonly DataContext _context;
        public RowIndexRepository(DataContext context) : base(context) {
            _context = context;
        }
    }
}