using Common;
using UserService.Infrastructure;

namespace UserService.Tests
{
    public class UserServiceWebApplicationFactory<TStartup>()
        : CustomWebApplicationFactory<TStartup, UserDbContext>("UserServiceDb", new List<string> { "Users" },
            "../../../../Configs/SqlScripts")
        where TStartup : class;
}