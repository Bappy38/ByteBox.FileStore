using ByteBox.FileStore.Application.Commands;
using FluentValidation;

namespace ByteBox.FileStore.Application.Validators;

public class CreateFolderCommandValidator : AbstractValidator<CreateFolderCommand>
{
    public CreateFolderCommandValidator()
    {
        RuleFor(o => o.FolderName)
            .NotNull()
            .NotEmpty()
            .WithMessage("{FolderName} is required")
            .MaximumLength(50)
            .WithMessage("{FolderName} must not exceed 50 characters");

        RuleFor(o => o.ParentFolderId)
            .NotNull()
            .NotEmpty()
            .WithMessage("{ParentFolderId} is required");
    }
}
