using DomainEntities.UserDto.Query;
using FastEndpoints;
using UserService.Application.Services;

namespace UserService.Api;

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