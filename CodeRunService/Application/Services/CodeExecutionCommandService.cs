using CodeRunService.Infrastructure.Database;
using Common;
using ExternalDomainEntities;
using ExternalDomainEntities.CodeRunDto.Events;
using Generics.Enums;

namespace CodeRunService.Application.Services;

public class CodeExecutionCommandService(
    ICodeRunRepository codeRunRepository,
    IEventPublisher eventPublisher,
    ILogger<CodeExecutionCommandService> logger)
    : ICodeExecutionCommandService
{
    public async Task HandleExecutionResultAsync(CodeRunExecutionResultEvent executionEvent)
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
            logger.LogInformation("Execution was successful for CodeRunId: {CodeRunId}. Updating status to Done.",
                executionEvent.CodeRunId);

            // Update codeRun status in the DB
            codeRun.Status = RunStatus.Done;
            await codeRunRepository.UpdateAsync(codeRun.Id, codeRun);

            // Create event that will inform the GUI that it went successful
            var eventToPublish = new CodeExecutionNotificationEvent
            {
                CodeRunId = executionEvent.CodeRunId,
                IsSuccess = true
            };

            logger.LogInformation("Publishing CodeExecutionNotificationEvent for CodeRunId: {CodeRunId}",
                executionEvent.CodeRunId);
            await eventPublisher.PublishAsync(eventToPublish, EventQueueList.CodeExecutionQueueNotification);
        }
        else
        {
            logger.LogInformation("Execution failed for CodeRunId: {CodeRunId}. Updating status to ErrorRunning.",
                executionEvent.CodeRunId);

            codeRun.Status = RunStatus.ErrorRunning;
            await codeRunRepository.UpdateAsync(codeRun.Id, codeRun);
        }

        logger.LogInformation("Execution result handled for CodeRunId: {CodeRunId}", executionEvent.CodeRunId);
    }
}