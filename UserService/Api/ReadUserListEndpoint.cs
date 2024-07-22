using DomainEntities.UserDto.Query;
using ExternalDomainEntities.UserDto.Query;
using FastEndpoints;
using UserService.Application.Services;

namespace UserService.Api;

public class ReadUserListEndpoint(ILogger<ReadUserListEndpoint> logger, IUserQueryService userQueryService)
    : Endpoint<ReadUserListRequest, ReadUserListResponse>
{
    private new ILogger<ReadUserListEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("user/all");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadUserListRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(ReadUserListEndpoint));
        var response = userQueryService.GetAllAsync(req.PaginationQuery);
        await SendAsync(await response, cancellation: ct);
    }
}