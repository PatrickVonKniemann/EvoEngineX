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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Additional configurations if needed
    }
}