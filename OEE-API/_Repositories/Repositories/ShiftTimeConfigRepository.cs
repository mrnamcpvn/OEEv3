using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class ShiftTimeConfigRepository : OEERepository<M_ShiftTimeConfig> ,IShiftTimeConfigRepository
    {
        private readonly DataContext _context;
        public ShiftTimeConfigRepository(DataContext context) : base(context) {
            _context = context;
        }
    }
}