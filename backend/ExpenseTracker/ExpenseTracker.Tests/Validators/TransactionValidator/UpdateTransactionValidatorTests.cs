using ExpenseTracker.DTOs.TransactionDtos;
using ExpenseTracker.Data.Enums;
using ExpenseTracker.Validators.TransactionValidator;
using FluentValidation.TestHelper;

namespace ExpenseTracker.Tests.Validators.TransactionValidator
{
    public class UpdateTransactionValidatorTests
    {
        private readonly UpdatedTransactionValidator _validator = new();

        [Fact]
        public void Should_Not_Have_Error_When_Dto_Is_Empty()
        {
            var dto = new UpdateTransactionDto();
            var result = _validator.TestValidate(dto);
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Empty_String()
        {
            var dto = new UpdateTransactionDto { Description = "" };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(t => t.Description)
                .WithErrorMessage("A descrição não pode ser vazia ao atualizar.");
        }

        [Fact]
        public void Should_Have_Error_When_Amount_Is_Zero_Or_Negative()
        {
            var dto = new UpdateTransactionDto { Amount = 0 };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(t => t.Amount);
        }

        [Fact]
        public void Should_Have_Error_When_Date_Is_In_Future()
        {
            var dto = new UpdateTransactionDto { Date = DateTime.UtcNow.AddDays(1) };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(t => t.Date);
        }

        [Fact]
        public void Should_Have_Error_When_Type_Is_Invalid_Enum()
        {
            var dto = new UpdateTransactionDto { Type = (TransactionType)99 };
            var result = _validator.TestValidate(dto);
            result.ShouldHaveValidationErrorFor(t => t.Type);
        }
    }
}