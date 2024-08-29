using CodeRunService.Infrastructure.Database;
using Common;
using ExternalDomainEntities.CodeRunDto.Events;
using Generics.Enums;
using Microsoft.AspNetCore.SignalR;

namespace CodeRunService.Application.Services;

public class CodeValidationService(
    IServiceProvider serviceProvider,
    ICodeRunRepository codeRunRepository,
    IEventPublisher eventPublisher,
    ILogger<CodeValidationService> logger)
    : ICodeValidationService
{
    public async Task HandleValidationResultAsync(CodeRunValidationResultEvent validationEvent)
    {
        logger.LogInformation("Handling validation result for CodeRunId: {CodeRunId}", validationEvent.CodeRunId);
        var result = false;
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
            result = true;
            await eventPublisher.PublishAsync(eventToPublish, EventQueueList.CodeExecutionQueue);
        }
        else
        {
            logger.LogInformation("Validation failed for CodeRunId: {CodeRunId}. Updating status to ErrorValidating.", validationEvent.CodeRunId);

            codeRun.Status = RunStatus.ErrorValidating;
            await codeRunRepository.UpdateAsync(codeRun.Id, codeRun);
        }

        logger.LogInformation("Validation result handled for CodeRunId: {CodeRunId}", validationEvent.CodeRunId);
        
        // Trigger SignalR update
        logger.LogInformation("Triggering CodeValidationNotificationEvent for CodeRunId: {CodeRunId}",
            validationEvent.CodeRunId);

        var hubContext = serviceProvider.GetService<IHubContext<CodeRunHub>>();
        await hubContext.Clients.All.SendAsync("ReceiveStatusUpdate", validationEvent.CodeRunId.ToString(),
            result ? RunStatus.Ready.ToString() : RunStatus.ErrorValidating.ToString());
        logger.LogInformation("Notification through hub send");
    }
}
