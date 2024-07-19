using CodeBaseService.Infrastructure;
using Common.Exceptions;
using DomainEntities;
using Generics.BaseEntities;
using Generics.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CodebaseService.Infrastructure.Database;

public class CodeBaseRepository : BaseRepository<CodeBase>, ICodeBaseRepository
{
    private readonly CodeBaseDbContext _context;

    public CodeBaseRepository(CodeBaseDbContext context)
    {
        _context = context;
    }

    // Query-side operations
    public async Task<CodeBase?> GetByIdAsync(Guid codeBaseId)
    {
        return await _context.CodeBases
            .FirstOrDefaultAsync(cb => cb.Id == codeBaseId);
    }


    public async Task<List<CodeBase>> GetAllAsync(PaginationQuery paginationQuery)
    {
        var query = _context.CodeBases.AsQueryable();
        return await base.GetAllAsync(query, paginationQuery);
    }

    // Command-side operations
    public async Task<CodeBase> AddAsync(CodeBase codeBase)
    {
        codeBase.Id = Guid.NewGuid();
        await _context.CodeBases.AddAsync(codeBase);
        return await Task.FromResult(codeBase);
    }

    public async Task<CodeBase> UpdateAsync(Guid codeBaseId, CodeBase updatedCodeBase)
    {
        var codeBase = await _context.CodeBases.FindAsync(codeBaseId);
        if (codeBase == null) throw new DbEntityNotFoundException("CodeBase", codeBaseId);

        // Update properties as needed
        // codeBase.Property = updatedCodeBase.Property;

        _context.CodeBases.Update(codeBase);
        await _context.SaveChangesAsync();
        return codeBase;
    }

    public async Task DeleteAsync(Guid codeBaseId)
    {
        var codeBase = await _context.CodeBases.FindAsync(codeBaseId);
        if (codeBase == null) throw new DbEntityNotFoundException("CodeBase", codeBaseId);

        _context.CodeBases.Remove(codeBase);
        await _context.SaveChangesAsync();
    }
}