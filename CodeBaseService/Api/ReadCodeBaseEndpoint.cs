using FastEndpoints;
using CodebaseService.Application.Services;
using DomainEntities.CodeBaseDto.Query;

namespace CodebaseService.Api;

public class ReadCodeBaseEndpoint(ILogger<ReadCodeBaseEndpoint> logger, ICodeBaseQueryService codeBaseQueryService)
    : Endpoint<ReadCodebaseRequest, ReadCodebaseResponse>
{
    private new ILogger<ReadCodeBaseEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("code-base/{ID}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadCodebaseRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(ReadCodeBaseEndpoint));
        var response = codeBaseQueryService.GetByIdAsync(req.Id);
        await SendAsync(await response, cancellation: ct);
    }
}