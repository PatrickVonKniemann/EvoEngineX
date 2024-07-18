using FastEndpoints;
using CodeRunService.Application.Services;
using ExternalDomainEntities.CodeRunDto.Command;

namespace CodeRunService.Api;

public class UpdateCodeRunEndpoint(ILogger<UpdateCodeRunEndpoint> logger, ICodeRunCommandService codeRunCommandService)
    : Endpoint<UpdateCodeRunRequest, UpdateCodeRunResponse>
{
    private new ILogger<UpdateCodeRunEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.PATCH);
        Routes("code-run/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateCodeRunRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(UpdateCodeRunEndpoint));
        var updateCodeRunResponse = codeRunCommandService.UpdateAsync(req.Id, req);
        await SendAsync(await updateCodeRunResponse, cancellation: ct);
    }
}