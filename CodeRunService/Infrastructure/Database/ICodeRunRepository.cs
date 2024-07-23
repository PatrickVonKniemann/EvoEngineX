using DomainEntities;
using Generics.Pagination;

namespace CodeRunService.Infrastructure.Database
{
    public interface ICodeRunRepository
    {
        // Command-side operations
        Task<CodeRun> AddAsync(CodeRun codeRun);
        Task<CodeRun> UpdateAsync(Guid codeRunId, CodeRun updatedCodeRun);
        Task DeleteAsync(Guid codeRunId);

        // Query-side operations
        Task<CodeRun?> GetByIdAsync(Guid codeRunId);
        Task<List<CodeRun>> GetAllAsync();
        Task<List<CodeRun>> GetAllAsync(PaginationQuery paginationQuery);
        Task<List<CodeRun>> GetAllByCodeBaseIdAsync(Guid codeBaseId);
    }
}