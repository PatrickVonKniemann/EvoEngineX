using ExternalDomainEntities;
using FastEndpoints;

namespace CodeFormaterService.Endpoints;

public class FormatJavaEndpoint : Endpoint<CodeRequest, CodeResponse>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/format/java");
        AllowAnonymous();
    }

    public override Task HandleAsync(CodeRequest req, CancellationToken ct)
    {
        return SendOkAsync(new CodeResponse { Code = "TODO implement java support" }, ct);
    }
}
