using System.Linq.Expressions;
using DomainEntities;
using Generics.BaseEntities;
using Generics.Enums;
using Generics.Pagination;

namespace CodeRunService.Infrastructure.Database;

public interface ICodeRunRepository : IRepository<CodeRun>
{
    // Single method for getting all by CodeBaseId with optional pagination
    Task<List<CodeRun>> GetAllByCodeBaseIdAsync(Guid codeBaseId, PaginationQuery? paginationQuery = null,
        params Expression<Func<CodeRun, object>>[] includes);

    // Method for getting the count by CodeBaseId
    Task<int> GetCountByCodeBaseIdAsync(Guid codeBaseId);

    // Method for reading logs from the database for a specific CodeRunId
    Task<RunResult> ReadLogsFromDatabaseAsync(Guid codeRunId);
}