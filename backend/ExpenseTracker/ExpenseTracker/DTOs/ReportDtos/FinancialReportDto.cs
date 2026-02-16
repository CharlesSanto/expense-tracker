using ExpenseTracker.Data.Enums;

namespace ExpenseTracker.DTOs.ReportDtos
{
    public class FinancialReportDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalIncome { get; set; }
        public decimal TotalExpense { get; set; }
        public decimal PeriodBalance => TotalIncome - TotalExpense;
        public List<CategorySummaryDto> IncomeCategories { get; set; } = [];
        public List<CategorySummaryDto> ExpenseCategories { get; set; } = [];
        public List<PaymentTypeSummaryDto> PaymentMethods { get; set; } = [];
    }

    public class CategorySummaryDto
    {
        public string CategoryName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public double Percentage { get; set; }
    }

    public class PaymentTypeSummaryDto
    {
        public string PaymentType { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public double Percentage { get; set; }
    }
}
