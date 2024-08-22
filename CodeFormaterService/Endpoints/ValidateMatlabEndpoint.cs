using ExternalDomainEntities;
using FastEndpoints;

namespace CodeFormaterService.Endpoints;

public class ValidateMatlabEndpoint : Endpoint<CodeRequest, bool>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/validate/matlab");
        AllowAnonymous();
    }

    public override Task HandleAsync(CodeRequest req, CancellationToken ct)
    {
        return SendOkAsync(true, ct);
    }
}
