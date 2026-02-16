using ExpenseTracker.Data.Enums;
using ExpenseTracker.Data.Models;

namespace ExpenseTracker.DTOs.TransactionDtos
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public Category Category { get; set; }
        public TransactionType Type { get; set; }
        public PaymentType PaymentType { get; set; }
        public DateTime Date { get; set; }

        public TransactionResponseDto(Transaction transaction)
        {
            Id = transaction.Id;
            Description = transaction.Description;
            Amount = transaction.Amount;
            Category = transaction.Category;
            Type = transaction.Type;
            PaymentType = transaction.PaymentType;
            Date = transaction.Date;
        }
    }
}
