using DomainEntities;
using Generics.Pagination;

namespace CodebaseService.Infrastructure.Database
{
    public interface ICodebaseRepository
    {
        // Command-side operations
        Task<Codebase?> AddAsync(Codebase? codebase);
        Task<Codebase> UpdateAsync(Guid codebaseId, Codebase updatedCodebase);
        Task DeleteAsync(Guid codebaseId);

        // Query-side operations
        Task<Codebase?> GetByIdAsync(Guid codebaseId);
        Task<List<Codebase?>> GetAllAsync(PaginationQuery paginationQuery);
    }
}