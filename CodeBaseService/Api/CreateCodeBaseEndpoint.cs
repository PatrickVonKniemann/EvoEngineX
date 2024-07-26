using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using CodebaseService.Application.Services;
using CodebaseService.Application.Validators;
using ExternalDomainEntities.CodeBaseDto.Command;
using Microsoft.Extensions.Logging;

namespace CodebaseService.Api;

public class CreateCodeBaseEndpoint(ILogger<CreateCodeBaseEndpoint> logger, ICodeBaseCommandService codeBaseCommandService)
    : Endpoint<CreateCodeBaseRequest, CreateCodeBaseResponse>
{
    private new ILogger<CreateCodeBaseEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("code-base/add");
        AllowAnonymous();
        Validator<CreateCodeBaseRequestValidator>();
    }

    public override async Task HandleAsync(CreateCodeBaseRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(CreateCodeBaseEndpoint));
        var createCodebaseResponse = codeBaseCommandService.AddAsync(req);
        await SendAsync(await createCodebaseResponse, cancellation: ct);
    }
}