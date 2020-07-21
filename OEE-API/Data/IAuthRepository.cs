using System.Threading.Tasks;
using OEE_API.Models;

namespace OEE_API.Data
{
     public interface IAuthRepository
    {
        Task<M_Users> Login(string username, string password);
    }
}