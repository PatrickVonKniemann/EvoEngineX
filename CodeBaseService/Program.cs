using CodebaseService.Application.Services;
using CodeBaseService.Application.Services;
using CodeBaseService.Infrastructure;
using CodebaseService.Infrastructure.Database;
using Common;
using DomainEntities;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "Code Base Service API";
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

builder.Services.AddScoped<ICodeBaseCommandService, CodeBaseCommandService>();
builder.Services.AddScoped<ICodeBaseQueryService, CodeBaseQueryService>();
builder.Services.AddScoped<ICodeBaseRepository, CodeBaseRepository>();

builder.Services.AddAutoMapper(cg => cg.AddProfile(new CodebaseProfile()));
builder.Services.AddDbContext<CodeBaseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("CodeBaseDatabase")));


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