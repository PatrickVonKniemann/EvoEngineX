using ExternalDomainEntities.CodeRunDto.Events;

namespace CodeRunService.Application.Services;

public interface ICodeExecutionService
{
    public Task HandleExecutionResultAsync(CodeRunExecutionResultEvent executionEvent);
}