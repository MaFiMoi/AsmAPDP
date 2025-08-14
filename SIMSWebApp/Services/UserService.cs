using SIMSWebApp.DatabaseContext.Entities;
using SIMSWebApp.Interfaces;

namespace SIMSWebApp.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User> LoginUserAsync(string username, string password)
        {
            var user = await _userRepository.GetUserByUsername(username);
            if (user == null) return null;

            return user.PasswordHash.Equals(password) ? user : null;
        }
    }
}
