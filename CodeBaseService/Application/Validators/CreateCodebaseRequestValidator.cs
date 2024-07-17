using DomainEntities.CodeBaseDto.Command;
using FluentValidation;

namespace CodebaseService.Application.Validators;

public class CreateCodebaseRequestValidator : AbstractValidator<CreateCodebaseRequest>
{
    public CreateCodebaseRequestValidator()
    {
    }
}