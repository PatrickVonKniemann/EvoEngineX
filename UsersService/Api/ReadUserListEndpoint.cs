using DomainEntities.Users.Query;
using DomainEntities.Users.Response;
using FastEndpoints;
using Generics.Pagination;
using UsersService.Services;

namespace UsersService.Api;

public class ReadUserListEndpoint : Endpoint<ReadUserListRequest, ReadUserListResponse>
{
    public new ILogger<ReadUserListEndpoint> Logger { get; }
    private readonly IUserQueryService _userQueryService;

    public ReadUserListEndpoint(ILogger<ReadUserListEndpoint> logger, IUserQueryService userQueryService)
    {
        Logger = logger;
        _userQueryService = userQueryService;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
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

        var response = _userQueryService.GetAll(req.PaginationQuery);

        await SendAsync(response);
    }
}