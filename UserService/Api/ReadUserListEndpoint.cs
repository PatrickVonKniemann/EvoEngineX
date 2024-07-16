using DomainEntities.UserDto.Query;
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
        Routes("users");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadUserListRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(ReadUserListEndpoint));
        
        if (req.PaginationQuery == null)
        {
            ThrowError("PaginationQuery is invalid or null");
            return;
        }

        var response = userQueryService.GetAllAsync(req.PaginationQuery);
        await SendAsync(await response, cancellation: ct);
    }
}