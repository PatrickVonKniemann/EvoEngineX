using AutoMapper;
using DomainEntities;
using ExternalDomainEntities.UserDto.Query;
using Generics.BaseEntities;
using UserService.Infrastructure.Database;

namespace UserService.Application.Services;

public class UserQueryService(IMapper mapper, IUserRepository userRepository, ILogger<UserQueryService> logger)
    : BaseQueryService<User, ReadUserResponse, UserListResponseItem, ReadUserListResponse>(mapper, userRepository,
        logger), IUserQueryService
{
    // Any additional methods specific to UserQueryService can go here
}