using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class ActionTimeForOEERepository : OEERepository<M_ActionTimeForOEE>, IActionTimeForOEERepository
    {
        private readonly DataContext _context;
        public ActionTimeForOEERepository(DataContext context) : base(context) {
            _context = context;
        }
        
    }
}