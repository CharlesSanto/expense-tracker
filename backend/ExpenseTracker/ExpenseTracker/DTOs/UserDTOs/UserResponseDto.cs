using ExpenseTracker.Data.Models;

namespace ExpenseTracker.DTOs.UserDTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public UserResponseDto(User user)
        {
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
        }

    }
}
