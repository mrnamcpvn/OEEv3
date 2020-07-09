using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class ShiftRepository : OEERepository<M_Shift> ,IShiftRepository
    {
        private readonly DataContext _context;
        public ShiftRepository(DataContext context) : base(context) {
            _context = context;
        }
    }
}