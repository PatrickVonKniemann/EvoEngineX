using DomainEntities.CodeRunDto.Command;
using FluentValidation;

namespace CodeRunService.Application.Validators;

public class CreateCodeRunRequestValidator : AbstractValidator<CreateCodeRunRequest>
{
    public CreateCodeRunRequestValidator()
    {
        RuleFor(x => x.CodeBaseId)
            .NotEmpty().WithMessage("CodeBaseId is required.");

    }
}