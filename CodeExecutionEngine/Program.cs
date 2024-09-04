using CodeExecutionService;
using Common;
using MongoDB.Driver;
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

// Get MongoDB connection settings from environment variables
var mongoConnectionString = $"mongodb://{Environment.GetEnvironmentVariable("MONGO_INITDB_ROOT_USERNAME") ?? "kolenpat"}:{Environment.GetEnvironmentVariable("MONGO_INITDB_ROOT_PASSWORD") ?? "sa"}@{Environment.GetEnvironmentVariable("MONGO_HOST") ?? "mongo"}:{Environment.GetEnvironmentVariable("MONGO_PORT") ?? "27017"}/{Environment.GetEnvironmentVariable("MONGO_DB") ?? "evoenginex_db"}";
var mongoDatabaseName = Environment.GetEnvironmentVariable("MONGO_DB") ?? "evoenginex_db";
Console.WriteLine("mongoConnectionString: " + mongoConnectionString);

// Register MongoDB client as a singleton
builder.Services.AddSingleton<IMongoClient, MongoClient>(sp =>
    new MongoClient(mongoConnectionString));


// Register MongoDB database instance
builder.Services.AddSingleton(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoDatabaseName);
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