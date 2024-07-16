using FastEndpoints;
using CodebaseService.Application.Services;
using DomainEntities.CodeBaseDto.Query;

namespace CodebaseService.Api;

public class ReadCodebaseListEndpoint(ILogger<ReadCodebaseListEndpoint> logger, ICodebaseQueryService codebaseQueryService)
    : Endpoint<ReadCodebaseListRequest, ReadCodebaseListResponse>
{
    private new ILogger<ReadCodebaseListEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("codebases");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadCodebaseListRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(ReadCodebaseListEndpoint));
        
        if (req.PaginationQuery == null)
        {
            ThrowError("PaginationQuery is invalid or null");
            return;
        }

        var response = codebaseQueryService.GetAllAsync(req.PaginationQuery);
        await SendAsync(await response, cancellation: ct);
    }
}