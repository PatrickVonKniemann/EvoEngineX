using Common.Exceptions;
using DomainEntities;
using Generics.BaseEntities;
using Generics.Pagination;
using CodeRunService.Infrastructure.Database;

namespace CodeRunService.Infrastructure.Database
{
    public class CodeRunRepository : BaseRepository<CodeRun>, ICodeRunRepository
    {
        private readonly List<CodeRun?> _codeRuns =
        [
            new CodeRun
            {
                Id = new Guid("123e4567-e89b-12d3-a456-426614174000"),
            },

            new CodeRun
            {
                Id = Guid.NewGuid(),
            },

            new CodeRun
            {
                Id = Guid.NewGuid(),
            },

            new CodeRun
            {
                Id = Guid.NewGuid(),

            }
        ];

        public async Task<List<CodeRun?>> GetAllAsync(PaginationQuery paginationQuery)
        {
            var query = _codeRuns.AsQueryable();
            return await base.GetAllAsync(query, paginationQuery);
        }

        // Command-side operations
        public async Task<CodeRun?> AddAsync(CodeRun? codeRun)
        {
            codeRun.Id = Guid.NewGuid();
            _codeRuns.Add(codeRun);
            return await Task.FromResult(codeRun);
        }

        public async Task<CodeRun> UpdateAsync(Guid codeRunId, CodeRun updatedCodeRun)
        {
            var codeRun = _codeRuns.FirstOrDefault(u => u.Id == codeRunId);
            if (codeRun == null) throw new DbEntityNotFoundException("CodeRun", codeRunId);

            // codeRun.CodeRunName = updatedCodeRun.CodeRunName;
            // codeRun.Email = updatedCodeRun.Email;
            // codeRun.Name = updatedCodeRun.Name;
            // codeRun.Language = updatedCodeRun.Language;
            return await Task.FromResult(codeRun);
        }

        public async Task DeleteAsync(Guid codeRunId)
        {
            var codeRun = _codeRuns.FirstOrDefault(u => u.Id == codeRunId);
            if (codeRun == null) throw new DbEntityNotFoundException("CodeRun", codeRunId);

            _ = await Task.Run(() => _codeRuns.Remove(codeRun));
        }

        // Query-side operations
        public async Task<CodeRun?> GetByIdAsync(Guid codeRunId)
        {
            var codeRun = _codeRuns.FirstOrDefault(u => u.Id == codeRunId);
            return await Task.FromResult(codeRun);
        }
    }
}
