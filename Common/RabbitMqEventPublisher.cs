using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Common;

public class RabbitMqEventPublisher(IConnectionFactory connectionFactory, ILogger<RabbitMqEventPublisher> logger)
    : IEventPublisher
{
    public async Task PublishAsync<TEvent>(TEvent eventMessage, string eventQueue) where TEvent : class
    {
        using var connection = connectionFactory.CreateConnection();
        using var channel = connection.CreateModel();

        var eventName = typeof(TEvent).Name;

        channel.QueueDeclare(
            queue: eventQueue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null
        );

        var message = JsonSerializer.Serialize(eventMessage);
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(
            exchange: "",
            routingKey: eventQueue,
            basicProperties: null,
            body: body
        );

        logger.LogInformation($"Published event: {eventName}");

        await Task.CompletedTask;
    }
}