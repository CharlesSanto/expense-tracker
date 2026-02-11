using FluentValidation;
using ExpenseTracker.DTOs.TransactionDtos;

namespace ExpenseTracker.Validators.TransactionValidator
{
    public class CreateTransactionValidator : AbstractValidator<CreateTransactionDto>
    {
        public CreateTransactionValidator()
        {
            RuleFor(t => t.Description)
                .NotEmpty().WithMessage("Campo obrigatório.")
                .MaximumLength(255).WithMessage("Descrição não pode exceder 255 caracteres.");

            RuleFor(t => t.Amount)
                .GreaterThan(0).WithMessage("O valor deve ser maior que zero.");

            RuleFor(t => t.Category)
                .IsInEnum().WithMessage("A categoria informada é inválida.");

            RuleFor(t => t.Type)
                .IsInEnum().WithMessage("O tipo de transação informado é inválido.");

            RuleFor(t => t.PaymentType)
                .IsInEnum().WithMessage("O tipo de pagamento informado é inválido.");

            RuleFor(t => t.Date)
                .NotEmpty().WithMessage("Campo obrigatório.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("A data não pode ser no futuro.");
        }
    }
}
