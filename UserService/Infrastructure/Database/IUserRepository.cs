using DomainEntities;
using Generics.BaseEntities;

namespace UserService.Infrastructure.Database;

public interface IUserRepository : IRepository<User>;