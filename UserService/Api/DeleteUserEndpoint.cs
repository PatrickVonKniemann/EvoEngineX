using ExternalDomainEntities.UserDto.Command;
using FastEndpoints;
using UserService.Application.Services;

namespace UserService.Api;

public class DeleteUserEndpoint(ILogger<DeleteUserEndpoint> logger, IUserCommandService userCommandService)
    : Endpoint<DeleteUserRequest>
{
    private new ILogger<DeleteUserEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.DELETE);
        Routes("user/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(DeleteUserEndpoint));
        await userCommandService.DeleteAsync(req.Id);
        await SendNoContentAsync(cancellation: ct);
    }
}