using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class OEE_VNRepository : OEERepository<M_OEE_VN>, IOEE_VNRepository
    {
        private readonly DataContext _context;
        public OEE_VNRepository(DataContext context) : base(context) {
            _context = context;
        }
    }
}