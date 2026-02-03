using ExpenseTracker.Data.Models;
using ExpenseTracker.Repositories.Interfaces;

namespace ExpenseTracker.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<User?> GetUserByIdAsync(int userId)
        {
            throw new NotImplementedException();
        }
        public Task<User?> GetUserByEmailAsync(string email)
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
