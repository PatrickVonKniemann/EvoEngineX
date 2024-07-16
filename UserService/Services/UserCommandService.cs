using AutoMapper;
using DomainEntities;
using DomainEntities.UserDto.Command;
using UserService.Database;

namespace UserService.Services;

public class UserCommandService(
    ILogger<UserCommandService> logger,
    IMapper mapper,
    IUserRepository userRepository)
    : IUserCommandService
{
    public async Task<CreateUserResponse> AddAsync(CreateUserRequest entityRequest)
    {
        logger.LogInformation($"{nameof(UserCommandService)} {nameof(AddAsync)}");
        var user = await userRepository.AddAsync(mapper.Map<User>(entityRequest));
        return mapper.Map<CreateUserResponse>(user);
    }

    public async Task<UpdateUserResponse> UpdateAsync(Guid entityId, UpdateUserRequest entityRequest)
    {
        logger.LogInformation($"{nameof(UserCommandService)} {nameof(UpdateAsync)}");
        var updatedUser = await userRepository.UpdateAsync(entityId, mapper.Map<User>(entityRequest));
        return mapper.Map<UpdateUserResponse>(updatedUser);
    }

    public async Task DeleteAsync(Guid entityId)
    {
        logger.LogInformation($"{nameof(UserCommandService)} {nameof(DeleteAsync)}");
        await userRepository.DeleteAsync(entityId);
    }
}