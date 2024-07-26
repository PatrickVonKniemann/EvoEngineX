using CodeBaseService.Infrastructure;
using DomainEntities;
using Generics.BaseEntities;
using Generics.Exceptions;
using Generics.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CodebaseService.Infrastructure.Database;

// TODO fix repositories to makes sence
public class CodeBaseRepository(CodeBaseDbContext context) : BaseRepository<CodeBase>, ICodeBaseRepository
{
    // Query-side operations
    public async Task<CodeBase?> GetByIdAsync(Guid codeBaseId)
    {
        return await context.CodeBases
            .FirstOrDefaultAsync(cb => cb.Id == codeBaseId);
    }


    public async Task<List<CodeBase>> GetAllAsync()
    {
        var query = context.CodeBases.AsQueryable();
        return await base.GetAllAsync(query);
    }

    public async Task<List<CodeBase>> GetAllAsync(PaginationQuery? paginationQuery)
    {
        var query = context.CodeBases.AsQueryable();
        return await base.GetAllAsync(query, paginationQuery);
    }

    public async Task<List<CodeBase>> GetAllByUserIdAsync(Guid userId)
    {
        return await context.CodeBases.Where(cb => cb.UserId == userId)
            .ToListAsync();
    }

    // Command-side operations
    public async Task<CodeBase> AddAsync(CodeBase codeBase)
    {
        codeBase.Id = Guid.NewGuid();
        await context.CodeBases.AddAsync(codeBase);
        return await Task.FromResult(codeBase);
    }

    public async Task<CodeBase> UpdateAsync(Guid codeBaseId, CodeBase updatedCodeBase)
    {
        var codeBase = await context.CodeBases.FindAsync(codeBaseId);
        if (codeBase == null) throw new DbEntityNotFoundException("CodeBase", codeBaseId);

        context.CodeBases.Update(codeBase);
        await context.SaveChangesAsync();
        return codeBase;
    }

    public async Task DeleteAsync(Guid codeBaseId)
    {
        var codeBase = await context.CodeBases.FindAsync(codeBaseId);
        if (codeBase == null) throw new DbEntityNotFoundException("CodeBase", codeBaseId);

        context.CodeBases.Remove(codeBase);
        await context.SaveChangesAsync();
    }
}