using ByteBox.FileStore.Application.Commands;
using FluentValidation;

namespace ByteBox.FileStore.Application.Validators;

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(o => o.UserName)
            .NotNull()
            .NotEmpty()
            .WithMessage("{UserName} is required")
            .MaximumLength(50)
            .WithMessage("{UserName} must not exceed 50 characters");

        RuleFor(o => o.Email)
            .NotNull()
            .NotEmpty()
            .WithMessage("{Email} is required")
            .EmailAddress()
            .WithMessage("{Email} is not valid");
    }
}
