using ExternalDomainEntities;
using FastEndpoints;

namespace CodeFormaterService.Endpoints;

public class ValidateCSharpEndpoint : Endpoint<CodeRequest, bool>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/validate/csharp");
        AllowAnonymous();
    }

    public override Task HandleAsync(CodeRequest req, CancellationToken ct)
    {
        return SendOkAsync(true, ct);
    }
}
