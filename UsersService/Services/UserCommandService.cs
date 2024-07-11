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

    public CreateUserResponse Add(CreateUserRequest entityRequest)
    {
        _logger.LogInformation($"{nameof(UserCommandService)} {nameof(Add)}");
        _userRepository.Add(_mapper.Map<User>(entityRequest));
        return _mapper.Map<CreateUserResponse>(entityRequest);
    }

    public UpdateUserResponse Update(Guid entityId, UpdateUserRequest entityRequest)
    {
        _logger.LogInformation($"{nameof(UserCommandService)} {nameof(Update)}");
        var updatedUser = _userRepository.Update(entityId, _mapper.Map<User>(entityRequest));
        return _mapper.Map<UpdateUserResponse>(updatedUser);
    }

    public void Delete(Guid entityId)
    {
        _logger.LogInformation($"{nameof(UserCommandService)} {nameof(Delete)}");
        _userRepository.Delete(entityId);
    }
}