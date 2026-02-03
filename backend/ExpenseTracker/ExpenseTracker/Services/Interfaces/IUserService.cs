using ExpenseTracker.Data.Models;

namespace ExpenseTracker.Services.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User?> CreateUserAsync(User user);
        Task<User?> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(int userId);
    }
}
