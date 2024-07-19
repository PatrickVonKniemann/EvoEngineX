using CodebaseService.Application.Services;
using FastEndpoints;
using DomainEntities.CodeBaseDto.Query;
using ExternalDomainEntities.CodeBaseDto.Query;

namespace CodeBaseService.Api;

public class ReadCodeBaseListEndpoint(ILogger<ReadCodeBaseListEndpoint> logger, ICodeBaseQueryService codeBaseQueryService)
    : Endpoint<ReadCodeBaseListRequest, ReadCodeBaseListResponse>
{
    private new ILogger<ReadCodeBaseListEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("code-base");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadCodeBaseListRequest req, CancellationToken ct)
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