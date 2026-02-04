using FluentValidation;
using ExpenseTracker.DTOs.Auth;

namespace ExpenseTracker.Validators.AuthValidator
{
    public class LoginDtoValidator : AbstractValidator<LoginDto>
    {
        public LoginDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Campo obrigatório.")
                .EmailAddress().WithMessage("Email inválido.");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Campo obrigatório.")
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.");
        }
    }
}
