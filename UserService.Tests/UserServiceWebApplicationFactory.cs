using Common;
using UserService.Infrastructure;
using DomainEntities;

namespace UserService.Tests;

public class UserServiceWebApplicationFactory<TStartup>()
    : CustomWebApplicationFactory<TStartup, UserDbContext>()
    where TStartup : class;