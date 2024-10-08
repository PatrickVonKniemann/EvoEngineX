using CodeRunService.Application.Services;
using CodeRunService.Application.Validators;
using ExternalDomainEntities.CodeRunDto.Command;
using FastEndpoints;


namespace CodeRunService.Api;

public class CreateCodeRunEndpoint(ILogger<CreateCodeRunEndpoint> logger, ICodeRunCommandService codeRunCommandService)
    : Endpoint<CreateCodeRunRequest, CreateCodeRunResponse>
{
    private new ILogger<CreateCodeRunEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("code-run/add");
        AllowAnonymous();
        Validator<CreateCodeRunRequestValidator>();
    }

    public override async Task HandleAsync(CreateCodeRunRequest req, CancellationToken ct)
    {
        Logger.LogInformation("Starting creation process for CodeRun.");
        Logger.LogInformation("Endpoint: {EndpointName}", nameof(CreateCodeRunEndpoint));

        var createCodeRunResponse = codeRunCommandService.HandleAddAsync(req);

        await SendAsync(await createCodeRunResponse, cancellation: ct);
    }
}