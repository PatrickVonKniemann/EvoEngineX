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

    public CodeValidationRequestConsumer(IServiceProvider serviceProvider, IConnectionFactory connectionFactory,
        ICodeValidationService codeValidationService, IEventPublisher eventPublisher)
    {
        _codeValidationService = codeValidationService;
        _eventPublisher = eventPublisher;
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
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
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var validationEvent = JsonSerializer.Deserialize<CodeFormaterValidationRequestEvent>(message);

            if (validationEvent != null)
            {
                var validationResult = await _codeValidationService.ValidateAsync(validationEvent.Code);

                // Create event that will start the execution in the execution service
                var eventResultToPublish = new CodeRunValidationResultEvent
                {
                    CodeRunId = validationEvent.CodeRunId,
                    IsValid = validationResult
                };

                await _eventPublisher.PublishAsync(eventResultToPublish, EventQueueList.CodeValidationQueueResult);
            }
        };

        _channel.BasicConsume(queue: EventQueueList.CodeValidationQueue, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}