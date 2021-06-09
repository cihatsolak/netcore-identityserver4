using IdentityServer.AuthServer.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace IdentityServer.AuthServer.Services.Users
{
    public class CustomUserService : ICustomUserService
    {
        private readonly DbSet<CustomUser> _customUsers;

        public CustomUserService(CustomDbContext context)
        {
            _customUsers = context.CustomUsers;
        }

        public async Task<CustomUser> FindByEmailAsync(string email)
        {
            return await _customUsers.AsNoTracking().FirstOrDefaultAsync(p => string.Equals(email, p.Email));
        }

        public async Task<CustomUser> FindByIdAsync(int id)
        {
            return await _customUsers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> ValidatePasswordAsync(string email, string password)
        {
            return await _customUsers.AsNoTracking().AnyAsync(p => string.Equals(p.Email, email) && string.Equals(p.Password, password));
        }
    }
}
