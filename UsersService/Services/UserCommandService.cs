using AutoMapper;
using DomainEntities.Users;
using DomainEntities.Users.Command;
using DomainEntities.Users.Response;
using UsersService.Database;

namespace UsersService.Services;

public class UserCommandService : IUserCommandService
{
    private readonly ILogger<UserCommandService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserCommandService(
        ILogger<UserCommandService> logger,
        IMapper mapper, IUserRepository userRepository)
    {
        _logger = logger;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<CreateUserResponse> AddAsync(CreateUserRequest entityRequest)
    {
        _logger.LogInformation($"{nameof(UserCommandService)} {nameof(AddAsync)}");
        var user = await _userRepository.AddAsync(_mapper.Map<User>(entityRequest));
        return _mapper.Map<CreateUserResponse>(user);
    }

    public async Task<UpdateUserResponse> UpdateAsync(Guid entityId, UpdateUserRequest entityRequest)
    {
        _logger.LogInformation($"{nameof(UserCommandService)} {nameof(UpdateAsync)}");
        var updatedUser = await _userRepository.UpdateAsync(entityId, _mapper.Map<User>(entityRequest));
        return _mapper.Map<UpdateUserResponse>(updatedUser);
    }

    public async Task DeleteAsync(Guid entityId)
    {
        _logger.LogInformation($"{nameof(UserCommandService)} {nameof(DeleteAsync)}");
        await _userRepository.DeleteAsync(entityId);
    }
}