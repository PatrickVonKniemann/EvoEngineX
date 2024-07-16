using FastEndpoints;
using CodebaseService.Application.Services;
using DomainEntities.CodeBaseDto.Command;

namespace CodebaseService.Api;

public class DeleteCodebaseEndpoint(ILogger<DeleteCodebaseEndpoint> logger, ICodebaseCommandService codebaseCommandService)
    : Endpoint<DeleteCodebaseRequest>
{
    private new ILogger<DeleteCodebaseEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.DELETE);
        Routes("codebases/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteCodebaseRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(DeleteCodebaseEndpoint));
        await codebaseCommandService.DeleteAsync(req.Id);
        await SendNoContentAsync(cancellation: ct);
    }
}