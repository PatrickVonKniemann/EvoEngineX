using Common;
using CodeBaseService.Infrastructure;

namespace CodeBase.Tests;

public class CodeBaseServiceWebApplicationFactory<TStartup>()
    : CustomWebApplicationFactory<TStartup, CodeBaseDbContext>("CodeBaseServiceDb",
        new List<string> { "CodeBases" }, "../../../../Configs/SqlScripts")
    where TStartup : class;