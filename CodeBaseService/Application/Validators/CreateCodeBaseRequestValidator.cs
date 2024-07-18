using ExternalDomainEntities.CodeBaseDto.Command;
using FluentValidation;

namespace CodebaseService.Application.Validators;

public class CreateCodeBaseRequestValidator : AbstractValidator<CreateCodeBaseRequest>
{
    public CreateCodeBaseRequestValidator()
    {
    }
}