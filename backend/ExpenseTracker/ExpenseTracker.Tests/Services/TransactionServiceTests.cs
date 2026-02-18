using Xunit;
using Moq;
using FluentAssertions;
using ExpenseTracker.Services;
using ExpenseTracker.Repositories.Interfaces;
using ExpenseTracker.DTOs.TransactionDtos;
using ExpenseTracker.Data.Models;
using ExpenseTracker.Data.Enums;

namespace ExpenseTracker.Tests.Services
{
    public class TransactionServiceTests
    {
        private readonly TransactionService _transactionService;
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock;

        public TransactionServiceTests()
        {
            _transactionRepositoryMock = new Mock<ITransactionRepository>();
            _transactionService = new TransactionService(_transactionRepositoryMock.Object);
        }

        #region Create Tests

        [Fact]
        public async Task CreateTransaction_ShouldReturnNull_WhenAmountIsNegative()
        {
            var dto = new CreateTransactionDto
            {
                Amount = -100,
                Description = "Teste",
                Category = Category.Food,
                Type = TransactionType.Expense,
                PaymentType = PaymentType.CreditCard,
                Date = DateTime.UtcNow
            };

            var result = await _transactionService.CreateTransactionAsync(1, dto);

            result.Should().BeNull();

            _transactionRepositoryMock.Verify(r => r.CreateTransactionAsync(It.IsAny<Transaction>()), Times.Never);
        }

        [Fact]
        public async Task CreateTransaction_ShouldCallRepository_WithCorrectMappedValues()
        {
            var dto = new CreateTransactionDto
            {
                Description = "   Teste   ",
                Amount = 100,
                Category = Category.Food,
                Type = TransactionType.Expense,
                PaymentType = PaymentType.CreditCard,
                Date = DateTime.UtcNow
            };

            _transactionRepositoryMock.Setup(r => r.CreateTransactionAsync(It.IsAny<Transaction>()))
                .ReturnsAsync((Transaction t) => t);

            var result = await _transactionService.CreateTransactionAsync(1, dto);

            result.Should().NotBeNull();

            _transactionRepositoryMock.Verify(r => r.CreateTransactionAsync(It.Is<Transaction>(t =>
                t.Amount == 100 &&
                t.Description == "Teste" &&
                t.UserId == 1
            )), Times.Once);
        }

        #endregion

        #region Get Tests

        [Fact]
        public async Task GetTransactionById_ShouldReturnDto_WhenTransactionExists()
        {
            var transaction = new Transaction { Id = 1, UserId = 1, Amount = 100, Description = "Test" };

            _transactionRepositoryMock.Setup(r => r.GetTransactionByIdAsync(1, 1))
                .ReturnsAsync(transaction);

            var result = await _transactionService.GetTransactionByIdAsync(1, 1);

            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetTransactionById_ShouldReturnNull_WhenTransactionDoesNotExist()
        {
            _transactionRepositoryMock.Setup(r => r.GetTransactionByIdAsync(1, 99))
                .ReturnsAsync((Transaction?)null);

            var result = await _transactionService.GetTransactionByIdAsync(1, 99);

            result.Should().BeNull();
        }

        #endregion

        #region Update Tests

        [Fact]
        public async Task UpdateTransaction_ShouldUpdateFields_WhenInputIsValid()
        {
            var existingTransaction = new Transaction
            {
                Id = 1,
                UserId = 1,
                Amount = 50,
                Description = "Old",
                UpdatedAt = DateTime.MinValue
            };

            var updateDto = new UpdateTransactionDto
            {
                Amount = 200,
                Description = " New Desc "
            };

            _transactionRepositoryMock.Setup(r => r.GetTransactionByIdAsync(1, 1))
                .ReturnsAsync(existingTransaction);

            _transactionRepositoryMock.Setup(r => r.UpdateTransactionAsync(1, It.IsAny<Transaction>()))
                .ReturnsAsync((int id, Transaction t) => t);

            var result = await _transactionService.UpdateTransactionAsync(1, 1, updateDto);
                
            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Description.Should().Be("New Desc");

            _transactionRepositoryMock.Verify(r => r.UpdateTransactionAsync(1, It.Is<Transaction>(t =>
                t.Amount == 200 &&
                t.Description == "New Desc" &&
                t.UpdatedAt > DateTime.MinValue
            )), Times.Once);
        }

        [Fact]
        public async Task UpdateTransaction_ShouldReturnNull_WhenTransactionNotFound()
        {
            _transactionRepositoryMock.Setup(r => r.GetTransactionByIdAsync(1, 99))
                .ReturnsAsync((Transaction?)null);

            var result = await _transactionService.UpdateTransactionAsync(1, 99, new UpdateTransactionDto());

            result.Should().BeNull();
            _transactionRepositoryMock.Verify(r => r.UpdateTransactionAsync(It.IsAny<int>(), It.IsAny<Transaction>()), Times.Never);
        }

        [Fact]
        public async Task UpdateTransaction_ShouldReturnNull_WhenNewAmountIsInvalid()
        {
            var existingTransaction = new Transaction { Id = 1, UserId = 1, Amount = 50 };
            var updateDto = new UpdateTransactionDto { Amount = -10 };

            _transactionRepositoryMock.Setup(r => r.GetTransactionByIdAsync(1, 1))
                .ReturnsAsync(existingTransaction);

            var result = await _transactionService.UpdateTransactionAsync(1, 1, updateDto);

            result.Should().BeNull();

            _transactionRepositoryMock.Verify(r => r.UpdateTransactionAsync(It.IsAny<int>(), It.IsAny<Transaction>()), Times.Never);
        }

        #endregion

        #region Delete Tests

        [Fact]
        public async Task DeleteTransaction_ShouldReturnTrue_WhenRepositoryReturnsTrue()
        {
            _transactionRepositoryMock.Setup(r => r.DeleteTransactionAsync(1, 1))
                .ReturnsAsync(true);

            var result = await _transactionService.DeleteTransactionAsync(1, 1);

            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteTransaction_ShouldReturnFalse_WhenRepositoryReturnsFalse()
        {
            _transactionRepositoryMock.Setup(r => r.DeleteTransactionAsync(1, 1))
                .ReturnsAsync(false);

            var result = await _transactionService.DeleteTransactionAsync(1, 1);

            result.Should().BeFalse();
        }

        #endregion
    }
}