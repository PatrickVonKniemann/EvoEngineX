using DomainEntities;
using Generics.BaseEntities;

namespace CodebaseService.Infrastructure.Database;

public interface ICodeBaseRepository : IRepository<CodeBase>
{
    Task<List<CodeBase>> GetAllByUserIdAsync(Guid userId);
}