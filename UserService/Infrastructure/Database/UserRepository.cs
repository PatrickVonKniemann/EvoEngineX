using Common.Exceptions;
using DomainEntities;
using Generics.BaseEntities;
using Generics.Pagination;
using Microsoft.EntityFrameworkCore;

namespace UserService.Infrastructure.Database;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly UserDbContext _context;

    // Query-side operations
    public async Task<User?> GetByIdAsync(Guid userId)
    {
        return await _context.Users
            .FirstOrDefaultAsync(user => user.Id == userId);
    }

    public async Task<List<User?>> GetAllAsync(PaginationQuery paginationQuery)
    {
        var query = _context.Users.AsQueryable();
        return await base.GetAllAsync(query, paginationQuery);
    }

    // Command-side operations
    public async Task<User?> AddAsync(User? user)
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

// private readonly List<User?> _users =
// [
//     new User
//     {
//         Id = new Guid("123e4567-e89b-12d3-a456-426614174000"),
//         UserName = "john_doe",
//         Email = "john.doe@example.com",
//         Name = "John Doe",
//         Language = "English"
//     },
//
//     new User
//     {
//         Id = Guid.NewGuid(),
//         UserName = "jane_smith",
//         Email = "jane.smith@example.com",
//         Name = "Jane Smith",
//         Language = "English"
//     },
//
//     new User
//     {
//         Id = Guid.NewGuid(),
//         UserName = "maria_garcia",
//         Email = "maria.garcia@example.com",
//         Name = "Maria Garcia",
//         Language = "Spanish"
//     },
//
//     new User
//     {
//         Id = Guid.NewGuid(),
//         UserName = "maria_garcia2",
//         Email = "maria2.garcia@example.com",
//         Name = "Maria2 Garcia",
//         Language = "Spanish"
//     }
// ];