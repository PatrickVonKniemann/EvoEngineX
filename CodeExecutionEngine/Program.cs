using CodeExecutionService;
using Common;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Get RabbitMQ settings from environment variables
var rabbitMqHost = Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost";
var rabbitMqUser = Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "kolenpat";
var rabbitMqPass = Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "sa";
var rabbitMqPort = Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672";

builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>(sp =>
    new ConnectionFactory
    {
        HostName = rabbitMqHost,
        UserName = rabbitMqUser,
        Password = rabbitMqPass,
        Port = int.Parse(rabbitMqPort)
    });

builder.Services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
builder.Services.AddSingleton<ICodeExecutionLogic, CodeExecutionLogic>();
builder.Services.AddHostedService<CodeExecutionRequestConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

await app.RunAsync();