using ExpenseTracker.Data.Models;
using ExpenseTracker.Data.Enums;

namespace ExpenseTracker.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetTransactionByIdAsync(int userId, int transactionId);
        Task<(IEnumerable<Transaction> Items, int TotalCount)> GetAllTransactionsAsync(int userId, int pageNumber, int pageSize);
        Task<Transaction?> CreateTransactionAsync(Transaction transaction);
        Task<Transaction?> UpdateTransactionAsync(int userId, Transaction transaction);
        Task<bool> DeleteTransactionAsync(int userId, int transactionId);
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(int userId, DateTime StartDate, DateTime EndDate);
        Task<decimal> GetTotalAmountByTypeAsync(int userId, TransactionType type, DateTime start, DateTime end);
        Task<Dictionary<Category, decimal>> GetCategorySummaryAsync(int userId, TransactionType type, DateTime start, DateTime end);
        Task<Dictionary<PaymentType, decimal>> GetPaymentTypeSummaryAsync(int userId, DateTime start, DateTime end);
    }
}
