using System.Threading.Tasks;
using OEE_API.Application.Interfaces;
using OEE_API.Data.Interfaces;
using OEE_API.Models.SHW_SHD;

namespace OEE_API.Application.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly IRepositorySHW_SHD<M_Users, string> _repositoryUser;

        public AuthService(IRepositorySHW_SHD<M_Users, string> repositoryUser)
        {
            _repositoryUser = repositoryUser;
        }

        public Task<M_Users> Login(string username, string password)
        {
            var data = _repositoryUser.FindSingle(x=> x.account == username && x.password == password);
            if(data != null)
            {
                return data;
            }
            else return null;
        }
    }
}