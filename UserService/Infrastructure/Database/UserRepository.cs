using DomainEntities;
using Generics.BaseEntities;

namespace UserService.Infrastructure.Database;

public class UserRepository(UserDbContext context, ILogger<UserRepository> logger) : BaseRepository<User>(context, logger), IUserRepository;