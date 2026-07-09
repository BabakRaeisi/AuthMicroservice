using Auth.Core.DTO;
using FluentValidation;

namespace Auth.Core.Validators;

internal class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        RuleFor(x => x.PersonName)
            .NotEmpty().WithMessage("Person name is required.")
            .MinimumLength(2).WithMessage("Person name must be at least 2 characters long.");
        RuleFor(x => x.Gender)
        .IsInEnum().WithMessage("Gender must be a valid enum value.");

    }
}
