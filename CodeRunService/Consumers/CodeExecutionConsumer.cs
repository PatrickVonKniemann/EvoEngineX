using System.Text;
using System.Text.Json;
using CodeRunService.Application.Services;
using Common;
using ExternalDomainEntities.CodeRunDto.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CodeRunService.Consumers;

public class CodeExecutionConsumer : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ICodeExecutionService _executionService;
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public CodeExecutionConsumer(IServiceProvider serviceProvider, IConnectionFactory connectionFactory, ICodeExecutionService executionService)
    {
        _serviceProvider = serviceProvider;
        _executionService = executionService;
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel.QueueDeclare(queue: EventQueueList.CodeExecutionQueueResult, durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var executionEvent = JsonSerializer.Deserialize<CodeRunExecutionResultEvent>(message);
            await _executionService.HandleExecutionResultAsync(executionEvent);
        };

        _channel.BasicConsume(queue: EventQueueList.CodeExecutionQueueResult, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}