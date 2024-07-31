using DomainEntities;
using Generics.BaseEntities;

namespace UserService.Infrastructure.Database;

public class UserRepository(UserDbContext context) : BaseRepository<User>(context), IUserRepository;