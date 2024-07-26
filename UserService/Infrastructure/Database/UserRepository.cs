using DomainEntities;
using Generics.BaseEntities;
using Generics.Exceptions;
using Generics.Pagination;
using Microsoft.EntityFrameworkCore;

namespace UserService.Infrastructure.Database;

public class UserRepository(UserDbContext context) : BaseRepository<User>, IUserRepository
{
    // Query-side operations
    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await context.Users
            .FirstOrDefaultAsync(user => user.Id == userId);
    }

    public async Task<List<User>> GetAllAsync()
    {
        var query = context.Users.AsQueryable();
        return await base.GetAllAsync(query);
    }

    public async Task<List<User>> GetAllAsync(PaginationQuery? paginationQuery)
    {
        var query = context.Users.AsQueryable();
        return await base.GetAllAsync(query, paginationQuery);
    }

    // Command-side operations
    public async Task<User> AddAsync(User user)
    {
        user.Id = Guid.NewGuid();
        await context.Users.AddAsync(user);
        return await Task.FromResult(user);
    }

    public async Task<User> UpdateAsync(Guid userId, User updatedUser)
    {
        var user = await context.Users.FindAsync(userId);

        if (user == null) throw new DbEntityNotFoundException("User", userId);

        user.UserName = updatedUser.UserName;
        user.Email = updatedUser.Email;
        user.Name = updatedUser.Name;
        user.Language = updatedUser.Language;
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(Guid userId)
    {
        var user = await context.Users.FindAsync(userId);
        if (user == null) throw new DbEntityNotFoundException("User", userId);

        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }
}