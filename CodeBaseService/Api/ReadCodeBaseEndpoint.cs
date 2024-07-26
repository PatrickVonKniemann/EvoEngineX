using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using CodebaseService.Application.Services;
using ExternalDomainEntities.CodeBaseDto.Query;
using Microsoft.Extensions.Logging;

namespace CodebaseService.Api;

public class ReadCodeBaseEndpoint(ILogger<ReadCodeBaseEndpoint> logger, ICodeBaseQueryService codeBaseQueryService)
    : Endpoint<ReadCodeBaseRequest, ReadCodeBaseResponse>
{
    private new ILogger<ReadCodeBaseEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("code-base/{ID}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadCodeBaseRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(ReadCodeBaseEndpoint));
        var response = codeBaseQueryService.GetByIdAsync(req.Id);
        await SendAsync(await response, cancellation: ct);
    }
}