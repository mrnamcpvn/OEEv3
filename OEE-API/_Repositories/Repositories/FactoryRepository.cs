using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class FactoryRepository : OEERepository<M_Factory>, IFactoryRepository
    {
        private readonly DataContext _context;
        public FactoryRepository(DataContext context) : base(context) {
            _context = context;
        }
    }
}