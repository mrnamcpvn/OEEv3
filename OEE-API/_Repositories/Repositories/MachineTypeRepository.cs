using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class MachineTypeRepository : OEERepository<M_MachineType>, IMachineTypeRepository
    {
        private readonly DataContext _context;
        public MachineTypeRepository(DataContext context) : base(context) {
            _context = context;
        }
    }
}