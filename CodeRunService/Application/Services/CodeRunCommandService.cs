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
    IEventPublisher eventPublisher,
    IConnectionFactory rabbitMqConnectionFactory)
    : BaseCommandService<CodeRun, CreateCodeRunDetailRequest, CreateCodeRunResponse, UpdateCodeRunRequest,
        UpdateCodeRunResponse>(mapper, codeRunRepository, logger), ICodeRunCommandService
{
    public async Task<CreateCodeRunResponse> HandleAddAsync(CreateCodeRunRequest req)
    {
        var newCodeRun = CreateNewCodeRunRequest(req);
        var createResponse = await AddAsync(newCodeRun);

        PublishStatusUpdate(newCodeRun.Status);
        await PublishCodeValidationRequestedEventAsync(createResponse.Id, newCodeRun.Code);

        return new CreateCodeRunResponse
        {
            Id = createResponse.Id,
            Status = "Accepted",
            Message = "Code run creation has started and will be processed."
        };
    }

    public async Task HandleValidationResultAsync(CodeRunValidationResultEvent validationEvent)
    {
        var codeRun = await codeRunRepository.GetByIdAsync(validationEvent.CodeRunId);

        if (validationEvent.IsValid)
        {
            await UpdateAndPublishStatusAsync(codeRun, RunStatus.Ready, new CodeRunExecutionRequestedEvent
            {
                CodeRunId = validationEvent.CodeRunId,
                Code = codeRun.Code
            });
        }
        else
        {
            await UpdateStatusAsync(codeRun, RunStatus.ErrorValidating);
        }
    }

    public async Task HandleExecutionResultAsync(CodeRunExecutionResultEvent executionEvent)
    {
        var codeRun = await codeRunRepository.GetByIdAsync(executionEvent.CodeRunId);
        var newStatus = executionEvent.IsSuccess ? RunStatus.Done : RunStatus.ErrorRunning;

        await UpdateStatusAsync(codeRun, newStatus);
    }

    private CreateCodeRunDetailRequest CreateNewCodeRunRequest(CreateCodeRunRequest req)
    {
        return new CreateCodeRunDetailRequest
        {
            CodeBaseId = req.CodeBaseId,
            Code = req.Code,
            Status = RunStatus.Created,
            RunStart = DateTimeOffset.UtcNow.UtcDateTime
        };
    }

    private async Task PublishCodeValidationRequestedEventAsync(Guid codeRunId, string code)
    {
        await eventPublisher.PublishAsync(new CodeRunValidationRequestedEvent
        {
            CodeRunId = codeRunId,
            Code = code
        });
    }

    private async Task UpdateAndPublishStatusAsync(CodeRun codeRun, RunStatus newStatus, CodeRunExecutionRequestedEvent eventToPublish)
    {
        await UpdateStatusAsync(codeRun, newStatus);
        await eventPublisher.PublishAsync(eventToPublish);
    }

    private async Task UpdateStatusAsync(CodeRun codeRun, RunStatus newStatus)
    {
        codeRun.Status = newStatus;
        await codeRunRepository.UpdateAsync(codeRun.Id, codeRun);
        PublishStatusUpdate(newStatus);
    }

    private void PublishStatusUpdate(RunStatus status)
    {
        using var connection = rabbitMqConnectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(
            queue: "codeRunStatusQueue",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var message = JsonSerializer.Serialize(new { Status = status });
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(
            exchange: "",
            routingKey: "codeRunStatusQueue",
            basicProperties: null,
            body: body
        );

        logger.LogInformation($"Status update published: {status}");
    }
   
}