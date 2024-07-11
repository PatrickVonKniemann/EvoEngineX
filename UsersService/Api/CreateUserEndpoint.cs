using DomainEntities.Users.Command;
using DomainEntities.Users.Response;
using FastEndpoints;
using UsersService.Services;

namespace UsersService.Api;

public class CreateUserEndpoint(ILogger<CreateUserEndpoint> logger, IUserCommandService userCommandService)
    : Endpoint<CreateUserRequest, CreateUserResponse>
{
    private new ILogger<CreateUserEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(CreateUserEndpoint));
        var response = userCommandService.AddAsync(req);
        await SendAsync(await response, cancellation: ct);
    }
}