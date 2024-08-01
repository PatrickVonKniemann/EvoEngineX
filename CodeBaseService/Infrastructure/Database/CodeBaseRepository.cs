using CodeBaseService.Infrastructure;
using DomainEntities;
using Generics.BaseEntities;

namespace CodebaseService.Infrastructure.Database;

public class CodeBaseRepository(CodeBaseDbContext context,  ILogger<CodeBaseRepository> logger) : BaseRepository<CodeBase>(context, logger), ICodeBaseRepository
{
    public async Task<List<CodeBase>> GetAllByUserIdAsync(Guid userId)
    {
        return await GetAllByParameterAsync("UserId", userId);
    }
}