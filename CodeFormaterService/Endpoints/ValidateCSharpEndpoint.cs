using CodeFormaterService.Services;
using ExternalDomainEntities;
using FastEndpoints;

namespace CodeFormaterService.Endpoints;

public class ValidateCSharpEndpoint(ICodeValidationService codeValidationService) : Endpoint<CodeRequest, bool>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/validate/csharp");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CodeRequest req, CancellationToken ct)
    {
        // await SendOkAsync(await codeValidationService.ValidateAsync(req.Code), ct);
        await SendOkAsync(true, ct);
    }
}