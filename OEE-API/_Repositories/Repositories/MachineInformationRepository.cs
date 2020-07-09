using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class MachineInformationRepository : OEERepository<M_MachineInformation>, IMachineInformationRepository
    {
        private readonly DataContext _context;
        public MachineInformationRepository(DataContext context) : base(context) {
            _context = context;
        }
    }
}