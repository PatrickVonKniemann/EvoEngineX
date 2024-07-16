using DomainEntities.CodeRunDto.Query;
using FastEndpoints;
using CodeRunService.Application.Services;

namespace CodeRunService.Api;

public class ReadCodeRunListEndpoint(ILogger<ReadCodeRunListEndpoint> logger, ICodeRunQueryService codeRunQueryService)
    : Endpoint<ReadCodeRunListRequest, ReadCodeRunListResponse>
{
    private new ILogger<ReadCodeRunListEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("code-run");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadCodeRunListRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(ReadCodeRunListEndpoint));
        
        if (req.PaginationQuery == null)
        {
            ThrowError("PaginationQuery is invalid or null");
            return;
        }

        var response = codeRunQueryService.GetAllAsync(req.PaginationQuery);
        await SendAsync(await response, cancellation: ct);
    }
}