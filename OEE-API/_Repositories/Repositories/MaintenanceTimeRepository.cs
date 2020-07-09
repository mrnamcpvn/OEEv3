using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class MaintenanceTimeRepository : OEERepository<M_MaintenanceTime>, IMaintenanceTimeRepository
    {
        private readonly DataContext _context;
        public MaintenanceTimeRepository(DataContext context) : base(context) {
            _context = context;
        }
    }
}