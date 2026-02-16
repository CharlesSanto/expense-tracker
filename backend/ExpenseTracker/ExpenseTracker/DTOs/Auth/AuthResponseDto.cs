using ExpenseTracker.DTOs.UserDTOs;

namespace ExpenseTracker.DTOs.Auth
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UserResponseDto User { get; set; }
        public AuthResponseDto(string token, UserResponseDto user)
        {
            Token = token;
            User = user;
        }
    }
}
