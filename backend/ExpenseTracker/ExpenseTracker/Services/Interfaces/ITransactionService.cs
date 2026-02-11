using ExpenseTracker.Data.Models;
using ExpenseTracker.DTOs.TransactionDtos;

namespace ExpenseTracker.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionResponseDto?> GetTransactionByIdAsync(int userId, int transactionId);
        Task<IEnumerable<TransactionResponseDto>> GetAllTransactionsAsync(int userId);
        Task<TransactionResponseDto?> CreateTransactionAsync(int userId, CreateTransactionDto transaction);
        Task<TransactionResponseDto?> UpdateTransactionAsync(int userId, int transactionId, UpdateTransactionDto transaction);
        Task<bool> DeleteTransactionAsync(int userId, int transactionId);
    }
}
