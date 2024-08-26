using System.Text;
using System.Text.Json;
using AutoMapper;
using Generics.BaseEntities;
using CodeRunService.Infrastructure.Database;
using DomainEntities;
using ExternalDomainEntities.CodeRunDto.Command;
using Generics.Enums;
using RabbitMQ.Client;
using Common;
using ExternalDomainEntities.CodeRunDto.Events;

namespace CodeRunService.Application.Services;

public class CodeRunCommandService(
    ILogger<CodeRunCommandService> logger,
    IMapper mapper,
    ICodeRunRepository codeRunRepository,
    IEventPublisher eventPublisher)
    : BaseCommandService<CodeRun, CreateCodeRunDetailRequest, CreateCodeRunResponse, UpdateCodeRunRequest,
        UpdateCodeRunResponse>(mapper, codeRunRepository, logger), ICodeRunCommandService
{
    public async Task<CreateCodeRunResponse> HandleAddAsync(CreateCodeRunRequest req)
    {
        var now = DateTimeOffset.UtcNow;
        var formattedDateTime = now.UtcDateTime;
        var newCodeRun = new CreateCodeRunDetailRequest
        {
            CodeBaseId = req.CodeBaseId,
            Code = req.Code,
            Status = RunStatus.Created,
            RunStart = formattedDateTime
        };

        var createResponse = await AddAsync(newCodeRun);

        // Publish initial status to RabbitMQ
        PublishStatusUpdate(newCodeRun.Status);

        // Publish event to initiate code validation
        await eventPublisher.PublishAsync(new CodeRunValidationRequestedEvent
        {
            CodeRunId = createResponse.Id,
            Code = newCodeRun.Code
        });

        // Return a response indicating the request was accepted and processing has started
        return new CreateCodeRunResponse
        {
            Id = createResponse.Id,
            Status = "Accepted",
            Message = "Code run creation has started and will be processed."
        };
    }
    
    private void PublishStatusUpdate(RunStatus status)
    {
        var factory = new ConnectionFactory() { HostName = "localhost", UserName = "kolenpat", Password = "sa"};
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: "codeRunStatusQueue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        string message = JsonSerializer.Serialize(new { Status = status });
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "",
            routingKey: "codeRunStatusQueue",
            basicProperties: null,
            body: body);

        logger.LogInformation($"Status update published: {status}");
    }
    

    // Method to update the status based on external events (triggered by event listeners)
    public async Task HandleValidationResultAsync(CodeRunValidationResultEvent validationEvent)
    {
        var codeRun = await codeRunRepository.GetByIdAsync(validationEvent.CodeRunId);

        if (validationEvent.IsValid)
        {
            // Update status to ReadyToRun
            await UpdateStatusAsync(codeRun, RunStatus.Ready);

            // Publish event to run the code
            await eventPublisher.PublishAsync(new CodeRunExecutionRequestedEvent
            {
                CodeRunId = validationEvent.CodeRunId,
                Code = codeRun.Code
            });
        }
        else
        {
            // Update status to ValidationFailed
            await UpdateStatusAsync(codeRun, RunStatus.ErrorValidating);
        }
    }

    // Method to handle the result of the code execution
    public async Task HandleExecutionResultAsync(CodeRunExecutionResultEvent executionEvent)
    {
        var codeRun = await codeRunRepository.GetByIdAsync(executionEvent.CodeRunId);

        var newStatus = executionEvent.IsSuccess ? RunStatus.Done : RunStatus.ErrorRunning;
        await UpdateStatusAsync(codeRun, newStatus);
    }

    private async Task UpdateStatusAsync(CodeRun codeRun, RunStatus newStatus)
    {
        codeRun.Status = newStatus;
        await codeRunRepository.UpdateAsync(codeRun.Id, codeRun);
        PublishStatusUpdate(newStatus);
    }

   
}