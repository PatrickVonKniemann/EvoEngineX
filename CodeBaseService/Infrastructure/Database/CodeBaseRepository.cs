using CodeBaseService.Infrastructure;
using DomainEntities;
using Generics.BaseEntities;
using Generics.Pagination;

namespace CodebaseService.Infrastructure.Database;

public class CodeBaseRepository(CodeBaseDbContext context,  ILogger<CodeBaseRepository> logger) : BaseRepository<CodeBase>(context, logger), ICodeBaseRepository
{
    public async Task<List<CodeBase>> GetAllByUserIdAsync(Guid userId, PaginationQuery? paginationQuery = null)
    {
        return await GetAllAsync(
            paginationQuery: paginationQuery,
            filter: codeBase => codeBase.UserId == userId
        );
    }


    public async Task<int> GetCountByUserId(Guid userId)
    {
        return await GetCountByParameterAsync("UserId", userId);
    }
}