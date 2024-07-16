using DomainEntities.CodeRunDto.Command;
using FluentValidation;

namespace CodeRunService.Application.Validators;

public class CreateCodeRunRequestValidator : AbstractValidator<CreateCodeRunRequest>
{
    public CreateCodeRunRequestValidator()
    {
        // RuleFor(x => x.Name)
        //     .NotEmpty().WithMessage("Name is required.")
        //     .MaximumLength(50).WithMessage("Name must be less than 50 characters.");
        // RuleFor(x => x.CodeRunName)
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