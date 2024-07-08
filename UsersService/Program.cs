using DomainEntities.Users;
using FastEndpoints;
using UsersService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IUserCommandService, UserCommandService>();
builder.Services.AddSwaggerGen();
builder.Services.AddFastEndpoints();
builder.Services.AddAutoMapper(cg => cg.AddProfile(new UserProfile()));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseFastEndpoints();

await app.RunAsync();