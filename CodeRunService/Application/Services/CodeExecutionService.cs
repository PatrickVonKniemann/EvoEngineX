using CodeRunService.Infrastructure.Database;
using Common;
using ExternalDomainEntities.CodeRunDto.Events;
using Generics.Enums;

namespace CodeRunService.Application.Services;

public class CodeExecutionService(ICodeRunRepository codeRunRepository, IEventPublisher eventPublisher) : ICodeExecutionService
{
    public async Task HandleExecutionResultAsync(CodeRunExecutionResultEvent executionEvent)
    {
        var codeRun = await codeRunRepository.GetByIdAsync(executionEvent.CodeRunId);

        if (executionEvent.IsSuccess)
        {
            // Update codeRun status in the DB
            codeRun.Status = RunStatus.Done;
            await codeRunRepository.UpdateAsync(codeRun.Id, codeRun);

            // Create event that will inform the GUI that it went successful
            var eventToPublish = new CodeRunExecutionRequestedEvent
            {
                CodeRunId = executionEvent.CodeRunId,
                Code = codeRun.Code
            };

            await eventPublisher.PublishAsync(eventToPublish, EventQueueList.CodeExecutionQueue);

        }
        else
        {
            codeRun.Status = RunStatus.ErrorRunning;
            await codeRunRepository.UpdateAsync(codeRun.Id, codeRun);
        }
    }

}