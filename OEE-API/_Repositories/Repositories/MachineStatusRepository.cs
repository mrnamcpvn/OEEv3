using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class MachineStatusRepository : OEERepository<M_MachineStatus>, IMachineStatusRepository
    {
        private readonly DataContext _context;
        public MachineStatusRepository(DataContext context) : base(context) {
            _context = context;
        }
    }
}