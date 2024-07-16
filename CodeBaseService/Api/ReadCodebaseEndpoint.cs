using FastEndpoints;
using CodebaseService.Application.Services;
using DomainEntities.CodeBaseDto.Query;

namespace CodebaseService.Api;

public class ReadCodebaseEndpoint(ILogger<ReadCodebaseEndpoint> logger, ICodebaseQueryService codebaseQueryService)
    : Endpoint<ReadCodebaseRequest, ReadCodebaseResponse>
{
    private new ILogger<ReadCodebaseEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("codebases/{ID}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadCodebaseRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(ReadCodebaseEndpoint));
        var response = codebaseQueryService.GetByIdAsync(req.Id);
        await SendAsync(await response, cancellation: ct);
    }
}