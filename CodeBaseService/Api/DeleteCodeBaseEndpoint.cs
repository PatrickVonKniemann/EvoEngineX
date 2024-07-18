using FastEndpoints;
using CodebaseService.Application.Services;
using ExternalDomainEntities.CodeBaseDto.Command;

namespace CodebaseService.Api;

public class DeleteCodeBaseEndpoint(ILogger<DeleteCodeBaseEndpoint> logger, ICodeBaseCommandService codeBaseCommandService)
    : Endpoint<DeleteCodeBaseRequest>
{
    private new ILogger<DeleteCodeBaseEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.DELETE);
        Routes("code-base/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteCodeBaseRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(DeleteCodeBaseEndpoint));
        await codeBaseCommandService.DeleteAsync(req.Id);
        await SendNoContentAsync(cancellation: ct);
    }
}