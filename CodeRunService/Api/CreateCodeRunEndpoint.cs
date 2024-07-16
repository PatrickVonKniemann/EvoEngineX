using CodeRunService.Application.Services;
using CodeRunService.Application.Validators;
using DomainEntities.CodeRunDto.Command;
using FastEndpoints;

namespace CodeRunService.Api;

public class CreateCodeRunEndpoint(ILogger<CreateCodeRunEndpoint> logger, ICodeRunCommandService codeRunCommandService)
    : Endpoint<CreateCodeRunRequest, CreateCodeRunResponse>
{
    private new ILogger<CreateCodeRunEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("code-runs/add");
        AllowAnonymous();
        Validator<CreateCodeRunRequestValidator>();
    }

    public override async Task HandleAsync(CreateCodeRunRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(CreateCodeRunEndpoint));
        var createCodeRunResponse = codeRunCommandService.AddAsync(req);
        await SendAsync(await createCodeRunResponse, cancellation: ct);
    }
}