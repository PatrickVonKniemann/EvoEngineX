using DomainEntities;
using Microsoft.EntityFrameworkCore;

namespace CodeRunService.Infrastructure;

public class CodeRunDbContext : DbContext
{
    public DbSet<CodeRun> CodeRuns { get; set; }

    public CodeRunDbContext(DbContextOptions<CodeRunDbContext> options)
        : base(options)
    {
    }
}