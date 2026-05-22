using ExpenseTracker.Data.Models;
using ExpenseTracker.DTOs.TransactionDtos;
using ExpenseTracker.DTOs;

namespace ExpenseTracker.Services.Interfaces
{
    public interface ITransactionService
    {
        Task<TransactionResponseDto?> GetTransactionByIdAsync(int userId, int transactionId);
        Task<PagedResponseDto<TransactionResponseDto>> GetAllTransactionsAsync(int userId, int pageNumber, int pageSize);
        Task<TransactionResponseDto?> CreateTransactionAsync(int userId, CreateTransactionDto transaction);
        Task<TransactionResponseDto?> UpdateTransactionAsync(int userId, int transactionId, UpdateTransactionDto transaction);
        Task<bool> DeleteTransactionAsync(int userId, int transactionId);
    }
}
