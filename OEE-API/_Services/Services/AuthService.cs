using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OEE_API._Repositories.Interfaces;
using OEE_API._Services.Interfaces;
using OEE_API.Models;

namespace OEE_API._Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsersRepository _repo;
        public AuthService(IUsersRepository repo) {
            _repo = repo;
        }
        public async Task<M_Users> Login(string username, string password)
        {
            // var user = await _repo.FindAll().FirstOrDefaultAsync(x => x.account == username);
            var user = await _repo.GetAll()
                    .Where(x => x.account.Trim() == username.Trim()).FirstOrDefaultAsync();
            if (user == null)
                return null;

            if (user.password != password)
                return null;
            else return user;
        }
    }
}