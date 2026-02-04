using ExpenseTracker.DTOs.Auth;
using ExpenseTracker.DTOs.UserDTOs;

namespace ExpenseTracker.Services.Interfaces
{
    public interface IAuthService
    {
        Task<UserResponseDto> LoginAsync(LoginDto loginDto);
    }
}
