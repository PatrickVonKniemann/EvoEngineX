using DomainEntities;
using Generics.BaseEntities;

namespace CodeRunService.Infrastructure.Database;

public interface ICodeRunRepository : IRepository<CodeRun>
{
    Task<List<CodeRun>> GetAllByCodeBaseIdAsync(Guid codeBaseId);
}