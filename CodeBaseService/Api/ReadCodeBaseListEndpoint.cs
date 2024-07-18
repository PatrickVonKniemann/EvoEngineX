using FastEndpoints;
using CodebaseService.Application.Services;
using DomainEntities.CodeBaseDto.Query;

namespace CodebaseService.Api;

public class ReadCodeBaseListEndpoint(ILogger<ReadCodeBaseListEndpoint> logger, ICodeBaseQueryService codeBaseQueryService)
    : Endpoint<ReadCodebaseListRequest, ReadCodebaseListResponse>
{
    private new ILogger<ReadCodeBaseListEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("code-base");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadCodebaseListRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(ReadCodeBaseListEndpoint));
        
        if (req.PaginationQuery == null)
        {
            ThrowError("PaginationQuery is invalid or null");
            return;
        }

        var response = codeBaseQueryService.GetAllAsync(req.PaginationQuery);
        await SendAsync(await response, cancellation: ct);
    }
}