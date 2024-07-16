using Common.Exceptions;
using DomainEntities;
using Generics.BaseEntities;
using Generics.Pagination;
using CodeRunService.Infrastructure.Database;
using MongoDB.Bson;
using Newtonsoft.Json.Bson;

namespace CodeRunService.Infrastructure.Database
{
    public class CodeRunRepository : BaseRepository<CodeRun>, ICodeRunRepository
    {
        private readonly List<CodeRun> _codeRuns = new List<CodeRun>
        {
            new CodeRun
            {
                Id = new Guid("123e4567-e89b-12d3-a456-426614174000"),
                CodebaseId = new Guid("222e4567-e89b-12d3-a456-426614174000"),
                Codebase = new Codebase
                {
                    Id = new Guid("222e4567-e89b-12d3-a456-426614174000"),
                    Code = "Sample code 1",
                    UserId = new Guid("111e4567-e89b-12d3-a456-426614174000"),
                    User = new User
                    {
                        Id = new Guid("111e4567-e89b-12d3-a456-426614174000"),
                        UserName = "testuser1",
                        Password = "password1",
                        Email = "testuser1@example.com",
                        Name = "Test User 1",
                        Language = "EN"
                    }
                },
                Status = RunStatus.Ready,
                RunStart = DateTime.UtcNow,
                RunFinish = null,
                Results = null
            },
            new CodeRun
            {
                Id = Guid.NewGuid(),
                CodebaseId = new Guid("333e4567-e89b-12d3-a456-426614174000"),
                Codebase = new Codebase
                {
                    Id = new Guid("333e4567-e89b-12d3-a456-426614174000"),
                    Code = "Sample code 2",
                    UserId = new Guid("222e4567-e89b-12d3-a456-426614174000"),
                    User = new User
                    {
                        Id = new Guid("222e4567-e89b-12d3-a456-426614174000"),
                        UserName = "testuser2",
                        Password = "password2",
                        Email = "testuser2@example.com",
                        Name = "Test User 2",
                        Language = "EN"
                    }
                },
                Status = RunStatus.Running,
                RunStart = DateTime.UtcNow.AddHours(-1),
                RunFinish = null,
                Results = null
            },
            new CodeRun
            {
                Id = Guid.NewGuid(),
                CodebaseId = new Guid("444e4567-e89b-12d3-a456-426614174000"),
                Codebase = new Codebase
                {
                    Id = new Guid("444e4567-e89b-12d3-a456-426614174000"),
                    Code = "Sample code 3",
                    UserId = new Guid("333e4567-e89b-12d3-a456-426614174000"),
                    User = new User
                    {
                        Id = new Guid("333e4567-e89b-12d3-a456-426614174000"),
                        UserName = "testuser3",
                        Password = "password3",
                        Email = "testuser3@example.com",
                        Name = "Test User 3",
                        Language = "EN"
                    }
                },
                Status = RunStatus.Ready,
                RunStart = DateTime.UtcNow.AddHours(-2),
                RunFinish = DateTime.UtcNow.AddHours(-1),
                Results = new RunResult
                {
                    Id = Guid.NewGuid(),
                    File = null,
                    ObjectRefId = ObjectId.GenerateNewId()
                }
            },
            new CodeRun
            {
                Id = Guid.NewGuid(),
                CodebaseId = new Guid("555e4567-e89b-12d3-a456-426614174000"),
                Codebase = new Codebase
                {
                    Id = new Guid("555e4567-e89b-12d3-a456-426614174000"),
                    Code = "Sample code 4",
                    UserId = new Guid("444e4567-e89b-12d3-a456-426614174000"),
                    User = new User
                    {
                        Id = new Guid("444e4567-e89b-12d3-a456-426614174000"),
                        UserName = "testuser4",
                        Password = "password4",
                        Email = "testuser4@example.com",
                        Name = "Test User 4",
                        Language = "EN"
                    }
                },
                Status = RunStatus.Ready,
                RunStart = DateTime.UtcNow.AddHours(-3),
                RunFinish = DateTime.UtcNow.AddHours(-2),
                Results = new RunResult
                {
                    Id = Guid.NewGuid(),
                    File = null,
                    ObjectRefId = ObjectId.GenerateNewId()
                }
            }
        };

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