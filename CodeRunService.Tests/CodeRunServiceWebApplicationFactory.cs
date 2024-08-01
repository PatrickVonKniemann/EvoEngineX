using Common;
using CodeRunService.Infrastructure;

namespace CodeRunService.Tests
{
    public class CodeRunServiceWebApplicationFactory<TStartup>()
        : CustomWebApplicationFactory<TStartup, CodeRunDbContext>("CodeRunServiceDb", new List<string> { "CodeRuns" },
            "../../../../Configs/SqlScripts")
        where TStartup : class;
}