using CodeRunService.Infrastructure.Database;
using Common;
using DomainEntities;
using ExternalDomainEntities.CodeRunDto.Events;
using Generics.Enums;
using Microsoft.AspNetCore.SignalR;

namespace CodeRunService.Application.Services;

public class CodeValidationService(
    ICodeRunRepository codeRunRepository,
    IEventPublisher eventPublisher,
    ILogger<CodeValidationService> logger,
    IHubContext<CodeRunHub>? hubContext)
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
            await HandleSuccessfulValidationAsync(validationEvent, codeRun);
        }
        else
        {
            await HandleFailedValidationAsync(validationEvent, codeRun);
        }

        await NotifyClientsAsync(validationEvent.CodeRunId, validationEvent.IsValid);
    }

    private async Task HandleSuccessfulValidationAsync(CodeRunValidationResultEvent validationEvent, CodeRun codeRun)
    {
        logger.LogInformation("Validation was successful for CodeRunId: {CodeRunId}. Updating status to Ready.", validationEvent.CodeRunId);

        codeRun.Status = RunStatus.Ready;
        await codeRunRepository.UpdateAsync(codeRun.Id, codeRun);

        var eventToPublish = new CodeRunExecutionRequestedEvent
        {
            CodeRunId = validationEvent.CodeRunId,
            Code = codeRun.Code
        };

        logger.LogInformation("Publishing CodeRunExecutionRequestedEvent for CodeRunId: {CodeRunId}", validationEvent.CodeRunId);
        await eventPublisher.PublishAsync(eventToPublish, EventQueueList.CodeExecutionQueue);
    }

    private async Task HandleFailedValidationAsync(CodeRunValidationResultEvent validationEvent, CodeRun codeRun)
    {
        logger.LogError("Validation failed for CodeRunId: {CodeRunId}. Updating status to ErrorValidating.", validationEvent.CodeRunId);

        codeRun.Status = RunStatus.ErrorValidating;
        await codeRunRepository.UpdateAsync(codeRun.Id, codeRun);
    }

    private async Task NotifyClientsAsync(Guid codeRunId, bool isValid)
    {
        var status = isValid ? RunStatus.Running.ToString() : RunStatus.ErrorValidating.ToString();

        logger.LogInformation("Triggering CodeValidationNotificationEvent for CodeRunId: {CodeRunId}", codeRunId);

        if (hubContext != null)
        {
            await hubContext.Clients.All.SendAsync("ReceiveStatusUpdate", codeRunId.ToString(), status);
            logger.LogInformation("Notification sent to clients for CodeRunId: {CodeRunId}", codeRunId);
        }
        else
        {
            logger.LogWarning("HubContext is null, skipping client notification for CodeRunId: {CodeRunId}", codeRunId);
        }
    }
}
