using ExpenseTracker.Data.Models;
using ExpenseTracker.Repositories.Interfaces;

namespace ExpenseTracker.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        public Task<Transaction?> GetTransactionByIdAsync(int transactionId)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            throw new NotImplementedException();
        }
        public Task<Transaction?> CreateTransactionAsync(Transaction transaction)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteTransactionAsync(int transactionId)
        {
            throw new NotImplementedException();
        }
        public Task<Transaction?> UpdateTransactionAsync(Transaction transaction)
        {
            throw new NotImplementedException();
        }
    }
}
