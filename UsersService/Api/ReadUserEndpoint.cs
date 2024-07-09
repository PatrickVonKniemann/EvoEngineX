using DomainEntities.Users.Query;
using DomainEntities.Users.Response;
using FastEndpoints;
using UsersService.Services;

namespace UsersService.Api;

public class ReadUserEndpoint : Endpoint<ReadUserRequest, ReadUserResponse>
{
    public new ILogger<ReadUserEndpoint> Logger { get; }
    private readonly IUserQueryService _userQueryService;

    public ReadUserEndpoint(ILogger<ReadUserEndpoint> logger, IUserQueryService userQueryService)
    {
        Logger = logger;
        _userQueryService = userQueryService;
    }

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("user/{ID}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadUserRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(ReadUserEndpoint));
        var response = _userQueryService.GetById(req.Id);

        await SendAsync(response);
    }
}