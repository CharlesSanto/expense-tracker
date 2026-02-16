using ExpenseTracker.Data.Models;
using ExpenseTracker.DTOs.Auth;
using ExpenseTracker.DTOs.UserDTOs;

namespace ExpenseTracker.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        string GenerateJwtToken(UserResponseDto user);

    }
}
