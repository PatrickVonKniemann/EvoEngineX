using DomainEntities.Users;
using Generics.Pagination;

namespace UsersService.Database;

public interface IUserRepository
{
    // Command-side operations
    User? Add(User? user);
    User Update(Guid userId, User updatedUser);
    void Delete(Guid userId);

    // Query-side operations
    User? GetById(Guid userId);
    List<User?> GetAll(PaginationQuery paginationQuery);
}
