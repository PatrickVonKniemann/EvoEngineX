using System.Text;
using System.Text.Json;
using CodeRunService.Application.Services;
using Common;
using ExternalDomainEntities;
using ExternalDomainEntities.CodeRunDto.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CodeRunService.Consumers;

public class CodeExecutionConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly ILogger<CodeExecutionConsumer> _logger;

    public CodeExecutionConsumer(IServiceProvider serviceProvider, IConnectionFactory connectionFactory,
        ILogger<CodeExecutionConsumer> logger)
    {
        _serviceProvider = serviceProvider;
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
        _logger = logger;
        _logger.LogInformation("CodeExecutionConsumer created, RabbitMQ connection established.");
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting CodeExecutionConsumer execution...");

        _channel.QueueDeclare(queue: EventQueueList.CodeExecutionQueueResult, durable: true, exclusive: false,
            autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            _logger.LogInformation("Message received from queue: {QueueName}", EventQueueList.CodeExecutionQueueResult);

            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                _logger.LogDebug("Message content: {Message}", message);

                var executionEvent = JsonSerializer.Deserialize<CodeRunExecutionResultEvent>(message);
                _logger.LogInformation("Deserialized message to CodeRunExecutionResultEvent.");

                using var scope = _serviceProvider.CreateScope();
                var executionCommandService = scope.ServiceProvider.GetRequiredService<ICodeExecutionCommandService>();

                _logger.LogInformation("Handling execution result...");

                await executionCommandService.HandleExecutionResultAsync(executionEvent);

                _logger.LogInformation("Execution result handled successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the message.");
            }
        };

        _channel.BasicConsume(queue: EventQueueList.CodeExecutionQueueResult, autoAck: true, consumer: consumer);

        _logger.LogInformation("CodeExecutionConsumer is now consuming messages.");

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _logger.LogInformation("Disposing CodeExecutionConsumer, closing channel and connection.");
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}