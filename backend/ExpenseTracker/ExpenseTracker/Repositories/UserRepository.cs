using ExpenseTracker.Data;
using ExpenseTracker.Data.Models;
using ExpenseTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .FindAsync(userId);
        }
        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User?> CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public async Task UpdateUserAsync(User user)
        {
             _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteUserAsync(int userId)
        {
            var affectedRows = await _context.Users
                .Where(u => u.Id == userId)
                .ExecuteDeleteAsync();

            return affectedRows > 0;
        }
    }
}
