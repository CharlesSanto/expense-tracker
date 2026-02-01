using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Data.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = [];
    }
}
