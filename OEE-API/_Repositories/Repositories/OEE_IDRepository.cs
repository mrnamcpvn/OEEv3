using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class OEE_IDRepository : OEERepository<M_OEE_ID>, IOEE_IDRepository
    {
        private readonly DataContext _context;
        public OEE_IDRepository(DataContext context) : base(context) {
            _context = context;
        }
    }
}