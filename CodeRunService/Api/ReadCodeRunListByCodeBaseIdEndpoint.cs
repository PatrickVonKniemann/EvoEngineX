using CodeRunService.Application.Services;
using ExternalDomainEntities.CodeRunDto.Query;
using FastEndpoints;

namespace CodeRunService.Api;

public class ReadCodeRunListByCodeBaseIdEndpoint(ILogger<ReadCodeRunListByCodeBaseIdEndpoint> logger, ICodeRunQueryService codeRunQueryService)
    : Endpoint<ReadCodeRunListByCodeBaseIdRequest, ReadCodeRunListByCodeBaseIdResponse>
{
    private new ILogger<ReadCodeRunListByCodeBaseIdEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("code-run/by-code-base-id/{CodeBaseId}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadCodeRunListByCodeBaseIdRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(ReadCodeRunListByCodeBaseIdEndpoint));
        var response = codeRunQueryService.GetAllByCodeBaseIdAsync(req.CodeBaseId, req.PaginationQuery);
        await SendAsync(await response, cancellation: ct);
    }
}