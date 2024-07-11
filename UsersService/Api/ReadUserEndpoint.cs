using DomainEntities.Users.Query;
using DomainEntities.Users.Response;
using FastEndpoints;
using UsersService.Services;

namespace UsersService.Api;

public class ReadUserEndpoint(ILogger<ReadUserEndpoint> logger, IUserQueryService userQueryService)
    : Endpoint<ReadUserRequest, ReadUserResponse>
{
    private new ILogger<ReadUserEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("user/{ID}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadUserRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(ReadUserEndpoint));
        var response = userQueryService.GetByIdAsync(req.Id);
        await SendAsync(await response, cancellation: ct);
    }
}