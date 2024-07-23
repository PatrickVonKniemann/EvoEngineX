using Common.Exceptions;
using DomainEntities;
using Generics.BaseEntities;
using Generics.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CodeRunService.Infrastructure.Database;

public class CodeRunRepository(CodeRunDbContext context) : BaseRepository<CodeRun>, ICodeRunRepository
{
    // Query-side operations
    public async Task<CodeRun?> GetByIdAsync(Guid codeRunId)
    {
        return await context.CodeRuns
            .FirstOrDefaultAsync(cr => cr.Id == codeRunId);
    }

    public async Task<List<CodeRun>> GetAllAsync()
    {
        var query = context.CodeRuns.AsQueryable();
        return await base.GetAllAsync(query);
    }

    public async Task<List<CodeRun>> GetAllAsync(PaginationQuery paginationQuery)
    {
        var query = context.CodeRuns.AsQueryable();
        return await base.GetAllAsync(query, paginationQuery);
    }

    public async Task<List<CodeRun>> GetAllByCodeBaseIdAsync(Guid codeBaseId)
    {
        return await context.CodeRuns.Where(cr => cr.CodeBaseId == codeBaseId)
            .ToListAsync();
    }

    // Command-side operations
    public async Task<CodeRun> AddAsync(CodeRun codeRun)
    {
        codeRun.Id = Guid.NewGuid();
        await context.CodeRuns.AddAsync(codeRun);
        return await Task.FromResult(codeRun);
    }

    public async Task<CodeRun> UpdateAsync(Guid codeRunId, CodeRun updatedCodeRun)
    {
        var codeRun = await context.CodeRuns.FindAsync(codeRunId);

        if (codeRun == null) throw new DbEntityNotFoundException("CodeRun", codeRunId);

        codeRun.RunFinish = updatedCodeRun.RunFinish;
        codeRun.RunStart = updatedCodeRun.RunStart;
        codeRun.Results = updatedCodeRun.Results;
        codeRun.Status = updatedCodeRun.Status;
        context.CodeRuns.Update(codeRun);
        await context.SaveChangesAsync();
        return codeRun;
    }

    public async Task DeleteAsync(Guid codeRunId)
    {
        var codeRun = await context.CodeRuns.FindAsync(codeRunId);
        if (codeRun == null) throw new DbEntityNotFoundException("CodeRun", codeRunId);

        context.CodeRuns.Remove(codeRun);
        await context.SaveChangesAsync();
    }
}