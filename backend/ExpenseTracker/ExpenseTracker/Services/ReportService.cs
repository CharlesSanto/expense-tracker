using ExpenseTracker.DTOs.ReportDtos;
using ExpenseTracker.Repositories.Interfaces;
using ExpenseTracker.Data.Enums;
using ExpenseTracker.Services.Interfaces;

namespace ExpenseTracker.Services
{
    public class ReportService : IReportService
    {
        private readonly ITransactionRepository _transactionRepository;

        public ReportService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<FinancialReportDto> GenerateFinancialReport(int userId, string range, DateTime? StartDate, DateTime? EndDate)
        {
            var (finalStart, finalEnd) = CalculateDates(range, StartDate, EndDate);

            var transactions = await _transactionRepository.GetByDateRangeAsync(userId, finalStart, finalEnd);

            var totalIncome = transactions
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Amount);

            var totalExpense = transactions
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Amount);

            var report = new FinancialReportDto
            {
                StartDate = finalStart,
                EndDate = finalEnd,

                TotalIncome = totalIncome,
                TotalExpense = totalExpense,

                IncomeCategories = transactions
                    .Where(t => t.Type == TransactionType.Income)
                    .GroupBy(t => t.Category)
                    .Select(g => new CategorySummaryDto
                    {
                        CategoryName = g.Key.ToString(),
                        Amount = g.Sum(t => t.Amount),
                        Percentage = totalIncome > 0
                        ? (double)Math.Round((g.Sum(t => t.Amount) / totalIncome) * 100, 2)
                        : 0
                    })
                    .OrderByDescending(c => c.Amount)
                    .ToList(),

                ExpenseCategories = transactions
                    .Where(t => t.Type == TransactionType.Expense)
                    .GroupBy(t => t.Category)
                    .Select(g => new CategorySummaryDto
                    {
                        CategoryName = g.Key.ToString(),
                        Amount = g.Sum(t => t.Amount),
                        Percentage = totalExpense > 0
                        ? (double)Math.Round((g.Sum(t => t.Amount) / totalExpense) * 100, 2)
                        : 0
                    })
                    .OrderByDescending(c => c.Amount)
                    .ToList(),

                PaymentMethods = transactions
                    .Where(t => t.Type == TransactionType.Expense)
                    .GroupBy(t => t.PaymentType)
                    .Select(g => 
                    {
                        var total = g.Sum(t => t.Amount);
                        return new PaymentTypeSummaryDto
                        {
                            PaymentType = g.Key.ToString(),
                            Amount = total,
                            Percentage = totalExpense > 0
                            ? (double)Math.Round((total / totalExpense) * 100, 2)
                            : 0
                        };
                    })
                    .OrderByDescending(g => g.Amount)
                    .ToList(),
            };

            return report;
        }

        private (DateTime start, DateTime end) CalculateDates(string range, DateTime? StartDate, DateTime? EndDate)
        {
            var now = DateTime.UtcNow;

            return range.ToLower() switch
            {
                "daily" => (now.Date, now.Date.AddDays(1).AddTicks(-1)),

                "weekly" => (now.AddDays(-7).Date, now.Date.AddDays(1).AddTicks(-1)),

                "monthly" => (new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc),
                              new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59, 999, DateTimeKind.Utc)),

                "yearly" => (new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                             new DateTime(now.Year, 12, 31, 23, 59, 59, 999, DateTimeKind.Utc)),

                "custom" when StartDate.HasValue && EndDate.HasValue =>
                    (DateTime.SpecifyKind(StartDate.Value.Date, DateTimeKind.Utc),
                     DateTime.SpecifyKind(EndDate.Value.Date.AddDays(1).AddTicks(-1), DateTimeKind.Utc)),

                _ => throw new ArgumentException("Invalid range specified.")
            };
        }
    }
}
