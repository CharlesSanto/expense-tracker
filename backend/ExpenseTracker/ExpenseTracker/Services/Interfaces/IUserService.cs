using ExpenseTracker.Data.Models;
using ExpenseTracker.DTOs.Auth;
using ExpenseTracker.DTOs.UserDTOs;

namespace ExpenseTracker.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> GetUserByIdAsync(int userId);
        Task<UserResponseDto> GetUserByEmailAsync(string email);
        Task<UserResponseDto> CreateUserAsync(RegisterDto user);
        Task<UserResponseDto> UpdateUserAsync(int userId, UpdateUserDto updateUser);
        Task<bool> DeleteUserAsync(int userId);
    }
}
