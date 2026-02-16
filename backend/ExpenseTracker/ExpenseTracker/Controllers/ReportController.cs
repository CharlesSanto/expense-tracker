using ExpenseTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFinancialReport([FromQuery] string range, [FromQuery] DateTime? StartDate, [FromQuery] DateTime? EndDate)
        {
            var user = GetUserId();

            try
            {
                var report = await _reportService.GenerateFinancialReport(user, range, StartDate, EndDate);
                return Ok(report);

            } catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }


        }
    }
}
