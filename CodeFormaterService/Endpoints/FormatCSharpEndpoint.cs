using ExternalDomainEntities;
using FastEndpoints;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;

namespace CodeFormaterService.Endpoints;

public class FormatCSharpEndpoint : Endpoint<CodeRequest, CodeResponse>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("/format/csharp");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CodeRequest req, CancellationToken ct)
    {
        string formattedCode = await FormatCSharpCodeAsync(req.Code);
        await SendOkAsync(new CodeResponse { Code = formattedCode }, ct);
    }
    
    async Task<string> FormatCSharpCodeAsync(string code)
    {
        var tree = CSharpSyntaxTree.ParseText(code);
        using var workspace = new AdhocWorkspace();
        var root = await tree.GetRootAsync();

        var options = workspace.Options
            .WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods, true)
            .WithChangedOption(CSharpFormattingOptions.IndentBraces, true)
            .WithChangedOption(FormattingOptions.UseTabs, LanguageNames.CSharp, false)
            .WithChangedOption(FormattingOptions.TabSize, LanguageNames.CSharp, 4)
            .WithChangedOption(FormattingOptions.IndentationSize, LanguageNames.CSharp, 4);

        var formattedRoot = Formatter.Format(root, workspace, options);

        return formattedRoot.ToFullString();
    }
}