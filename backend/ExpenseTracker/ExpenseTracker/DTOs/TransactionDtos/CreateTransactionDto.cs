using ExpenseTracker.Data.Enums;
using ExpenseTracker.Data.Models;

namespace ExpenseTracker.DTOs.TransactionDtos
{
    public class CreateTransactionDto
    {
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public Category Category { get; set; }
        public TransactionType Type { get; set; }
        public PaymentType PaymentType { get; set; }
        public DateTime Date { get; set; }
    }
}
