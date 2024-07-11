using DomainEntities.Users.Command;
using DomainEntities.Users.Response;
using FastEndpoints;
using UsersService.Services;

namespace UsersService.Api;

public class UpdateUserEndpoint(ILogger<UpdateUserEndpoint> logger, IUserCommandService userCommandService)
    : Endpoint<UpdateUserRequest, UpdateUserResponse>
{
    private new ILogger<UpdateUserEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.PATCH);
        Routes("users/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(UpdateUserEndpoint));
        var response = userCommandService.UpdateAsync(req.Id, req);
        await SendAsync(await response, cancellation: ct);
    }
}