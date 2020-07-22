using System.Threading.Tasks;
using OEE_API.Models;

namespace OEE_API._Services.Interfaces
{
    public interface IAuthService
    {
        Task<M_Users> Login(string username, string password); 
    }
}