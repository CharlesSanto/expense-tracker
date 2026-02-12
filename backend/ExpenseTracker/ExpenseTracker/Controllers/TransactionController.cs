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
        public async Task<IActionResult> GetAllTransactions()
        {
            int userId = GetUserId();

            var transactions = await _transactionService.GetAllTransactionsAsync(userId);
            return Ok(transactions);
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

        [HttpPatch("{transactionId:int}")]
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
