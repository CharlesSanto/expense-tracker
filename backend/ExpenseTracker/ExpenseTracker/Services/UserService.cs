using ExpenseTracker.Data.Models;
using ExpenseTracker.Repositories.Interfaces;
using ExpenseTracker.Services.Interfaces;

namespace ExpenseTracker.Services
{
    public class UserService : IUserService
    {   
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<User?> GetUserByEmailAsync(string email)
        {
            throw new NotImplementedException();
        }
        public Task<User?> GetUserByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }
        public Task<User?> CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
        public Task<User?> UpdateUserAsync(User user)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

    }
}
