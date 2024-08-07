using System.Diagnostics;
using ExternalDomainEntities;

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
    return Results.Ok(new CodeResponse { Code = "TestMatlabCode" });
});

app.MapPost("/format/java", async (CodeRequest request) =>
{
    // string formattedCode = await FormatJavaCodeAsync(request.Code);
    // return Results.Ok(new { formattedCode });
    return Results.Ok(new CodeResponse { Code = "TestJavaCode" });
});

app.MapPost("/format/csharp", async (CodeRequest request) =>
{
    // string formattedCode = await FormatCSharpCodeAsync(request.Code);
    // return Results.Ok(new { formattedCode });
    return Results.Ok(new CodeResponse { Code = "TestCsharpCode" });
});


app.Run();

async Task<string> FormatMatlabCodeAsync(string code)
{
    // Validate MATLAB code
    bool isValid = await ValidateMatlabCodeAsync(code);
    if (!isValid)
    {
        throw new ArgumentException("The provided MATLAB code is invalid.");
    }

    // Implement MATLAB code formatting logic here
    // Placeholder implementation
    await Task.Delay(10); // Simulate async work
    return code; // Replace with actual formatted code
}

async Task<bool> ValidateMatlabCodeAsync(string code)
{
    var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "matlab",
            Arguments = "-batch \"try, eval('" + code.Replace("'", "''") + "'), catch, exit(1), end, exit(0)\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }
    };

    process.Start();
    string errors = await process.StandardError.ReadToEndAsync();
    await process.WaitForExitAsync();

    return process.ExitCode == 0 && string.IsNullOrEmpty(errors);
}

async Task<string> FormatJavaCodeAsync(string code)
{
    // Validate Java code
    bool isValid = await ValidateJavaCodeAsync(code);
    if (!isValid)
    {
        throw new ArgumentException("The provided Java code is invalid.");
    }

    // Implement Java code formatting logic here
    // Placeholder implementation using google-java-format
    var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "java",
            Arguments = $"-jar path/to/google-java-format.jar -",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }
    };

    process.Start();
    await process.StandardInput.WriteAsync(code);
    process.StandardInput.Close();
    string formattedCode = await process.StandardOutput.ReadToEndAsync();
    await process.WaitForExitAsync();
    return formattedCode;
}

async Task<bool> ValidateJavaCodeAsync(string code)
{
    var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "javac",
            Arguments = "-Xlint -d . -",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }
    };

    process.Start();
    await process.StandardInput.WriteAsync(code);
    process.StandardInput.Close();
    string errors = await process.StandardError.ReadToEndAsync();
    await process.WaitForExitAsync();

    return string.IsNullOrEmpty(errors);
}

async Task<string> FormatCSharpCodeAsync(string code)
{
    // Validate C# code
    bool isValid = await ValidateCSharpCodeAsync(code);
    if (!isValid)
    {
        throw new ArgumentException("The provided C# code is invalid.");
    }

    // Implement C# code formatting logic here
    // Placeholder implementation using dotnet-format
    var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = "format -",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }
    };

    process.Start();
    await process.StandardInput.WriteAsync(code);
    process.StandardInput.Close();
    string formattedCode = await process.StandardOutput.ReadToEndAsync();
    await process.WaitForExitAsync();
    return formattedCode;
}

async Task<bool> ValidateCSharpCodeAsync(string code)
{
    var process = new Process
    {
        StartInfo = new ProcessStartInfo
        {
            FileName = "csc",
            Arguments = "-nologo -t:library -",
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }
    };

    process.Start();
    await process.StandardInput.WriteAsync(code);
    process.StandardInput.Close();
    string errors = await process.StandardError.ReadToEndAsync();
    await process.WaitForExitAsync();

    return process.ExitCode == 0 && string.IsNullOrEmpty(errors);
}