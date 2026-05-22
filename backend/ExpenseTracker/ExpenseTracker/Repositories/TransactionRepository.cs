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
        public async Task<(IEnumerable<Transaction> Items, int TotalCount)> GetAllTransactionsAsync(int userId, int pageNumber, int pageSize)
        {
            var query = _context.Transactions
                .AsNoTracking()
                .Where(t => t.UserId == userId);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(t => t.Date)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
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

        public async Task<IEnumerable<Transaction>> GetByDateRangeAsync(int userId, DateTime StartDate, DateTime EndDate)
        {
            var transactions = await _context.Transactions
                .AsNoTracking()
                .Where(t => t.UserId == userId && t.Date >= StartDate && t.Date <= EndDate)
                .ToListAsync();

            return transactions;
        }

        public async Task<decimal> GetTotalAmountByTypeAsync(int userId, ExpenseTracker.Data.Enums.TransactionType type, DateTime start, DateTime end)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Where(t => t.UserId == userId && t.Type == type && t.Date >= start && t.Date <= end)
                .SumAsync(t => t.Amount);
        }

        public async Task<Dictionary<ExpenseTracker.Data.Enums.Category, decimal>> GetCategorySummaryAsync(int userId, ExpenseTracker.Data.Enums.TransactionType type, DateTime start, DateTime end)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Where(t => t.UserId == userId && t.Type == type && t.Date >= start && t.Date <= end)
                .GroupBy(t => t.Category)
                .ToDictionaryAsync(g => g.Key, g => g.Sum(t => t.Amount));
        }

        public async Task<Dictionary<ExpenseTracker.Data.Enums.PaymentType, decimal>> GetPaymentTypeSummaryAsync(int userId, DateTime start, DateTime end)
        {
            return await _context.Transactions
                .AsNoTracking()
                .Where(t => t.UserId == userId && t.Type == ExpenseTracker.Data.Enums.TransactionType.Expense && t.Date >= start && t.Date <= end)
                .GroupBy(t => t.PaymentType)
                .ToDictionaryAsync(g => g.Key, g => g.Sum(t => t.Amount));
        }

    }
}
