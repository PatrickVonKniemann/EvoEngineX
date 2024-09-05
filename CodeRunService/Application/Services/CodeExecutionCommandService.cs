using CodeRunService.Infrastructure.Database;
using Common;
using DomainEntities;
using ExternalDomainEntities;
using ExternalDomainEntities.CodeRunDto.Events;
using Generics.Enums;
using Microsoft.AspNetCore.SignalR;

namespace CodeRunService.Application.Services;

public class CodeExecutionCommandService(
    ICodeRunRepository codeRunRepository,
    IEventPublisher eventPublisher,
    ILogger<CodeExecutionCommandService> logger,
    IHubContext<CodeRunHub>? hubContext)
    : ICodeExecutionCommandService
{
    public async Task HandleExecutionResultAsync(CodeRunExecutionResultEvent? executionEvent)
    {
        if (executionEvent != null)
        {
            logger.LogInformation("Handling execution result for CodeRunId: {CodeRunId}", executionEvent.CodeRunId);

            var codeRun = await codeRunRepository.GetByIdAsync(executionEvent.CodeRunId);
            if (codeRun == null)
            {
                logger.LogWarning("CodeRun with Id {CodeRunId} not found.", executionEvent.CodeRunId);
                return;
            }

            if (executionEvent.IsSuccess)
            {
                await HandleSuccessAsync(codeRun, executionEvent.CodeRunId);
            }
            else
            {
                await HandleFailureAsync(codeRun, executionEvent.CodeRunId);
            }

            await NotifyClientsAsync(executionEvent.CodeRunId, executionEvent.IsSuccess);
        }
    }

    private async Task HandleSuccessAsync(CodeRun codeRun, Guid codeRunId)
    {
        logger.LogInformation("Execution was successful for CodeRunId: {CodeRunId}. Updating status to Done.", codeRunId);

        codeRun.Status = RunStatus.Done;
        codeRun.RunFinish = DateTime.Now;
        codeRun.Results =  await codeRunRepository.ReadLogsFromDatabaseAsync(codeRun.Id);
        
        await codeRunRepository.UpdateAsync(codeRun.Id, codeRun);

        var eventToPublish = new CodeExecutionNotificationEvent
        {
            CodeRunId = codeRunId,
            IsSuccess = true
        };

        logger.LogInformation("Publishing CodeExecutionNotificationEvent for CodeRunId: {CodeRunId}", codeRunId);
        await eventPublisher.PublishAsync(eventToPublish, EventQueueList.CodeExecutionQueueNotification);
    }

    private async Task HandleFailureAsync(CodeRun codeRun, Guid codeRunId)
    {
        logger.LogError("Execution failed for CodeRunId: {CodeRunId}. Updating status to ErrorRunning.", codeRunId);

        codeRun.Status = RunStatus.ErrorRunning;
        await codeRunRepository.UpdateAsync(codeRun.Id, codeRun);
    }

    private async Task NotifyClientsAsync(Guid codeRunId, bool isSuccess)
    {
        var status = isSuccess ? RunStatus.Done.ToString() : RunStatus.ErrorRunning.ToString();

        logger.LogInformation("Triggering CodeExecutionNotificationEvent for CodeRunId: {CodeRunId}", codeRunId);

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
