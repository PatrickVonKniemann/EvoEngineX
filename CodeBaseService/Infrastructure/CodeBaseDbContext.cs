using DomainEntities;
using Microsoft.EntityFrameworkCore;

namespace CodeBaseService.Infrastructure;

public class CodeBaseDbContext : DbContext
{
    public DbSet<CodeBase> CodeBases { get; set; }

    public CodeBaseDbContext(DbContextOptions<CodeBaseDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}