using DomainEntities;
using Microsoft.EntityFrameworkCore;

namespace CodeBaseService.Infrastructure;

public class CodeBaseDbContext(DbContextOptions<CodeBaseDbContext> options) : DbContext(options)
{
    public DbSet<CodeBase> CodeBases { get; set; }
}