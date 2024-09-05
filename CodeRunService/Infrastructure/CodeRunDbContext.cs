using DomainEntities;
using Microsoft.EntityFrameworkCore;

namespace CodeRunService.Infrastructure;

public class CodeRunDbContext(DbContextOptions<CodeRunDbContext> options) : DbContext(options)
{
    public DbSet<CodeRun> CodeRuns { get; set; }
}