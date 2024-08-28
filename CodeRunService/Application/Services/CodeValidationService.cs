using CodeRunService.Infrastructure.Database;
using Common;
using ExternalDomainEntities.CodeRunDto.Events;
using Generics.Enums;

namespace CodeRunService.Application.Services;

public class CodeValidationService(
    ICodeRunRepository codeRunRepository,
    IEventPublisher eventPublisher,
    ILogger<CodeValidationService> logger) : ICodeValidationService
{
    public async Task HandleValidationResultAsync(CodeRunValidationResultEvent validationEvent)
    {
        var codeRun = await codeRunRepository.GetByIdAsync(validationEvent.CodeRunId);

        if (validationEvent.IsValid)
        {
            // Update codeRun status in the DB
            codeRun.Status = RunStatus.Ready;
            await codeRunRepository.UpdateAsync(codeRun.Id, codeRun);

            // Create event that will start the execution in the execution service
            var eventToPublish = new CodeRunExecutionRequestedEvent
            {
                CodeRunId = validationEvent.CodeRunId,
                Code = codeRun.Code
            };

            await eventPublisher.PublishAsync(eventToPublish, EventQueueList.CodeExecutionQueue);
        }
        else
        {
            codeRun.Status = RunStatus.ErrorValidating;
            await codeRunRepository.UpdateAsync(codeRun.Id, codeRun);
        }
    }
}