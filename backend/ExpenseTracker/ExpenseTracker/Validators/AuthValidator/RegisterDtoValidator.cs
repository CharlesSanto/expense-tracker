using FluentValidation;
using ExpenseTracker.DTOs.Auth;

namespace ExpenseTracker.Validators.AuthValidator
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator() 
        { 
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Campo obrigatório");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Campo obrigatório")
                .EmailAddress().WithMessage("Email inválido");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Campo obrigatório")
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres");
            
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Campo obrigatório")
                .Equal(x => x.Password).WithMessage("As senhas não coincidem")
                .When(x => !string.IsNullOrWhiteSpace(x.Password));


        }
    }
}
