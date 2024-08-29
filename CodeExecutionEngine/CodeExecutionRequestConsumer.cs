using System.Text;
using System.Text.Json;
using Common;
using ExternalDomainEntities;
using ExternalDomainEntities.CodeRunDto.Events;
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
        _logger = logger;

        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        InitializeQueue();
        _logger.LogInformation("CodeExecutionRequestConsumer created and RabbitMQ connection established.");
    }

    private void InitializeQueue()
    {
        _channel.QueueDeclare(
            queue: EventQueueList.CodeExecutionQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        _logger.LogInformation("Queue {QueueName} declared successfully.", EventQueueList.CodeExecutionQueue);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting CodeExecutionRequestConsumer execution...");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (_, ea) => await HandleMessageAsync(ea);

        _channel.BasicConsume(queue: EventQueueList.CodeExecutionQueue, autoAck: true, consumer: consumer);

        _logger.LogInformation("CodeExecutionRequestConsumer is now consuming messages.");

        return Task.CompletedTask;
    }

    private async Task HandleMessageAsync(BasicDeliverEventArgs eventArgs)
    {
        _logger.LogInformation("Message received from queue: {QueueName}", EventQueueList.CodeExecutionQueue);

        try
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogDebug("Message content: {Message}", message);

            var executionEvent = JsonSerializer.Deserialize<CodeExecutionRequestEvent>(message);
            if (executionEvent != null)
            {
                await ProcessExecutionRequestAsync(executionEvent);
            }
            else
            {
                _logger.LogWarning("Received invalid or null execution event.");
            }
        }
        catch (JsonException jsonEx)
        {
            _logger.LogError(jsonEx, "JSON deserialization error occurred while processing the message.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the message.");
        }
    }

    private async Task ProcessExecutionRequestAsync(CodeExecutionRequestEvent executionEvent)
    {
        if (executionEvent == null) throw new ArgumentNullException(nameof(executionEvent));

        _logger.LogInformation("Executing code for CodeRunId: {CodeRunId}", executionEvent.CodeRunId);

        var executionResult = await _codeExecutionService.ExecuteAsync(executionEvent.Code);

        _logger.LogInformation("Execution result for CodeRunId {CodeRunId}: {ExecutionResult}",
            executionEvent.CodeRunId, executionResult);

        var eventResultToPublish = new CodeRunExecutionResultEvent
        {
            CodeRunId = executionEvent.CodeRunId,
            IsSuccess = executionResult
        };

        _logger.LogInformation("Publishing execution result for CodeRunId: {CodeRunId}", executionEvent.CodeRunId);
        await _eventPublisher.PublishAsync(eventResultToPublish, EventQueueList.CodeExecutionQueueResult);
    }

    public override void Dispose()
    {
        _logger.LogInformation("Disposing CodeExecutionRequestConsumer, closing channel and connection.");
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}
