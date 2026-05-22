using ExpenseTracker.DTOs.TransactionDtos;
using ExpenseTracker.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExpenseTracker.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : BaseController
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTransactions([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            if (pageNumber < 1 ) pageNumber = 1;
            if (pageSize < 1 || pageSize > 50) pageSize = 10;

            int userId = GetUserId();
            var pagedResult = await _transactionService.GetAllTransactionsAsync(userId, pageNumber, pageSize);

            return Ok(pagedResult);
        }

        [HttpGet("{transactionId:int}")]
        public async Task<IActionResult> GetTransactionById(int transactionId)
        {
            int userId = GetUserId();
            var transaction = await _transactionService.GetTransactionByIdAsync(userId, transactionId);

            if (transaction == null) return NotFound();

            return Ok(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTransaction(CreateTransactionDto request)
        {
            int userId = GetUserId();
            var createdTransaction = await _transactionService.CreateTransactionAsync(userId, request);

            if (createdTransaction == null) return BadRequest("Falha em criar uma nova transação.");

            return CreatedAtAction(nameof(GetTransactionById), new { transactionId = createdTransaction.Id }, createdTransaction);
        }

        [HttpPut("{transactionId:int}")]
        public async Task<IActionResult> UpdateTransaction(UpdateTransactionDto request, int transactionId)
        {
            int userId = GetUserId();
            var updatedTransaction = await _transactionService.UpdateTransactionAsync(userId, transactionId, request);

            if (updatedTransaction == null) return NotFound();

            return Ok(updatedTransaction);
        }

        [HttpDelete("{transactionId:int}")]
        public async Task<IActionResult> DeleteTransaction(int transactionId)
        {
            int userId = GetUserId();
            var isDeleted = await _transactionService.DeleteTransactionAsync(userId, transactionId);

            if (!isDeleted) return NotFound();

            return NoContent();
        }
    }
}
