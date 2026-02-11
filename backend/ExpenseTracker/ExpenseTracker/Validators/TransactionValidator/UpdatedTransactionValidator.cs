using ExpenseTracker.DTOs.TransactionDtos;
using FluentValidation;

namespace ExpenseTracker.Validators.TransactionValidator
{
    public class UpdatedTransactionValidator : AbstractValidator<UpdateTransactionDto>
    {
        public UpdatedTransactionValidator()
        {
            RuleFor(t => t.Description)
                .NotEmpty().WithMessage("A descrição não pode ser vazia ao atualizar.")
                .MaximumLength(255).WithMessage("Descrição não pode exceder 255 caracteres.")
                .When(t => t.Description != null);

            RuleFor(t => t.Amount)
                .GreaterThan(0).WithMessage("O valor deve ser maior que zero.")
                .When(t => t.Amount.HasValue);
                
            RuleFor(t => t.Category)
                .IsInEnum().WithMessage("A categoria informada é inválida.")
                .When(t => t.Category.HasValue);

            RuleFor(t => t.PaymentType)
                .IsInEnum().WithMessage("O tipo de pagamento informado é inválido.")
                .When(t => t.PaymentType.HasValue);

            RuleFor(t => t.Date)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("A data não pode ser no futuro.")
                .When(t => t.Date.HasValue);
        }
    }
}
