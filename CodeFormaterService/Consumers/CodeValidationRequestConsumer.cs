using System.Text;
using System.Text.Json;
using CodeFormaterService.Services;
using ExternalDomainEntities;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CodeFormaterService.Consumers;

public class CodeValidationRequestConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public CodeValidationRequestConsumer(IServiceProvider serviceProvider, IConnectionFactory connectionFactory)
    {
        _serviceProvider = serviceProvider;
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel.QueueDeclare(
            queue: "CodeValidationRequestQueue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var validationEvent = JsonSerializer.Deserialize<CodeFormaterValidationRequestEvent>(message);

            using var scope = _serviceProvider.CreateScope();
            var codeValidationService = scope.ServiceProvider.GetRequiredService<CodeValidationService>();
            
            if (validationEvent != null)
            {
                await codeValidationService.ValidateAsync(validationEvent.Code);
            }
        };

        _channel.BasicConsume(queue: "CodeValidationRequestQueue", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}