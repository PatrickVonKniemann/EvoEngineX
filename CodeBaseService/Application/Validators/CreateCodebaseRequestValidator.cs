using DomainEntities.CodeBaseDto.Command;
using FluentValidation;

namespace CodebaseService.Application.Validators;

public class CreateCodebaseRequestValidator : AbstractValidator<CreateCodebaseRequest>
{
    public CreateCodebaseRequestValidator()
    {
        // RuleFor(x => x.Name)
        //     .NotEmpty().WithMessage("Name is required.")
        //     .MaximumLength(50).WithMessage("Name must be less than 50 characters.");
        // RuleFor(x => x.CodebaseName)
        //     .NotEmpty().WithMessage("Name is required.")
        //     .MaximumLength(50).WithMessage("Name must be less than 50 characters.");
        //
        // RuleFor(x => x.Email)
        //     .NotEmpty().WithMessage("Email is required.")
        //     .EmailAddress().WithMessage("Email is not valid.");
        //
        // RuleFor(x => x.Language)
        //     .NotEmpty().WithMessage("Language is required.");
    }
}