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

// private readonly List<CodeBase> _codebases = new List<CodeBase>
// {
//     new CodeBase
//     {
//         Id = new Guid("222e4567-e89b-12d3-a456-426614174000"),
//         Code = "Sample code 1",
//         UserId = new Guid("111e4567-e89b-12d3-a456-426614174000"),
//         User = new User
//         {
//             Id = new Guid("111e4567-e89b-12d3-a456-426614174000"),
//             UserName = "testuser1",
//             Password = "password1",
//             Email = "testuser1@example.com",
//             Name = "Test User 1",
//             Language = "EN"
//         },
//         CodeRuns = new List<CodeRun>
//         {
//         }
//     },
//     new CodeBase
//     {
//         Id = new Guid("333e4567-e89b-12d3-a456-426614174000"),
//         Code = "Sample code 2",
//         UserId = new Guid("222e4567-e89b-12d3-a456-426614174000"),
//         User = new User
//         {
//             Id = new Guid("222e4567-e89b-12d3-a456-426614174000"),
//             UserName = "testuser2",
//             Password = "password2",
//             Email = "testuser2@example.com",
//             Name = "Test User 2",
//             Language = "EN"
//         },
//         CodeRuns = new List<CodeRun>
//         {
//         }
//     },
//     new CodeBase
//     {
//         Id = new Guid("444e4567-e89b-12d3-a456-426614174000"),
//         Code = "Sample code 3",
//         UserId = new Guid("333e4567-e89b-12d3-a456-426614174000"),
//         User = new User
//         {
//             Id = new Guid("333e4567-e89b-12d3-a456-426614174000"),
//             UserName = "testuser3",
//             Password = "password3",
//             Email = "testuser3@example.com",
//             Name = "Test User 3",
//             Language = "EN"
//         },
//         CodeRuns = new List<CodeRun>
//         {
//             new CodeRun
//             {
//                 Id = Guid.NewGuid(),
//                 CodeBaseId = new Guid("444e4567-e89b-12d3-a456-426614174000"),
//                 Status = RunStatus.Done,
//                 RunStart = DateTime.UtcNow.AddHours(-2),
//                 RunFinish = DateTime.UtcNow.AddHours(-1),
//                 Results = new RunResult
//                 {
//                     Id = Guid.NewGuid(),
//                     File = new byte[]
//                     {
//                         0x04,
//                         0x05,
//                         0x06
//                     },
//                     ObjectRefId = ObjectId.GenerateNewId()
//
//                 },
//                 CodeBase = null
//             }
//         }
//     },
//     new CodeBase
//     {
//         Id = new Guid("555e4567-e89b-12d3-a456-426614174000"),
//         Code = "Sample code 4",
//         UserId = new Guid("444e4567-e89b-12d3-a456-426614174000"),
//         User = new User
//         {
//             Id = new Guid("444e4567-e89b-12d3-a456-426614174000"),
//             UserName = "testuser4",
//             Password = "password4",
//             Email = "testuser4@example.com",
//             Name = "Test User 4",
//             Language = "EN"
//         },
//         CodeRuns = new List<CodeRun>
//         {
//             new CodeRun
//             {
//                 Id = Guid.NewGuid(),
//                 CodeBaseId = new Guid("555e4567-e89b-12d3-a456-426614174000"),
//                 Status = RunStatus.Ready,
//                 RunStart = DateTime.UtcNow.AddHours(-3),
//                 RunFinish = DateTime.UtcNow.AddHours(-2),
//                 Results = new RunResult
//                 {
//                     Id = Guid.NewGuid(),
//                     File = new byte[]
//                     {
//                         0x07,
//                         0x08,
//                         0x09
//                     },
//                     ObjectRefId = ObjectId.GenerateNewId()
//
//                 },
//                 CodeBase = null
//             }
//         }
//     }
// };