using CodeFormaterService.Consumers;
using CodeFormaterService.Services;
using Common;
using FastEndpoints;
using FastEndpoints.Swagger;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerGen();
// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        corsPolicyBuilder => corsPolicyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});
builder.Services.AddScoped<ICodeValidationService, CodeValidationService>();

// Get RabbitMQ settings from environment variables
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
builder.Services.AddHostedService<CodeValidationRequestConsumer>();

var app = builder.Build();
app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseFastEndpoints()
    .UseSwaggerGen();

await app.RunAsync();