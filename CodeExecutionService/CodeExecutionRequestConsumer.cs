using System.Text;
using System.Text.Json;
using Common;
using ExternalDomainEntities;
using ExternalDomainEntities.CodeRunDto.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CodeExecutionService;

public class CodeExecutionRequestConsumer : BackgroundService
{
    private readonly ICodeExecutionLogic _codeExecutionService;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    private readonly IEventPublisher _eventPublisher;

    public CodeExecutionRequestConsumer(IConnectionFactory connectionFactory, ICodeExecutionLogic codeExecutionService, IEventPublisher eventPublisher)
    {
        _codeExecutionService = codeExecutionService;
        _eventPublisher = eventPublisher;
        _connection = connectionFactory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel.QueueDeclare(
            queue: EventQueueList.CodeExecutionQueue,
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
            var validationEvent = JsonSerializer.Deserialize<CodeExecutionExecutionRequestEvent>(message);
            

            if (validationEvent != null)
            {
                var executionResult = await _codeExecutionService.ExecuteAsync(validationEvent.Code);
                
                // Create event that will start the execution in the execution service
                var eventResultToPublish = new CodeRunExecutionResultEvent()
                {
                    CodeRunId = validationEvent.CodeRunId,
                    IsSuccess = executionResult
                };

                await _eventPublisher.PublishAsync(eventResultToPublish, EventQueueList.CodeExecutionQueueResult);
            }
        };

        _channel.BasicConsume(queue: EventQueueList.CodeExecutionQueue, autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    public override void Dispose()
    {
        _channel.Close();
        _connection.Close();
        base.Dispose();
    }
}