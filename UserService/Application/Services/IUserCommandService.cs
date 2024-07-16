using DomainEntities.UserDto.Command;
using Generics.BaseEntities;

namespace UserService.Application.Services;

public interface IUserCommandService : IGenericCommandService<CreateUserRequest, UpdateUserRequest, CreateUserResponse, UpdateUserResponse>
{
    
}