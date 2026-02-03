using ExpenseTracker.Data.Models;

namespace ExpenseTracker.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction?> GetTransactionByIdAsync(int transactionId);
        Task<IEnumerable<Transaction>> GetAllTransactionsAsync();
        Task<Transaction?> CreateTransactionAsync(Transaction transaction);
        Task<Transaction?> UpdateTransactionAsync(Transaction transaction);
        Task<bool> DeleteTransactionAsync(int transactionId);
    }
}
