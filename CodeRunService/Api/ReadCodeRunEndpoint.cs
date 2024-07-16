using DomainEntities.CodeRunDto.Query;
using FastEndpoints;
using CodeRunService.Application.Services;

namespace CodeRunService.Api;

public class ReadCodeRunEndpoint(ILogger<ReadCodeRunEndpoint> logger, ICodeRunQueryService codeRunQueryService)
    : Endpoint<ReadCodeRunRequest, ReadCodeRunResponse>
{
    private new ILogger<ReadCodeRunEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("code-runs/{ID}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadCodeRunRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(ReadCodeRunEndpoint));
        var response = codeRunQueryService.GetByIdAsync(req.Id);
        await SendAsync(await response, cancellation: ct);
    }
}