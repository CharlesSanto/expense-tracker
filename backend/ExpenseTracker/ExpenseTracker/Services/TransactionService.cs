using ExpenseTracker.Data.Models;
using ExpenseTracker.Services.Interfaces;
using ExpenseTracker.Repositories.Interfaces;
using ExpenseTracker.DTOs.TransactionDtos;

namespace ExpenseTracker.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }
        public async Task<TransactionResponseDto?> GetTransactionByIdAsync(int userId, int transactionId)
        {
            var transaction = await _transactionRepository.GetTransactionByIdAsync(userId, transactionId);

            return transaction == null ? null : new TransactionResponseDto(transaction);
        }
        public async Task<IEnumerable<TransactionResponseDto>> GetAllTransactionsAsync(int userId)
        {
            var transactions = await _transactionRepository.GetAllTransactionsAsync(userId);

            return transactions.Select(t => new TransactionResponseDto(t)).ToList();    
        }
        public async Task<TransactionResponseDto?> CreateTransactionAsync(int userId, CreateTransactionDto transaction)
        { 
            if (transaction.Amount <= 0) return null;

            var newTransaction = new Transaction
            {
                UserId = userId,
                Description = transaction.Description.Trim(),
                Amount = transaction.Amount,
                Category = transaction.Category,
                Type = transaction.Type,
                PaymentType = transaction.PaymentType,
                Date = transaction.Date,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var transactionResponse = await _transactionRepository.CreateTransactionAsync(newTransaction);

            return transactionResponse == null ? null : new TransactionResponseDto(transactionResponse);
        }
        public async Task<TransactionResponseDto?> UpdateTransactionAsync(int userId, int transactionId, UpdateTransactionDto transaction)
        {
            var existingTransaction = await _transactionRepository.GetTransactionByIdAsync(userId, transactionId);

            if (existingTransaction == null) return null;

            if (transaction.Amount.HasValue)
            {
                if (transaction.Amount <= 0)
                    return null;
                existingTransaction.Amount = transaction.Amount.Value;
            }

            if (!string.IsNullOrWhiteSpace(transaction.Description))
                existingTransaction.Description = transaction.Description.Trim();

            if (transaction.Category.HasValue)
                existingTransaction.Category = transaction.Category.Value;

            if (transaction.PaymentType.HasValue)
                existingTransaction.PaymentType = transaction.PaymentType.Value;

            if (transaction.Date.HasValue)
                existingTransaction.Date = transaction.Date.Value;

            existingTransaction.UpdatedAt = DateTime.UtcNow;

            var updatedTransaction = await _transactionRepository.UpdateTransactionAsync(userId, existingTransaction);

            return updatedTransaction == null ? null :new TransactionResponseDto(updatedTransaction);

        }
        public async Task<bool> DeleteTransactionAsync(int userId, int transactionId)
        {
            return await _transactionRepository.DeleteTransactionAsync(userId, transactionId);
        }
    }
}
