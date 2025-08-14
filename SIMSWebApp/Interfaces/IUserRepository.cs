using SIMSWebApp.DatabaseContext.Entities;

namespace SIMSWebApp.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByUsername(string username);
        Task<User?> GetUserById(int id);
        Task AddSync(User user);
        Task SaveChangeAsync();
    }
}
