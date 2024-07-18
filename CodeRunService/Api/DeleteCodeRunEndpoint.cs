using CodeRunService.Application.Services;
using ExternalDomainEntities.CodeRunDto.Command;
using FastEndpoints;

namespace CodeRunService.Api;

public class DeleteCodeRunEndpoint(ILogger<DeleteCodeRunEndpoint> logger, ICodeRunCommandService codeRunCommandService)
    : Endpoint<DeleteCodeRunRequest>
{
    private new ILogger<DeleteCodeRunEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.DELETE);
        Routes("code-run/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteCodeRunRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(DeleteCodeRunEndpoint));
        await codeRunCommandService.DeleteAsync(req.Id);
        await SendNoContentAsync(cancellation: ct);
    }
}