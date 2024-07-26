using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using CodebaseService.Application.Services;
using ExternalDomainEntities.CodeBaseDto.Query;
using Microsoft.Extensions.Logging;

namespace CodebaseService.Api;

public class ReadCodeBaseListByUserId(ILogger<ReadCodeBaseListByUserId> logger, ICodeBaseQueryService codeBaseQueryService)
    : Endpoint<ReadCodeBaseListByUserIdRequest, ReadCodeBaseListByUserIdResponse>
{
    private new ILogger<ReadCodeBaseListByUserId> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("code-base/by-user-id/{UserID}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadCodeBaseListByUserIdRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(ReadCodeBaseListByUserId));
        var response = codeBaseQueryService.GetAllByUserIdAsync(req.UserId);
        await SendAsync(await response, cancellation: ct);
    }
}