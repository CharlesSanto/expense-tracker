using ExpenseTracker.Data.Enums;

namespace ExpenseTracker.DTOs.TransactionDtos
{
    public class UpdateTransactionDto
    {
        public string? Description { get; set; }
        public decimal? Amount { get; set; }
        public Category? Category { get; set; }
        public PaymentType? PaymentType { get; set; }
        public DateTime? Date { get; set; }
    }
}
