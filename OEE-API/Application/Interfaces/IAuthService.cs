using System.Threading.Tasks;
using OEE_API.Models.SHW_SHD;

namespace OEE_API.Application.Interfaces
{
    public interface IAuthService
    {
        Task<M_Users> Login(string username, string password);
    }
}