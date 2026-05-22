using ExpenseTracker.Data.Enums;
using ExpenseTracker.DTOs.TransactionDtos;
using ExpenseTracker.Validators.TransactionValidator;
using FluentAssertions;
using Xunit;

namespace ExpenseTracker.Tests.Validators.TransactionValidator
{
    public class CreateTransactionValidatorTests
    {
        private readonly CreateTransactionValidator _validator;

        public CreateTransactionValidatorTests()
        {
            _validator = new CreateTransactionValidator();
        }

        [Fact]
        public void Validate_WhenDtoIsValid_ShouldNotHaveValidationErrors()
        {
            var dto = new CreateTransactionDto
            {
                Description = "Almoço de negócios",
                Amount = 45.50m,
                Category = Category.Food,
                Type = TransactionType.Expense,
                PaymentType = PaymentType.Pix,
                Date = DateTime.UtcNow.AddHours(-1)
            };

            var result = _validator.Validate(dto);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-50.35)]
        public void Validate_WhenAmountIsInvalid_ShouldHaveValidationError(decimal invalidAmount)
        {
            var dto = new CreateTransactionDto
            {
                Description = "Combustível",
                Amount = invalidAmount,
                Category = Category.Transportation,
                Type = TransactionType.Expense,
                PaymentType = PaymentType.DebitCard,
                Date = DateTime.UtcNow.AddHours(-1)
            };

            var result = _validator.Validate(dto);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(dto.Amount));
        }

        [Fact]
        public void Validate_WhenDescriptionIsEmpty_ShouldHaveValidationError()
        {
            var dto = new CreateTransactionDto
            {
                Description = "",
                Amount = 100m,
                Category = Category.Leisure,
                Type = TransactionType.Expense,
                PaymentType = PaymentType.Cash,
                Date = DateTime.UtcNow
            };

            var result = _validator.Validate(dto);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(dto.Description));
        }

        [Fact]
        public void Validate_WhenDateIsInFuture_ShouldHaveValidationError()
        {
            var dto = new CreateTransactionDto
            {
                Description = "Inscrição Maratona",
                Amount = 120m,
                Category = Category.Leisure,
                Type = TransactionType.Expense,
                PaymentType = PaymentType.CreditCard,
                Date = DateTime.UtcNow.AddDays(1)
            };

            var result = _validator.Validate(dto);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(e => e.PropertyName == nameof(dto.Date));
        }
    }
}
