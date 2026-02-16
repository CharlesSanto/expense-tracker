using ExpenseTracker.DTOs.ReportDtos;

namespace ExpenseTracker.Services.Interfaces
{
    public interface IReportService
    {
        Task<FinancialReportDto> GenerateFinancialReport(int userId, string range, DateTime? StartDate, DateTime? EndDate);

    }
}
