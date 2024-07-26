using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using CodebaseService.Application.Services;
using ExternalDomainEntities.CodeBaseDto.Command;
using Microsoft.Extensions.Logging;

namespace CodebaseService.Api;

public class UpdateCodeBaseEndpoint(ILogger<UpdateCodeBaseEndpoint> logger, ICodeBaseCommandService codeBaseCommandService)
    : Endpoint<UpdateCodeBaseRequest, UpdateCodeBaseResponse>
{
    private new ILogger<UpdateCodeBaseEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.PATCH);
        Routes("code-base/{id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateCodeBaseRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(UpdateCodeBaseEndpoint));
        var updateCodebaseResponse = codeBaseCommandService.UpdateAsync(req.Id, req);
        await SendAsync(await updateCodebaseResponse, cancellation: ct);
    }
}