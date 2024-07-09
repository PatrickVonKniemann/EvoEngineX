using DomainEntities.Users;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using UsersService.Database;
using UsersService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddFastEndpoints();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocument(settings =>
{
    settings.Title = "My API";
    settings.Version = "v1";
});

builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddScoped<IUserQueryService, UserQueryService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddAutoMapper(cg => cg.AddProfile(new UserProfile()));

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseFastEndpoints();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi();
    app.UseSwaggerGen();
}

app.UseHttpsRedirection();

await app.RunAsync();