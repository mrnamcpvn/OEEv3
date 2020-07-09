using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class RolesRepository : OEERepository<M_Roles>, IRolesRepository
    {
        private readonly DataContext _context;
        public RolesRepository(DataContext context) : base(context) {
            _context = context;
        }
    }
}