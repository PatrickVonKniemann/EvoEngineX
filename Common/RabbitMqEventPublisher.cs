using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Common;

public class RabbitMqEventPublisher(IConnectionFactory connectionFactory, ILogger<RabbitMqEventPublisher> logger)
    : IEventPublisher
{
    private readonly IConnectionFactory _connectionFactory = connectionFactory;
    private readonly ILogger<RabbitMqEventPublisher> _logger = logger;

    public async Task PublishAsync<TEvent>(TEvent eventMessage) where TEvent : class
    {
        using var connection = _connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        var eventName = typeof(TEvent).Name;

        channel.QueueDeclare(queue: eventName, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var message = JsonSerializer.Serialize(eventMessage);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: "", routingKey: eventName, basicProperties: null, body: body);

        _logger.LogInformation($"Published event: {eventName}");

        await Task.CompletedTask;
    }
}
