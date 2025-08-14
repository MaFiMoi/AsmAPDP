using Microsoft.EntityFrameworkCore;
using SIMSWebApp.DatabaseContext;
using SIMSWebApp.DatabaseContext.Entities;
using SIMSWebApp.Interfaces;

namespace SIMSWebApp.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SIMSDbContext _context;
        public UserRepository(SIMSDbContext context)
        {
            _context = context;

        }
        public async Task AddSync(User user) => await _context.Users.AddAsync(user);
        

        public async Task<User?> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id).AsTask();
        }

        public async Task<User?> GetUserByUsername(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task SaveChangeAsync() => await _context.SaveChangesAsync();
    }
}
