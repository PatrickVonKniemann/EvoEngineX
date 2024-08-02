using DomainEntities;
using Generics.BaseEntities;
using Generics.Pagination;

namespace CodebaseService.Infrastructure.Database;

public interface ICodeBaseRepository : IRepository<CodeBase>
{
    Task<List<CodeBase>> GetAllByUserIdAsync(Guid userId);
    Task<List<CodeBase>> GetAllByUserIdAsync(Guid userId, PaginationQuery paginationQuery);
    Task<int> GetCountByUserId(Guid userId);
}