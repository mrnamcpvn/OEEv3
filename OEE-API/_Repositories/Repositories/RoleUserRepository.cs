using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class RoleUserRepository : OEERepository<M_RoleUser>, IRoleUserRepository
    {
        private readonly DataContext _context;
        public RoleUserRepository(DataContext context) : base(context) {
            _context = context;
        }
    }
}