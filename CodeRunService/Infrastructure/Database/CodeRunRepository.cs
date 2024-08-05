using DomainEntities;
using Generics.BaseEntities;
using Generics.Pagination;

namespace CodeRunService.Infrastructure.Database;

public class CodeRunRepository(CodeRunDbContext context, ILogger<CodeRunRepository> logger)
    : BaseRepository<CodeRun>(context, logger), ICodeRunRepository
{
    public async Task<List<CodeRun>> GetAllByCodeBaseIdAsync(Guid codeBaseId)
    {
        return await GetAllByParameterAsync("CodeBaseId", codeBaseId);
    }

    public async Task<List<CodeRun>> GetAllByCodeBaseIdAsync(Guid codeBaseId, PaginationQuery paginationQuery)
    {
        return await GetAllByParameterAsync("CodeBaseId", codeBaseId, paginationQuery);
    }

    public async Task<int> GetCountByCodeBaseId(Guid codeBaseId)
    {
        return await GetCountByParameterAsync("CodeBaseId", codeBaseId);
    }
}