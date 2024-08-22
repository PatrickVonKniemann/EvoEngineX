namespace Common;

using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

public class RabbitMqPublisher(string hostname, string queueName)
{
    public void Publish<T>(T message)
    {
        var factory = new ConnectionFactory() { HostName = hostname };
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
        channel.QueueDeclare(queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;

        channel.BasicPublish(exchange: "",
            routingKey: queueName,
            basicProperties: properties,
            body: body);
    }
}
