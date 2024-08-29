using System.Text;
using System.Text.Json;
using CodeRunService.Application.Services;
using Common;
using ExternalDomainEntities.CodeRunDto.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CodeRunService.Consumers;

public class CodeValidationConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<CodeValidationConsumer> _logger;
    private readonly string _queueName = EventQueueList.CodeValidationQueueResult;

    public CodeValidationConsumer(
        IConnectionFactory connectionFactory, 
        IServiceProvider serviceProvider,
        ILogger<CodeValidationConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;

        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();

        InitializeQueue();

        _logger.LogInformation("CodeValidationConsumer created, RabbitMQ connection established.");
    }

    private void InitializeQueue()
    {
        _channel.QueueDeclare(
            queue: _queueName, 
            durable: true, 
            exclusive: false,
            autoDelete: false, 
            arguments: null);

        _logger.LogInformation("Queue {QueueName} declared.", _queueName);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting CodeValidationConsumer execution...");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) => await HandleMessageAsync(ea);

        _channel.BasicConsume(
            queue: _queueName, 
            autoAck: true, 
            consumer: consumer);

        _logger.LogInformation("CodeValidationConsumer is now consuming messages.");

        return Task.CompletedTask;
    }

    private async Task HandleMessageAsync(BasicDeliverEventArgs eventArgs)
    {
        _logger.LogInformation("Message received from queue: {QueueName}", _queueName);

        try
        {
            var body = eventArgs.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogDebug("Message content: {Message}", message);

            var validationEvent = JsonSerializer.Deserialize<CodeRunValidationResultEvent>(message);
            if (validationEvent == null)
            {
                _logger.LogWarning("Failed to deserialize message into CodeRunValidationResultEvent. Message: {Message}", message);
                return;
            }

            _logger.LogInformation("Deserialized message to CodeRunValidationResultEvent.");

            using var scope = _serviceProvider.CreateScope();
            var codeValidationService = scope.ServiceProvider.GetRequiredService<ICodeValidationService>();

            _logger.LogInformation("Handling validation result...");
            await codeValidationService.HandleValidationResultAsync(validationEvent);
        }
        catch (JsonException jsonEx)
        {
            _logger.LogError(jsonEx, "JSON deserialization error occurred while processing the validation message.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while processing the validation message.");
        }
    }

    public override void Dispose()
    {
        _logger.LogInformation("Disposing CodeValidationConsumer, closing channel and connection.");
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}
