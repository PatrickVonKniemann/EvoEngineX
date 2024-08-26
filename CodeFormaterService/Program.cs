using CodeFormaterService.Consumers;
using CodeFormaterService.Services;
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
builder.Services.AddSingleton<IConnectionFactory, ConnectionFactory>(sp => new ConnectionFactory { HostName = "localhost", UserName = "kolenpat", Password = "sa"});

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
