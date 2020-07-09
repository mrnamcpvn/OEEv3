using OEE_API._Repositories.Interfaces;
using OEE_API.Data;
using OEE_API.Models;

namespace OEE_API._Repositories.Repositories
{
    public class UsersRepository : OEERepository<M_Users>, IUsersRepository
    {
        private readonly DataContext _context;
        public UsersRepository(DataContext context) : base(context) {
            _context = context;
        }
    }
}