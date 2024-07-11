using DomainEntities.Users;
using Generics.BaseEntities;
using Generics.Pagination;
using Common.Exceptions;

namespace UsersService.Database
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly List<User?> _users = new()
        {
            new User
            {
                Id = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
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

        public async Task<List<User?>> GetAllAsync(PaginationQuery paginationQuery)
        {
            var query = _users.AsQueryable();
            return await base.GetAllAsync(query, paginationQuery);
        }

        // Command-side operations
        public async Task<User?> AddAsync(User? user)
        {
            user.Id = Guid.NewGuid();
            _users.Add(user);
            return await Task.FromResult(user);
        }

        public async Task<User> UpdateAsync(Guid userId, User updatedUser)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user == null) throw new Exception(CoreMessages.EntityNotFound);

            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;
            user.Name = updatedUser.Name;
            user.Language = updatedUser.Language;
            return await Task.FromResult(user);
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user == null) throw new Exception(CoreMessages.UserDoesntExists);

            _ = await Task.Run(() => _users.Remove(user));
        }

        // Query-side operations
        public async Task<User?> GetByIdAsync(Guid userId)
        {
            var user = _users.FirstOrDefault(u => u.Id == userId);
            return await Task.FromResult(user);
        }
    }
}
