using ExpenseTracker.Data.Enums;
using System.Diagnostics;

namespace ExpenseTracker.DTOs.TransactionDtos
{
    public class UpdateTransactionDto
    {
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public TransactionType? Type { get; set; }
        public Category? Category { get; set; }
        public PaymentType? PaymentType { get; set; }
        public DateTime? Date { get; set; }
    }
}
