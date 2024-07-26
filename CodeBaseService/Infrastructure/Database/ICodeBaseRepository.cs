using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DomainEntities;
using Generics.Pagination;

namespace CodebaseService.Infrastructure.Database
{
    public interface ICodeBaseRepository
    {
        // Command-side operations
        Task<CodeBase> AddAsync(CodeBase codeBase);
        Task<CodeBase> UpdateAsync(Guid codeBaseId, CodeBase updatedCodeBase);
        Task DeleteAsync(Guid codeBaseId);

        // Query-side operations
        Task<CodeBase?> GetByIdAsync(Guid codeBaseId);
        Task<List<CodeBase>> GetAllAsync();
        Task<List<CodeBase>> GetAllAsync(PaginationQuery? paginationQuery);
        Task<List<CodeBase>> GetAllByUserIdAsync(Guid userId);
    }
}