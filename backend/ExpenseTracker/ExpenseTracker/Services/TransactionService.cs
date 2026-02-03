using ExpenseTracker.Data.Models;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Repositories.Interfaces;

namespace ExpenseTracker.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<Transaction?> GetTransactionByIdAsync(int transactionId)
        {
            return await _transactionRepository.GetTransactionByIdAsync(transactionId);
        }
        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync()
        {
            return await _transactionRepository.GetAllTransactionsAsync();
        }
        public async Task<Transaction?> CreateTransactionAsync(Transaction transaction)
        {
            return await _transactionRepository.CreateTransactionAsync(transaction);
        }
        public async Task<Transaction?> UpdateTransactionAsync(Transaction transaction)
        {
            return await _transactionRepository.UpdateTransactionAsync(transaction);
        }
        public async Task<bool> DeleteTransactionAsync(int transactionId)
        {
            return await _transactionRepository.DeleteTransactionAsync(transactionId);
        }
    }
}
