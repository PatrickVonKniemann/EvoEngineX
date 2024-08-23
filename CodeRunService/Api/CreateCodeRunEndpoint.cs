using System.Globalization;
using CodeRunService.Application.Services;
using CodeRunService.Application.Validators;
using ExternalDomainEntities.CodeRunDto.Command;
using FastEndpoints;
using Generics.Enums;

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
        Logger.LogInformation(nameof(CreateCodeRunEndpoint));
        DateTimeOffset now = DateTimeOffset.UtcNow;
        string formattedDateTimeString = now.ToString("yyyy-MM-dd HH:mm:ss.ffffff zzz");

        // Parse back to DateTimeOffset to ensure we keep the UTC offset
        DateTimeOffset parsedDateTimeOffset = DateTimeOffset.ParseExact(formattedDateTimeString,
            "yyyy-MM-dd HH:mm:ss.ffffff zzz", CultureInfo.InvariantCulture);

        // Convert to DateTime with UTC kind
        DateTime formattedDateTime = parsedDateTimeOffset.UtcDateTime;

        CreateCodeRunDetailRequest newCodeRun = new CreateCodeRunDetailRequest
        {
            CodeBaseId = req.CodeBaseId,
            Code = req.Code,
            Status = RunStatus.Ready,
            RunStart = formattedDateTime
        };

        var createCodeRunResponse = codeRunCommandService.AddAsync(newCodeRun);
        await SendAsync(await createCodeRunResponse, cancellation: ct);
    }
}