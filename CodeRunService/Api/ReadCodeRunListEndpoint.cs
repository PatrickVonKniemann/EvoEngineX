using FastEndpoints;
using CodeRunService.Application.Services;
using ExternalDomainEntities.CodeRunDto.Query;

namespace CodeRunService.Api;

public class ReadCodeRunListEndpoint(ILogger<ReadCodeRunListEndpoint> logger, ICodeRunQueryService codeRunQueryService)
    : Endpoint<ReadCodeRunListRequest, ReadCodeRunListResponse>
{
    private new ILogger<ReadCodeRunListEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("code-run/all");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadCodeRunListRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(ReadCodeRunListEndpoint));
        var response = codeRunQueryService.GetAllAsync(req.PaginationQuery);
        await SendAsync(await response, cancellation: ct);
    }
}