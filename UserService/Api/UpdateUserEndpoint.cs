using ExternalDomainEntities.UserDto.Command;
using FastEndpoints;
using UserService.Application.Services;

namespace UserService.Api;

public class UpdateUserEndpoint(ILogger<UpdateUserEndpoint> logger, IUserCommandService userCommandService)
    : Endpoint<UpdateUserRequest, UpdateUserResponse>
{
    private new ILogger<UpdateUserEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.PATCH);
        Routes("user/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(UpdateUserEndpoint));
        var updateUserResponse = await userCommandService.UpdateAsync(req.Id, req);
        await SendAsync(updateUserResponse, cancellation: ct);
    }
}