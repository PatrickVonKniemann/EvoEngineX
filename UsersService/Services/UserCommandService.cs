using AutoMapper;
using DomainEntities.Users;
using DomainEntities.Users.Request;
using DomainEntities.Users.Response;

namespace UsersService.Services;

public class UserCommandService : IUserCommandService
{
    private readonly ILogger<UserCommandService> _logger;
    private readonly IMapper _mapper;

    private readonly List<User> _users = new()
    {
        new User
        {
            Id = Guid.NewGuid(),
            UserName = "john_doe",
            Email = "john.doe@example.com",
            Name = "John Doe",
            Language = "English"
        },
        new User
        {
            Id = Guid.NewGuid(),
            UserName = "jane_smith",
            Email = "jane.smith@example.com",
            Name = "Jane Smith",
            Language = "English"
        },
        new User
        {
            Id = Guid.NewGuid(),
            UserName = "maria_garcia",
            Email = "maria.garcia@example.com",
            Name = "Maria Garcia",
            Language = "Spanish"
        }
    };


    public UserCommandService(
        ILogger<UserCommandService> logger,
        IMapper mapper
    )
    {
        _logger = logger;
        _mapper = mapper;
    }

    public CreateUserResponse Add(CreateUserRequest entityRequest)
    {
        _users.Add(_mapper.Map<User>(entityRequest));
        return _mapper.Map<CreateUserResponse>(entityRequest);
    }

    public UpdateUserResponse Update(Guid entityId, UpdateUserRequest entityRequest)
    {
        var user = _users.FirstOrDefault(u => u.Id == entityId);
        if (user != null)
        {
            user.Email = entityRequest.Email ?? user.Email;
            user.Name = entityRequest.Name;
            user.Language = entityRequest.Language;
        }


        return _mapper.Map<UpdateUserResponse>(user);
    }

    public void Delete(Guid entityId)
    {
        var user = _users.FirstOrDefault(u => u.Id == entityId);
        if (user != null)
        {
            _users.Remove(user);
        }
    }
}