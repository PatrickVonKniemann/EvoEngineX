using DomainEntities.Users.Command;
using DomainEntities.Users.Response;
using FastEndpoints;
using UsersService.Services;

namespace UsersService.Api;

public class CreateUserEndpoint : Endpoint<CreateUserRequest, CreateUserResponse>
{
    public new ILogger<CreateUserEndpoint> Logger { get; }
    private readonly IUserCommandService _userCommandService;

    public CreateUserEndpoint(ILogger<CreateUserEndpoint> logger, IUserCommandService userCommandService)
    {
        Logger = logger;
        _userCommandService = userCommandService;
    }

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("user");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(CreateUserEndpoint));
        var response = _userCommandService.Add(req);

        await SendAsync(response);
    }
}