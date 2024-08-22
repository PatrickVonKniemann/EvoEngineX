using ExternalDomainEntities;
using FastEndpoints;

namespace CodeFormaterService.Endpoints;

public class FormatMatlabEndpoint : Endpoint<CodeRequest, CodeResponse>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/format/matlab");
        AllowAnonymous();
    }

    public override Task HandleAsync(CodeRequest req, CancellationToken ct)
    {
        return SendOkAsync(new CodeResponse { Code = "TODO implement matlab support" }, ct);
    }
}