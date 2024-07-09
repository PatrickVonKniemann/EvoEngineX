
using DomainEntities.Users.Command;
using DomainEntities.Users.Response;
using Generics.BaseEntities;

namespace UsersService.Services;

public interface IUserCommandService : IGenericCommandService<CreateUserRequest, UpdateUserRequest, CreateUserResponse, UpdateUserResponse>
{
    
}