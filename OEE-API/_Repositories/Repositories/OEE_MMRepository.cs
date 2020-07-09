using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class OEE_MMRepository : OEERepository<M_OEE_MM>, IOEE_MMRepository
    {
        private readonly DataContext _context;
        public OEE_MMRepository(DataContext context) : base(context) {
            _context = context;
        }
    }
}