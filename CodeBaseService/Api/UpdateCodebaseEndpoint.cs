using FastEndpoints;
using CodebaseService.Application.Services;
using DomainEntities.CodeBaseDto.Command;

namespace CodebaseService.Api;

public class UpdateCodebaseEndpoint(ILogger<UpdateCodebaseEndpoint> logger, ICodebaseCommandService codebaseCommandService)
    : Endpoint<UpdateCodebaseRequest, UpdateCodebaseResponse>
{
    private new ILogger<UpdateCodebaseEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.PATCH);
        Routes("codebase/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateCodebaseRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(UpdateCodebaseEndpoint));
        var updateCodebaseResponse = codebaseCommandService.UpdateAsync(req.Id, req);
        await SendAsync(await updateCodebaseResponse, cancellation: ct);
    }
}