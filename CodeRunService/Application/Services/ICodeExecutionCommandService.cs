using ExternalDomainEntities.CodeRunDto.Events;

namespace CodeRunService.Application.Services;

public interface ICodeExecutionCommandService
{
    public Task HandleExecutionResultAsync(CodeRunExecutionResultEvent executionEvent);
}