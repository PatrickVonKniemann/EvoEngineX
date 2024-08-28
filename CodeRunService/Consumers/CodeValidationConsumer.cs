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
    private readonly ICodeValidationService _codeValidationService;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    

    public CodeValidationConsumer(IConnectionFactory connectionFactory, ICodeValidationService codeValidationService)
    {
        _codeValidationService = codeValidationService;
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel.QueueDeclare(queue: EventQueueList.CodeValidationQueueResult, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var validationEvent = JsonSerializer.Deserialize<CodeRunValidationResultEvent>(message);
            await _codeValidationService.HandleValidationResultAsync(validationEvent);
        };

        _channel.BasicConsume(queue: EventQueueList.CodeValidationQueueResult, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}