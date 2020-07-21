using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OEE_API.Models;

namespace OEE_API.Data
{
    public class AuthRepository : IAuthRepository
    {
       private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<M_Users> Login(string username, string password)
        {
            var user = await _context.M_Users.FirstOrDefaultAsync(x => x.account == username);
            if (user == null)
                return null;

            if (user.password != password)
                return null;
            else return user;
        }
    }
}