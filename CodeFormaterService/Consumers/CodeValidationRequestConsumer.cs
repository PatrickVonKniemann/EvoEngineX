using System.Text;
using System.Text.Json;
using CodeFormaterService.Services;
using Common;
using ExternalDomainEntities;
using ExternalDomainEntities.CodeRunDto.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CodeFormaterService.Consumers;

public class CodeValidationRequestConsumer : BackgroundService
{
    private readonly ICodeValidationService _codeValidationService;
    private readonly IEventPublisher _eventPublisher;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<CodeValidationRequestConsumer> _logger;

    public CodeValidationRequestConsumer(IServiceProvider serviceProvider, IConnectionFactory connectionFactory,
        ICodeValidationService codeValidationService, IEventPublisher eventPublisher,
        ILogger<CodeValidationRequestConsumer> logger)
    {
        _codeValidationService = codeValidationService;
        _eventPublisher = eventPublisher;
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _logger = logger;

        _logger.LogInformation("CodeValidationRequestConsumer created, RabbitMQ connection established.");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting CodeValidationRequestConsumer execution...");

        _channel.QueueDeclare(
            queue: EventQueueList.CodeValidationQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            _logger.LogInformation("Message received from queue: {QueueName}", EventQueueList.CodeValidationQueue);

            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogDebug("Message content: {Message}", message);

                var validationEvent = JsonSerializer.Deserialize<CodeFormaterValidationRequestEvent>(message);
                if (validationEvent != null)
                {
                    _logger.LogInformation("Validating code for CodeRunId: {CodeRunId}", validationEvent.CodeRunId);

                    var validationResult = await _codeValidationService.ValidateAsync(validationEvent.Code);
                    if (validationResult)
                    {
                        _logger.LogInformation("Validation result for CodeRunId {CodeRunId}: {ValidationResult}",
                            validationEvent.CodeRunId, validationResult);
                    }
                    else
                    {
                        _logger.LogWarning(
                            "Validation result for CodeRunId {CodeRunId}: {ValidationResult} validation failed",
                            validationEvent.CodeRunId, validationResult);
                    }

                    // Create event that will start the execution in the execution service
                    var eventResultToPublish = new CodeRunValidationResultEvent
                    {
                        CodeRunId = validationEvent.CodeRunId,
                        IsValid = validationResult
                    };

                    _logger.LogInformation("Publishing validation result for CodeRunId: {CodeRunId}",
                        validationEvent.CodeRunId);
                    await _eventPublisher.PublishAsync(eventResultToPublish, EventQueueList.CodeValidationQueueResult);
                }
                else
                {
                    _logger.LogWarning("Received invalid or null validation event.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the message.");
            }
        };

        _channel.BasicConsume(queue: EventQueueList.CodeValidationQueue, autoAck: true, consumer: consumer);

        _logger.LogInformation("CodeValidationRequestConsumer is now consuming messages.");

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _logger.LogInformation("Disposing CodeValidationRequestConsumer, closing channel and connection.");
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}