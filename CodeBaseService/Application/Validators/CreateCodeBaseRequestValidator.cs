using ExternalDomainEntities.CodeBaseDto.Command;
using FluentAssertions;
using FluentValidation;

namespace CodebaseService.Application.Validators;

public class CreateCodeBaseRequestValidator : AbstractValidator<CreateCodeBaseRequest>
{
    public CreateCodeBaseRequestValidator()
    {
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");
        // RuleFor(x => x.SupportedPlatform)
        //     .NotEmpty().WithMessage("SupportedPlatform is required.");
        RuleFor(x => x.Valid)
            .Equal(false).WithMessage("Valid must be false.");

    }
}