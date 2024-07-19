using Common.Exceptions;
using DomainEntities;
using Generics.BaseEntities;
using Generics.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CodeRunService.Infrastructure.Database;

public class CodeRunRepository : BaseRepository<CodeRun>, ICodeRunRepository
{
    private readonly CodeRunDbContext _context;

    public CodeRunRepository(CodeRunDbContext context)
    {
        _context = context;
    }

    // Query-side operations
    public async Task<CodeRun?> GetByIdAsync(Guid codeRunId)
    {
        return await _context.CodeRuns
            .FirstOrDefaultAsync(cr => cr.Id == codeRunId);
    }

    public async Task<List<CodeRun>> GetAllAsync(PaginationQuery paginationQuery)
    {
        var query = _context.CodeRuns.AsQueryable();
        return await base.GetAllAsync(query, paginationQuery);
    }

    // Command-side operations
    public async Task<CodeRun> AddAsync(CodeRun codeRun)
    {
        codeRun.Id = Guid.NewGuid();
        await _context.CodeRuns.AddAsync(codeRun);
        return await Task.FromResult(codeRun);
    }

    public async Task<CodeRun> UpdateAsync(Guid codeRunId, CodeRun updatedCodeRun)
    {
        var codeRun = await _context.CodeRuns.FindAsync(codeRunId);

        if (codeRun == null) throw new DbEntityNotFoundException("CodeRun", codeRunId);

        codeRun.RunFinish = updatedCodeRun.RunFinish;
        codeRun.RunStart = updatedCodeRun.RunStart;
        codeRun.Results = updatedCodeRun.Results;
        codeRun.Status = updatedCodeRun.Status;
        _context.CodeRuns.Update(codeRun);
        await _context.SaveChangesAsync();
        return codeRun;
    }

    public async Task DeleteAsync(Guid codeRunId)
    {
        var codeRun = await _context.CodeRuns.FindAsync(codeRunId);
        if (codeRun == null) throw new DbEntityNotFoundException("User", codeRunId);

        _context.CodeRuns.Remove(codeRun);
        await _context.SaveChangesAsync();
    }
}

