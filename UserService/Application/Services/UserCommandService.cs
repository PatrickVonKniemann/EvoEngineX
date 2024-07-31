using AutoMapper;
using DomainEntities;
using ExternalDomainEntities.UserDto.Command;
using Generics.BaseEntities;
using UserService.Infrastructure.Database;

namespace UserService.Application.Services
{
    public class UserCommandService(
        ILogger<UserCommandService> logger,
        IMapper mapper,
        IUserRepository userRepository)
        : BaseCommandService<User, CreateUserRequest, CreateUserResponse, UpdateUserRequest,
            UpdateUserResponse>(mapper, userRepository, logger), IUserCommandService
    {
        // Any additional methods specific to UserCommandService can go here
    }
}