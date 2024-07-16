using CodebaseService.Application.Services;
using CodeBaseService.Application.Services;
using CodebaseService.Infrastructure.Database;
using Common;
using DomainEntities.CodebaseDto;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "Code base Service API";
            s.Version = "v0.0.1";
        };
    });

// Configure logging explicitly
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddLogging(); // Ensure logging is added first

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddScoped<ICodebaseCommandService, CodebaseCommandService>();
builder.Services.AddScoped<ICodebaseQueryService, CodebaseQueryService>();
builder.Services.AddScoped<ICodebaseRepository, CodebaseRepository>();

builder.Services.AddAutoMapper(cg => cg.AddProfile(new CodebaseProfile()));

var app = builder.Build();
app.UseMiddleware<ErrorHandlingMiddleware>(); // Use custom middleware
app.UseFastEndpoints()
    .UseSwaggerGen();

app.UseHttpsRedirection();

await app.RunAsync();

/// <summary>
/// This class is used to start the API,
/// Partial class is used to add the entry point for CustomWebApplicationFactory
/// </summary>
public partial class Program
{
}