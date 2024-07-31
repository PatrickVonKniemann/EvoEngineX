using DomainEntities;
using Generics.BaseEntities;

namespace CodeRunService.Infrastructure.Database;

public class CodeRunRepository(CodeRunDbContext context) : BaseRepository<CodeRun>(context), ICodeRunRepository
{
    public async Task<List<CodeRun>> GetAllByCodeBaseIdAsync(Guid codeBaseId)
    {
        return await GetAllByParameterAsync("CodeBaseId", codeBaseId);
    }
}