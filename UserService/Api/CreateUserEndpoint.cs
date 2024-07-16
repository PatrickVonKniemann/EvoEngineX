using DomainEntities.UserDto.Command;
using FastEndpoints;
using UserService.Services;
using UserService.Validators;

namespace UserService.Api;

public class CreateUserEndpoint(ILogger<CreateUserEndpoint> logger, IUserCommandService userCommandService)
    : Endpoint<CreateUserRequest, CreateUserResponse>
{
    private new ILogger<CreateUserEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("users/add");
        AllowAnonymous();
        Validator<CreateUserRequestValidator>();
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(CreateUserEndpoint));
        var createUserResponse = userCommandService.AddAsync(req);
        await SendAsync(await createUserResponse, cancellation: ct);
    }
}