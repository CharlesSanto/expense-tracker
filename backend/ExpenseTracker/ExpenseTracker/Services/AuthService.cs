using ExpenseTracker.DTOs.Auth;
using ExpenseTracker.DTOs.UserDTOs;
using ExpenseTracker.Repositories;
using ExpenseTracker.Repositories.Interfaces;
using ExpenseTracker.Security;
using ExpenseTracker.Services.Interfaces;

namespace ExpenseTracker.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);

            if (user == null || !PasswordHasher.VerifyPassword(loginDto.Password, user.PasswordHash)) 
                return null;

            return new UserResponseDto(user);
        }
    }
}
