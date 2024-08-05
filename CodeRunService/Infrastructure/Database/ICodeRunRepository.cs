using DomainEntities;
using Generics.BaseEntities;
using Generics.Pagination;

namespace CodeRunService.Infrastructure.Database;

public interface ICodeRunRepository : IRepository<CodeRun>
{
    Task<List<CodeRun>> GetAllByCodeBaseIdAsync(Guid codeBaseId);
    Task<List<CodeRun>> GetAllByCodeBaseIdAsync(Guid codeBaseId, PaginationQuery paginationQuery);
    Task<int> GetCountByCodeBaseId(Guid codeBaseId);
}