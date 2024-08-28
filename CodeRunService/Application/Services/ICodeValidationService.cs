using ExternalDomainEntities.CodeRunDto.Events;

namespace CodeRunService.Application.Services;

public interface ICodeValidationService
{
    public Task HandleValidationResultAsync(CodeRunValidationResultEvent validationEvent);
}