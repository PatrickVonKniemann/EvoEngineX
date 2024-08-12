using System.Diagnostics;
using System.Text;
using ExternalDomainEntities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Formatting;
using Microsoft.CodeAnalysis.Formatting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        corsPolicyBuilder => corsPolicyBuilder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

var app = builder.Build();
app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/format/matlab", async (CodeRequest request) =>
{
    // string formattedCode = await FormatMatlabCodeAsync(request.Code);
    // return Results.Ok(new { formattedCode });
    return Results.Ok(new CodeResponse { Code = "TODO implement matlab support" });
});

app.MapPost("/format/java", async (CodeRequest request) =>
{
    // string formattedCode = await FormatJavaCodeAsync(request.Code);
    // return Results.Ok(new { formattedCode });
    return Results.Ok(new CodeResponse { Code = "TODO implement java support" });
});

app.MapPost("/format/csharp", async (CodeRequest request) =>
{
    string formattedCode = await FormatCSharpCodeAsync(request.Code);
    return Results.Ok(new CodeResponse { Code = formattedCode });
});


await app.RunAsync();

async Task<string> FormatCSharpCodeAsync(string code)
{
    // Create a syntax tree from the code
    var tree = CSharpSyntaxTree.ParseText(code);

    // Create a workspace
    using var workspace = new AdhocWorkspace();

    // Retrieve the root of the syntax tree
    var root = await tree.GetRootAsync();

    // Create options for formatting based on standard conventions
    var options = workspace.Options
        .WithChangedOption(CSharpFormattingOptions.NewLinesForBracesInMethods, true)
        .WithChangedOption(CSharpFormattingOptions.IndentBraces, true)
        .WithChangedOption(FormattingOptions.UseTabs, LanguageNames.CSharp, false)  // Using spaces instead of tabs
        .WithChangedOption(FormattingOptions.TabSize, LanguageNames.CSharp, 4)
        .WithChangedOption(FormattingOptions.IndentationSize, LanguageNames.CSharp, 4);

    // Format the root node
    var formattedRoot = Formatter.Format(root, workspace, options);

    // Return the formatted code as a string
    return formattedRoot.ToFullString();
}