using System.Text;
using System.Text.Json;
using Common;
using ExternalDomainEntities;
using ExternalDomainEntities.CodeRunDto.Events;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CodeExecutionService;

public class CodeExecutionRequestConsumer : BackgroundService
{
    private readonly ICodeExecutionLogic _codeExecutionService;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<CodeExecutionRequestConsumer> _logger;

    public CodeExecutionRequestConsumer(
        IConnectionFactory connectionFactory,
        ICodeExecutionLogic codeExecutionService,
        IEventPublisher eventPublisher,
        ILogger<CodeExecutionRequestConsumer> logger)
    {
        _codeExecutionService = codeExecutionService;
        _eventPublisher = eventPublisher;
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _logger = logger;

        _logger.LogInformation("CodeExecutionRequestConsumer created, RabbitMQ connection established.");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting CodeExecutionRequestConsumer execution...");

        _channel.QueueDeclare(
            queue: EventQueueList.CodeExecutionQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            _logger.LogInformation("Message received from queue: {QueueName}", EventQueueList.CodeExecutionQueue);

            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogDebug("Message content: {Message}", message);

                var executionEvent = JsonSerializer.Deserialize<CodeExecutionRequestEvent>(message);
                if (executionEvent != null)
                {
                    _logger.LogInformation("Executing code for CodeRunId: {CodeRunId}", executionEvent.CodeRunId);

                    var executionResult = await _codeExecutionService.ExecuteAsync(executionEvent.Code);

                    _logger.LogInformation("Execution result for CodeRunId {CodeRunId}: {ExecutionResult}",
                        executionEvent.CodeRunId, executionResult);

                    // Create event that will indicate the result of the execution
                    var eventResultToPublish = new CodeRunExecutionResultEvent
                    {
                        CodeRunId = executionEvent.CodeRunId,
                        IsSuccess = executionResult
                    };

                    _logger.LogInformation("Publishing execution result for CodeRunId: {CodeRunId}",
                        executionEvent.CodeRunId);
                    await _eventPublisher.PublishAsync(eventResultToPublish, EventQueueList.CodeExecutionQueueResult);
                }
                else
                {
                    _logger.LogWarning("Received invalid or null execution event.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the message.");
            }
        };

        _channel.BasicConsume(queue: EventQueueList.CodeExecutionQueue, autoAck: true, consumer: consumer);

        _logger.LogInformation("CodeExecutionRequestConsumer is now consuming messages.");

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _logger.LogInformation("Disposing CodeExecutionRequestConsumer, closing channel and connection.");
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}