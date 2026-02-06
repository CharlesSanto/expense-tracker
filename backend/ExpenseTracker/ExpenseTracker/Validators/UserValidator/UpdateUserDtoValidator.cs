using FluentValidation;
using ExpenseTracker.DTOs.UserDTOs;

namespace ExpenseTracker.Validators.UserValidator
{
    public class UpdateUserDtoValidator : AbstractValidator<UpdateUserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("O nome deve ter no máximo 25 caracteres.")
                .When(x => !string.IsNullOrWhiteSpace(x.Name));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("O email deve ser válido.")
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Password)
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres.")
                .When(x => !string.IsNullOrEmpty(x.Password));

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("As senhas devem coincidir.")
                .When(x => !string.IsNullOrEmpty(x.Password));
        }
    }
}
