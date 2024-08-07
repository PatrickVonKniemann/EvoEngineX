using ExternalDomainEntities.UserDto.Command;
using Generics.BaseEntities;

namespace UserService.Application.Services;

public interface IUserCommandService : IGenericEntityCommandService<CreateUserRequest, CreateUserResponse,
    UpdateUserRequest, UpdateUserResponse>;