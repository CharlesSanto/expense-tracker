using ExpenseTracker.DTOs.Auth;
using ExpenseTracker.DTOs.UserDTOs;
using ExpenseTracker.Repositories.Interfaces;
using ExpenseTracker.Security;
using ExpenseTracker.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ExpenseTracker.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private static readonly JwtSecurityTokenHandler _tokenHandler = new();

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);

            if (user == null || !PasswordHasher.VerifyPassword(loginDto.Password, user.PasswordHash)) 
                return null;

            var userDto = new UserResponseDto(user);

            var token = GenerateJwtToken(userDto);

            return new AuthResponseDto(token, userDto);
        }

        public string GenerateJwtToken(UserResponseDto user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return _tokenHandler.WriteToken(token);
        }
    }
}
