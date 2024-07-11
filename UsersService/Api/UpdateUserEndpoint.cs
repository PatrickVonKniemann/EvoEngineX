using DomainEntities.Users.Command;
using DomainEntities.Users.Response;
using FastEndpoints;
using UsersService.Services;

namespace UsersService.Api;

public class UpdateUserEndpoint : Endpoint<UpdateUserRequest, UpdateUserResponse>
{
    public new ILogger<UpdateUserEndpoint> Logger { get; }
    private readonly IUserCommandService _userCommandService;

    public UpdateUserEndpoint(ILogger<UpdateUserEndpoint> logger, IUserCommandService userCommandService)
    {
        Logger = logger;
        _userCommandService = userCommandService;
    }

    public override void Configure()
    {
        Verbs(Http.PATCH);
        Routes("users/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(UpdateUserEndpoint));
        var response = _userCommandService.Update(req.Id, req);

        await SendAsync(response);
    }
}