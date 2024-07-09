using DomainEntities.Users;
using Generics.Pagination;

namespace UsersService.Database;

public class UserRepository : IUserRepository
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
        }
    };

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

    public (List<User> users, int totalCount) GetAll(PaginationQuery paginationQuery)
    {
        var totalCount = _users.Count;
        var users = _users
            .Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize)
            .Take(paginationQuery.PageSize)
            .ToList();

        return (users, totalCount);
    }
}