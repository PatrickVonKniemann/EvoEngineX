using DomainEntities;
using Microsoft.EntityFrameworkCore;

namespace UserService.Infrastructure;

public class UserDbContext : DbContext
{
    public DbSet<User?> Users { get; set; }

    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Additional configurations if needed
    }
}