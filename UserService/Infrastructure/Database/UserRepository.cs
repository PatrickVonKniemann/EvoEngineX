using Common.Exceptions;
using DomainEntities;
using Generics.BaseEntities;
using Generics.Pagination;
using Microsoft.EntityFrameworkCore;

namespace UserService.Infrastructure.Database;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly UserDbContext _context;

    public UserRepository(UserDbContext context)
    {
        _context = context;
    }
    
    // Query-side operations
    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(user => user.Id == userId);
    }

    public async Task<List<User>> GetAllAsync(PaginationQuery paginationQuery)
    {
        var query = _context.Users.AsQueryable();
        return await base.GetAllAsync(query, paginationQuery);
    }

    // Command-side operations
    public async Task<User> AddAsync(User user)
    {
        user.Id = Guid.NewGuid();
        await _context.Users.AddAsync(user);
        return await Task.FromResult(user);
    }

    public async Task<User> UpdateAsync(Guid userId, User updatedUser)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null) throw new DbEntityNotFoundException("User", userId);

        user.UserName = updatedUser.UserName;
        user.Email = updatedUser.Email;
        user.Name = updatedUser.Name;
        user.Language = updatedUser.Language;
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task DeleteAsync(Guid userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) throw new DbEntityNotFoundException("User", userId);

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }
}