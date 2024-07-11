using DomainEntities.Users;
using Generics.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UsersService.Database
{
    public interface IUserRepository
    {
        // Command-side operations
        Task<User?> AddAsync(User? user);
        Task<User> UpdateAsync(Guid userId, User updatedUser);
        Task DeleteAsync(Guid userId);

        // Query-side operations
        Task<User?> GetByIdAsync(Guid userId);
        Task<List<User?>> GetAllAsync(PaginationQuery paginationQuery);
    }
}