using CodeRunService.Infrastructure.Database;
using Common;
using ExternalDomainEntities.CodeRunDto.Events;
using Generics.Enums;

namespace CodeRunService.Application.Services;

public class CodeValidationService(
    ICodeRunRepository codeRunRepository,
    IEventPublisher eventPublisher,
    ILogger<CodeValidationService> logger)
    : ICodeValidationService
{
    public async Task HandleValidationResultAsync(CodeRunValidationResultEvent validationEvent)
    {
        logger.LogInformation("Handling validation result for CodeRunId: {CodeRunId}", validationEvent.CodeRunId);

        var codeRun = await codeRunRepository.GetByIdAsync(validationEvent.CodeRunId);
        if (codeRun == null)
        {
            logger.LogWarning("CodeRun with Id {CodeRunId} not found.", validationEvent.CodeRunId);
            return;
        }

        if (validationEvent.IsValid)
        {
            logger.LogInformation("Validation was successful for CodeRunId: {CodeRunId}. Updating status to Ready.", validationEvent.CodeRunId);

            // Update codeRun status in the DB
            codeRun.Status = RunStatus.Ready;
            await codeRunRepository.UpdateAsync(codeRun.Id, codeRun);

            // Create event that will start the execution in the execution service
            var eventToPublish = new CodeRunExecutionRequestedEvent
            {
                CodeRunId = validationEvent.CodeRunId,
                Code = codeRun.Code
            };

            logger.LogInformation("Publishing CodeRunExecutionRequestedEvent for CodeRunId: {CodeRunId}", validationEvent.CodeRunId);
            await eventPublisher.PublishAsync(eventToPublish, EventQueueList.CodeExecutionQueue);
        }
        else
        {
            logger.LogInformation("Validation failed for CodeRunId: {CodeRunId}. Updating status to ErrorValidating.", validationEvent.CodeRunId);

            codeRun.Status = RunStatus.ErrorValidating;
            await codeRunRepository.UpdateAsync(codeRun.Id, codeRun);
        }

        logger.LogInformation("Validation result handled for CodeRunId: {CodeRunId}", validationEvent.CodeRunId);
    }
}
