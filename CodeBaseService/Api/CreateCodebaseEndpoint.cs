using FastEndpoints;
using DomainEntities.CodeBaseDto.Command;
using CodebaseService.Application.Services;
using CodebaseService.Application.Validators;

namespace CodebaseService.Api;

public class CreateCodebaseEndpoint(ILogger<CreateCodebaseEndpoint> logger, ICodebaseCommandService codebaseCommandService)
    : Endpoint<CreateCodebaseRequest, CreateCodebaseResponse>
{
    private new ILogger<CreateCodebaseEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("codebases/add");
        AllowAnonymous();
        Validator<CreateCodebaseRequestValidator>();
    }

    public override async Task HandleAsync(CreateCodebaseRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(CreateCodebaseEndpoint));
        var createCodebaseResponse = codebaseCommandService.AddAsync(req);
        await SendAsync(await createCodebaseResponse, cancellation: ct);
    }
}