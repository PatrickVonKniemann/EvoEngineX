
using DomainEntities.UserDto.Command;
using Generics.BaseEntities;

namespace UserService.Services;

public interface IUserCommandService : IGenericCommandService<CreateUserRequest, UpdateUserRequest, CreateUserResponse, UpdateUserResponse>
{
    
}