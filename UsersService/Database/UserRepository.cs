using System.Linq.Expressions;
using System.Reflection;
using DomainEntities.Users;
using DomainEntities.Users.Query;
using Generics.BaseEntities;
using Generics.Pagination;

namespace UsersService.Database;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly List<User> _users = new()
    {
        new User
        {
            Id = Guid.NewGuid(),
            UserName = "john_doe",
            Email = "john.doe@example.com",
            Name = "John Doe",
            Language = "English"
        },
        new User
        {
            Id = Guid.NewGuid(),
            UserName = "jane_smith",
            Email = "jane.smith@example.com",
            Name = "Jane Smith",
            Language = "English"
        },
        new User
        {
            Id = Guid.NewGuid(),
            UserName = "maria_garcia",
            Email = "maria.garcia@example.com",
            Name = "Maria Garcia",
            Language = "Spanish"
        },
        new User
        {
            Id = Guid.NewGuid(),
            UserName = "maria_garcia2",
            Email = "maria2.garcia@example.com",
            Name = "Maria2 Garcia",
            Language = "Spanish"
        }
    };

    public List<User> GetAll(PaginationQuery paginationQuery)
    {
        var query = _users.AsQueryable();

        // Use the base repository methods to apply filtering, sorting, and pagination
        return base.GetAll(query, paginationQuery);
    }
    
    // Command-side operations
    public User Add(User user)
    {
        user.Id = Guid.NewGuid();
        _users.Add(user);
        return user;
    }

    public User Update(Guid userId, User updatedUser)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user == null) throw new Exception("User not found");

        user.UserName = updatedUser.UserName;
        user.Email = updatedUser.Email;
        user.Name = updatedUser.Name;
        user.Language = updatedUser.Language;
        return user;
    }

    public void Delete(Guid userId)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        if (user == null) throw new Exception("User not found");

        _users.Remove(user);
    }

    // Query-side operations
    public User GetById(Guid userId)
    {
        return _users.FirstOrDefault(u => u.Id == userId);
    }

    
   
}