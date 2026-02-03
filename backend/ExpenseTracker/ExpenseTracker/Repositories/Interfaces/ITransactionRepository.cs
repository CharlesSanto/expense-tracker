using ExpenseTracker.Data.Models;

namespace ExpenseTracker.Repositories.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction?> GetTransactionByIdAsync(int transactionId);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<Transaction?> CreateTransactionAsync(Transaction transaction);
        Task<Transaction?> UpdateTransactionAsync(Transaction transaction);
        Task<bool> DeleteTransactionAsync(int transactionId);
    }
}
