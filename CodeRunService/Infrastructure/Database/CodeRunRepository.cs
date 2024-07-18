using Common.Exceptions;
using DomainEntities;
using Generics.BaseEntities;
using Generics.Pagination;
using Microsoft.EntityFrameworkCore;

namespace CodeRunService.Infrastructure.Database;

public class CodeRunRepository : BaseRepository<CodeRun>, ICodeRunRepository
{
    private readonly CodeRunDbContext _context;

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

// private readonly List<CodeRun> _codeRuns = new List<CodeRun>
//         {
//             new CodeRun
//             {
//                 Id = new Guid("123e4567-e89b-12d3-a456-426614174000"),
//                 CodeBaseId = new Guid("222e4567-e89b-12d3-a456-426614174000"),
//                 CodeBase = new CodeBase
//                 {
//                     Id = new Guid("222e4567-e89b-12d3-a456-426614174000"),
//                     Code = "Sample code 1",
//                     UserId = new Guid("111e4567-e89b-12d3-a456-426614174000"),
//                     User = new User
//                     {
//                         Id = new Guid("111e4567-e89b-12d3-a456-426614174000"),
//                         UserName = "testuser1",
//                         Password = "password1",
//                         Email = "testuser1@example.com",
//                         Name = "Test User 1",
//                         Language = "EN"
//                     }
//                 },
//                 Status = RunStatus.Ready,
//                 RunStart = DateTime.UtcNow,
//                 RunFinish = null,
//                 Results = null
//             },
//             new CodeRun
//             {
//                 Id = Guid.NewGuid(),
//                 CodeBaseId = new Guid("333e4567-e89b-12d3-a456-426614174000"),
//                 CodeBase = new CodeBase
//                 {
//                     Id = new Guid("333e4567-e89b-12d3-a456-426614174000"),
//                     Code = "Sample code 2",
//                     UserId = new Guid("222e4567-e89b-12d3-a456-426614174000"),
//                     User = new User
//                     {
//                         Id = new Guid("222e4567-e89b-12d3-a456-426614174000"),
//                         UserName = "testuser2",
//                         Password = "password2",
//                         Email = "testuser2@example.com",
//                         Name = "Test User 2",
//                         Language = "EN"
//                     }
//                 },
//                 Status = RunStatus.Running,
//                 RunStart = DateTime.UtcNow.AddHours(-1),
//                 RunFinish = null,
//                 Results = null
//             },
//             new CodeRun
//             {
//                 Id = Guid.NewGuid(),
//                 CodeBaseId = new Guid("444e4567-e89b-12d3-a456-426614174000"),
//                 CodeBase = new CodeBase
//                 {
//                     Id = new Guid("444e4567-e89b-12d3-a456-426614174000"),
//                     Code = "Sample code 3",
//                     UserId = new Guid("333e4567-e89b-12d3-a456-426614174000"),
//                     User = new User
//                     {
//                         Id = new Guid("333e4567-e89b-12d3-a456-426614174000"),
//                         UserName = "testuser3",
//                         Password = "password3",
//                         Email = "testuser3@example.com",
//                         Name = "Test User 3",
//                         Language = "EN"
//                     }
//                 },
//                 Status = RunStatus.Ready,
//                 RunStart = DateTime.UtcNow.AddHours(-2),
//                 RunFinish = DateTime.UtcNow.AddHours(-1),
//                 Results = new RunResult
//                 {
//                     Id = Guid.NewGuid(),
//                     File = null,
//                     ObjectRefId = ObjectId.GenerateNewId()
//                 }
//             },
//             new CodeRun
//             {
//                 Id = Guid.NewGuid(),
//                 CodeBaseId = new Guid("555e4567-e89b-12d3-a456-426614174000"),
//                 CodeBase = new CodeBase
//                 {
//                     Id = new Guid("555e4567-e89b-12d3-a456-426614174000"),
//                     Code = "Sample code 4",
//                     UserId = new Guid("444e4567-e89b-12d3-a456-426614174000"),
//                     User = new User
//                     {
//                         Id = new Guid("444e4567-e89b-12d3-a456-426614174000"),
//                         UserName = "testuser4",
//                         Password = "password4",
//                         Email = "testuser4@example.com",
//                         Name = "Test User 4",
//                         Language = "EN"
//                     }
//                 },
//                 Status = RunStatus.Ready,
//                 RunStart = DateTime.UtcNow.AddHours(-3),
//                 RunFinish = DateTime.UtcNow.AddHours(-2),
//                 Results = new RunResult
//                 {
//                     Id = Guid.NewGuid(),
//                     File = null,
//                     ObjectRefId = ObjectId.GenerateNewId()
//                 }
//             }
//         };