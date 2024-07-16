using CodebaseService.Infrastructure.Database;
using Common.Exceptions;
using DomainEntities;
using Generics.BaseEntities;
using Generics.Pagination;
using MongoDB.Bson;
using Newtonsoft.Json.Bson;

namespace CodebaseService.Infrastructure.Database
{
    public class CodebaseRepository : BaseRepository<Codebase>, ICodebaseRepository
    {
        private readonly List<Codebase> _codebases = new List<Codebase>
        {
            new Codebase
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
                },
                CodeRuns = new List<CodeRun>
                {
                    new CodeRun(id: new Guid("123e4567-e89b-12d3-a456-426614174000"),
                        codebaseId: new Guid("222e4567-e89b-12d3-a456-426614174000"), status: RunStatus.Ready,
                        runStart: DateTime.UtcNow, runFinish: null, results: new RunResult
                        {
                            Id = Guid.NewGuid(),
                            File = null,
                            ObjectRefId = ObjectId.GenerateNewId()
                        }, codebase: null)
                }
            },
            new Codebase
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
                },
                CodeRuns = new List<CodeRun>
                {
                    new CodeRun(id: Guid.NewGuid(), codebaseId: new Guid("333e4567-e89b-12d3-a456-426614174000"),
                        status: RunStatus.Running, runStart: DateTime.UtcNow.AddHours(-1), runFinish: null,
                        results: new RunResult
                        {
                            Id = Guid.NewGuid(),
                            File = new byte[]
                            {
                                0x01,
                                0x02,
                                0x03
                            },
                            ObjectRefId = ObjectId.GenerateNewId()

                        }, codebase: null)
                }
            },
            new Codebase
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
                },
                CodeRuns = new List<CodeRun>
                {
                    new CodeRun
                    {
                        Id = Guid.NewGuid(),
                        CodebaseId = new Guid("444e4567-e89b-12d3-a456-426614174000"),
                        Status = RunStatus.Done,
                        RunStart = DateTime.UtcNow.AddHours(-2),
                        RunFinish = DateTime.UtcNow.AddHours(-1),
                        Results = new RunResult
                        {
                            Id = Guid.NewGuid(),
                            File = new byte[]
                            {
                                0x04,
                                0x05,
                                0x06
                            },
                            ObjectRefId = ObjectId.GenerateNewId()

                        },
                        Codebase = null
                    }
                }
            },
            new Codebase
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
                },
                CodeRuns = new List<CodeRun>
                {
                    new CodeRun
                    {
                        Id = Guid.NewGuid(),
                        CodebaseId = new Guid("555e4567-e89b-12d3-a456-426614174000"),
                        Status = RunStatus.Ready,
                        RunStart = DateTime.UtcNow.AddHours(-3),
                        RunFinish = DateTime.UtcNow.AddHours(-2),
                        Results = new RunResult
                        {
                            Id = Guid.NewGuid(),
                            File = new byte[]
                            {
                                0x07,
                                0x08,
                                0x09
                            },
                            ObjectRefId = ObjectId.GenerateNewId()

                        },
                        Codebase = null
                    }
                }
            }
        };

        public async Task<List<Codebase?>> GetAllAsync(PaginationQuery paginationQuery)
        {
            var query = _codebases.AsQueryable();
            return await base.GetAllAsync(query, paginationQuery);
        }

        // Command-side operations
        public async Task<Codebase?> AddAsync(Codebase? user)
        {
            user.Id = Guid.NewGuid();
            _codebases.Add(user);
            return await Task.FromResult(user);
        }

        public async Task<Codebase> UpdateAsync(Guid userId, Codebase updatedCodebase)
        {
            var user = _codebases.FirstOrDefault(u => u.Id == userId);
            if (user == null) throw new DbEntityNotFoundException("Codebase", userId);

            // user.CodebaseName = updatedCodebase.CodebaseName;
            // user.Email = updatedCodebase.Email;
            // user.Name = updatedCodebase.Name;
            // user.Language = updatedCodebase.Language;
            return await Task.FromResult(user);
        }

        public async Task DeleteAsync(Guid userId)
        {
            var user = _codebases.FirstOrDefault(u => u.Id == userId);
            if (user == null) throw new DbEntityNotFoundException("Codebase", userId);

            _ = await Task.Run(() => _codebases.Remove(user));
        }

        // Query-side operations
        public async Task<Codebase?> GetByIdAsync(Guid userId)
        {
            var user = _codebases.FirstOrDefault(u => u.Id == userId);
            return await Task.FromResult(user);
        }
    }
}