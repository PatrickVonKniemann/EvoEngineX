using FastEndpoints;
using CodeRunService.Application.Services;
using ExternalDomainEntities.CodeRunDto.Query;

namespace CodeRunService.Api;

public class ReadCodeRunFileEndpoint(ILogger<ReadCodeRunFileEndpoint> logger, ICodeRunQueryService codeRunQueryService)
    : Endpoint<ReadRunResultRequest>
{
    private new ILogger<ReadCodeRunFileEndpoint> Logger { get; } = logger;

    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("code-run/{ID}/file");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ReadRunResultRequest req, CancellationToken ct)
    {
        Logger.LogInformation(nameof(ReadCodeRunEndpoint));

        // Fetch the file from the service
        ReadCodeRunFileResponse response = await codeRunQueryService.GetFileByIdAsyncDetail(req.Id);

        if (response.File == null || response.File.Length == 0)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        // Set the content type and the file name (change MIME type if necessary)
        HttpContext.Response.ContentType = "text/csv";
        HttpContext.Response.Headers.Append("Content-Disposition", $"attachment; filename=\"result.csv\"");

        // Write the byte[] (file content) to the response body
        await HttpContext.Response.Body.WriteAsync(response.File, 0, response.File.Length, ct);
    }
}