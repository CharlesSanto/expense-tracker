using ExpenseTracker.Data;
using ExpenseTracker.Data.Models;
using ExpenseTracker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {

        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int userId, int transactionId)
        {
            return await _context.Transactions
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.UserId == userId && t.Id == transactionId);
        }
        public async Task<IEnumerable<Transaction>> GetAllTransactionsAsync(int userId)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }
        public async Task<Transaction?> CreateTransactionAsync(Transaction transaction)
        {
            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
        public async Task<Transaction?> UpdateTransactionAsync(int userId, Transaction transaction)
        {
            if (transaction.UserId != userId) return null;

            _context.Transactions.Update(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
        public async Task<bool> DeleteTransactionAsync(int userId, int transactionId)
        {
            var rows = await _context.Transactions
                .Where(t => t.Id == transactionId && t.UserId == userId)
                .ExecuteDeleteAsync();
            return rows > 0;
        }
        
    }
}
