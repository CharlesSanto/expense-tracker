using ExpenseTracker.Data.Models;

namespace ExpenseTracker.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetTransactionByIdAsync(int userId, int transactionId);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync(int userId, int pageNumber, int pageSize);
        Task<Transaction?> CreateTransactionAsync(Transaction transaction);
        Task<Transaction?> UpdateTransactionAsync(int userId, Transaction transaction);
        Task<bool> DeleteTransactionAsync(int userId, int transactionId);
        Task<IEnumerable<Transaction>> GetByDateRangeAsync(int userId, DateTime StartDate, DateTime EndDate);
    }
}
