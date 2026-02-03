    using ExpenseTracker.Data.Models;
using ExpenseTracker.Repositories.Interfaces;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.DTOs.Auth;
using ExpenseTracker.DTOs.UserDTOs;
using ExpenseTracker.Security;
namespace ExpenseTracker.Services
{
    public class UserService : IUserService
    {   
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserResponseDto?> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            return user == null ? null : new UserResponseDto(user);
        }

        public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email.ToLower());

            return user == null ? null : new UserResponseDto(user);
        }

        public async Task<UserResponseDto?> CreateUserAsync(RegisterDto registerDto)
        {
            var emailExists = await _userRepository.GetUserByEmailAsync(registerDto.Email.ToLower());

            if (emailExists != null)
            {
                throw new InvalidOperationException("Este e-mail já está cadastrado.");
            }

            User newUser = new User {
                Name = registerDto.Name.Trim(),
                Email = registerDto.Email.ToLower(),    
                PasswordHash = PasswordHasher.HashPassword(registerDto.Password),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var user = await _userRepository.CreateUserAsync(newUser);

            return user == null ? null : new UserResponseDto(user);
        }

        public async Task<UserResponseDto?> UpdateUserAsync(int userId, UpdateUserDto updateUser)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) return null;

            if (!string.IsNullOrWhiteSpace(updateUser.Name))
                user.Name = updateUser.Name.Trim();

            if (!(string.IsNullOrWhiteSpace(updateUser.Email)) && !string.Equals(updateUser.Email, user.Email, StringComparison.OrdinalIgnoreCase))
            {
                var emailExists = await _userRepository.GetUserByEmailAsync(updateUser.Email);

                if (emailExists != null && emailExists.Id != user.Id)
                {
                    throw new InvalidOperationException("Este e-mail já está cadastrado.");
                }

                user.Email = updateUser.Email.ToLower();
            }
                

            if (!string.IsNullOrWhiteSpace(updateUser.Password))
                user.PasswordHash = PasswordHasher.HashPassword(updateUser.Password);

            user.UpdatedAt = DateTime.UtcNow;

            var updatedUser = await _userRepository.UpdateUserAsync(user);

            return updatedUser == null ? null : new UserResponseDto(updatedUser);
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            return await _userRepository.DeleteUserAsync(userId);
        }

    }
}
