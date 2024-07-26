using DomainEntities;
using Microsoft.EntityFrameworkCore;

namespace UserService.Infrastructure;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}