using System.Text;
using System.Text.Json;
using Common;
using ExternalDomainEntities.CodeRunDto.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CodeExecutionService;

public class CodeExecutionConsumer(IEventPublisher eventPublisher, ILogger<CodeExecutionConsumer> logger)
{
    private readonly IEventPublisher _eventPublisher = eventPublisher;

    public void StartListening()
    {
        var factory = new ConnectionFactory() { HostName = "localhost", UserName = "kolenpat", Password = "sa"};
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: nameof(CodeRunExecutionRequestedEvent), durable: true, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var executionRequest = JsonSerializer.Deserialize<CodeRunExecutionRequestedEvent>(message);

            if (executionRequest != null)
            {
                await ExecuteCodeAsync(executionRequest);
            }
        };

        channel.BasicConsume(queue: nameof(CodeRunExecutionRequestedEvent), autoAck: true, consumer: consumer);
        logger.LogInformation("Listening for CodeRunExecutionRequestedEvent...");
    }

    private async Task ExecuteCodeAsync(CodeRunExecutionRequestedEvent executionRequest)
    {
        // Simulate code execution with a 3-second delay
        await Task.Delay(3000);

        // Randomly decide whether the execution was successful or not
        var random = new Random();
        bool executionSuccess = random.Next(0, 2) == 0; // 50/50 chance

        var executionResultEvent = new CodeRunExecutionResultEvent
        {
            CodeRunId = executionRequest.CodeRunId,
            IsSuccess = executionSuccess
        };

        // Publish the result event
        await _eventPublisher.PublishAsync(executionResultEvent);

        logger.LogInformation($"Execution completed for CodeRunId: {executionRequest.CodeRunId}, Success: {executionSuccess}");
    }
}