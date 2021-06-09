using IdentityServer.AuthServer.Models;
using System.Threading.Tasks;

namespace IdentityServer.AuthServer.Repositories.Users
{
    internal interface ICustomUserRepository
    {
        Task<bool> ValidatePasswordAsync(string email, string password);
        Task<CustomUser> FindByIdAsync(int id);
        Task<CustomUser> FindByEmailAsync(string email);
    }
}
