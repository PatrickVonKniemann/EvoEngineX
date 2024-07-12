using DomainEntities.Users.Command;
using FastEndpoints;
using UsersService.Services;

namespace UsersService.Api;

public class DeleteUserEndpoint(ILogger<DeleteUserEndpoint> logger, IUserCommandService userCommandService)
    : Endpoint<DeleteUserRequest>
{
    private new ILogger<DeleteUserEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.DELETE);
        Routes("users/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(DeleteUserEndpoint));
        await userCommandService.DeleteAsync(req.Id);
        await SendNoContentAsync(cancellation: ct);
    }
}