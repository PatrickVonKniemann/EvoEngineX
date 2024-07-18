using System.Configuration;
using Common;
using DomainEntities.UserDto;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using UserService.Application.Services;
using UserService.Infrastructure;
using UserService.Infrastructure.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "UserService API";
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

builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddAutoMapper(cg => cg.AddProfile(new UserProfile()));

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("UserDatabase")));


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