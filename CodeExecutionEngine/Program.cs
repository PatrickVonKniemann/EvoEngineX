using CodeExecutionService;
using Common;
using MongoDB.Bson;
using MongoDB.Driver;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

// Environment-specific configuration
var environment = builder.Environment.EnvironmentName;

// Configure logging explicitly based on environment
ConfigureLogging(builder.Logging, environment);

// Add services to the container
ConfigureServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline
ConfigureMiddleware(app);

await app.RunAsync();

// --------------------------
// Application methods
// --------------------------

void ConfigureLogging(ILoggingBuilder loggingBuilder, string profileEnvironment)
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddConsole();

    if (profileEnvironment == "Development")
    {
        loggingBuilder.AddDebug(); // Add debug-level logging for development
    }

    loggingBuilder.AddFilter("Microsoft", LogLevel.Warning)
                  .AddFilter("System", LogLevel.Error);
}

void ConfigureServices(IServiceCollection services)
{
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    // RabbitMQ settings from environment variables
    var rabbitMqSettings = GetRabbitMqSettings();
    services.AddSingleton<IConnectionFactory, ConnectionFactory>(sp =>
        new ConnectionFactory
        {
            HostName = rabbitMqSettings.Host,
            UserName = rabbitMqSettings.User,
            Password = rabbitMqSettings.Pass,
            Port = rabbitMqSettings.Port
        });

    services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();

    // MongoDB configuration
    var mongoDatabaseName = GetMongoDatabaseName();
    services.AddSingleton<IMongoClient, MongoClient>(sp =>
    {
        return CreateMongoClient(mongoDatabaseName);
    });

    services.AddSingleton(sp =>
    {
        var client = sp.GetRequiredService<IMongoClient>();
        return client.GetDatabase(mongoDatabaseName);
    });

    services.AddSingleton<ICodeExecutionLogic, CodeExecutionLogic>();
    services.AddHostedService<CodeExecutionRequestConsumer>();
}

(string Host, string User, string Pass, int Port) GetRabbitMqSettings()
{
    return (
        Host: Environment.GetEnvironmentVariable("RABBITMQ_HOST") ?? "localhost",
        User: Environment.GetEnvironmentVariable("RABBITMQ_USER") ?? "kolenpat",
        Pass: Environment.GetEnvironmentVariable("RABBITMQ_PASS") ?? "sa",
        Port: int.Parse(Environment.GetEnvironmentVariable("RABBITMQ_PORT") ?? "5672")
    );
}

MongoClient CreateMongoClient(string mongoDatabaseName)
{
    var mongoConnectionString = $"mongodb://{Environment.GetEnvironmentVariable("MONGO_INITDB_ROOT_USERNAME") ?? "kolenpat"}:{Environment.GetEnvironmentVariable("MONGO_INITDB_ROOT_PASSWORD") ?? "sa"}@{Environment.GetEnvironmentVariable("MONGO_HOST") ?? "mongo"}:{Environment.GetEnvironmentVariable("MONGO_PORT") ?? "27017"}/{mongoDatabaseName}";

    var mongoClient = new MongoClient(mongoConnectionString);

    // Try to ping the MongoDB server to check the connection
    try
    {
        var database = mongoClient.GetDatabase(mongoDatabaseName);
        var pingResult = database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Result;

        if (pingResult != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Successfully connected to MongoDB at {mongoConnectionString}");
            Console.ResetColor();
        }
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Failed to connect to MongoDB at {mongoConnectionString}: " + ex.Message);
        Console.ResetColor();
        throw; // Optionally, you can decide if you want to stop the app if MongoDB is not reachable
    }

    return mongoClient;
}

string GetMongoDatabaseName()
{
    return Environment.GetEnvironmentVariable("MONGO_DB") ?? "evoenginex_db";
}

void ConfigureMiddleware(WebApplication appRuntime)
{
    if (appRuntime.Environment.IsDevelopment())
    {
        appRuntime.UseSwagger();
        appRuntime.UseSwaggerUI();
    }

    appRuntime.UseHttpsRedirection();
}
