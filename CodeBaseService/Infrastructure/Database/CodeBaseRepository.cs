using CodeBaseService.Infrastructure;
using DomainEntities;
using Generics.BaseEntities;

namespace CodebaseService.Infrastructure.Database;

public class CodeBaseRepository(CodeBaseDbContext context) : BaseRepository<CodeBase>(context), ICodeBaseRepository
{
    public async Task<List<CodeBase>> GetAllByUserIdAsync(Guid userId)
    {
        return await GetAllByParameterAsync("UserId", userId);
    }
}