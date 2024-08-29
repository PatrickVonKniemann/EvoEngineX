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

    public CodeValidationConsumer(IConnectionFactory connectionFactory, IServiceProvider serviceProvider,
        ILogger<CodeValidationConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _logger = logger;

        _logger.LogInformation("CodeValidationConsumer created, RabbitMQ connection established.");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting CodeValidationConsumer execution...");

        _channel.QueueDeclare(queue: EventQueueList.CodeValidationQueueResult, durable: true, exclusive: false,
            autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            _logger.LogInformation("Message received from queue: {QueueName}",
                EventQueueList.CodeValidationQueueResult);

            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogDebug("Message content: {Message}", message);

                var validationEvent = JsonSerializer.Deserialize<CodeRunValidationResultEvent>(message);
                _logger.LogInformation("Deserialized message to CodeRunValidationResultEvent.");

                using var scope = _serviceProvider.CreateScope();
                var codeValidationService = scope.ServiceProvider.GetRequiredService<ICodeValidationService>();

                _logger.LogInformation("Handling validation result...");

                await codeValidationService.HandleValidationResultAsync(validationEvent);

                _logger.LogInformation("Validation result handled successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the validation message.");
            }
        };

        _channel.BasicConsume(queue: EventQueueList.CodeValidationQueueResult, autoAck: true, consumer: consumer);

        _logger.LogInformation("CodeValidationConsumer is now consuming messages.");

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _logger.LogInformation("Disposing CodeValidationConsumer, closing channel and connection.");
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}