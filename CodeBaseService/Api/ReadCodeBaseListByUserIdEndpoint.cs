using FastEndpoints;
using CodebaseService.Application.Services;
using ExternalDomainEntities.CodeBaseDto.Query;

namespace CodebaseService.Api;

public class ReadCodeBaseListByUserId(ILogger<ReadCodeBaseListByUserId> logger, ICodeBaseQueryService codeBaseQueryService)
    : Endpoint<ReadCodeBaseListByUserIdRequest, ReadCodeBaseListByUserIdResponse>
{
    private new ILogger<ReadCodeBaseListByUserId> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("code-base/by-user-id/{UserID}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadCodeBaseListByUserIdRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(ReadCodeBaseListByUserId));
        var response = codeBaseQueryService.GetAllByUserIdAsync(req.UserId, req.PaginationQuery);
        await SendAsync(await response, cancellation: ct);
    }
}