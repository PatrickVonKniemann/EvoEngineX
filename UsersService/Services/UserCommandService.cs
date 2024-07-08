using AutoMapper;
using DomainEntities.Users.Request;
using DomainEntities.Users.Response;

namespace UsersService.Services;

public class UserCommandService : IUserCommandService
{
    
    private readonly ILogger<UserCommandService> _logger;
    private readonly IMapper _mapper;
    private readonly List<ReadUserResponse> _users = new()
    {
        new ReadUserResponse
        {
            Id = Guid.NewGuid(),
            UserName = "john_doe",
            Email = "john.doe@example.com",
            Name = "John Doe",
            Language = "English"
        },
        new ReadUserResponse
        {
            Id = Guid.NewGuid(),
            UserName = "jane_smith",
            Email = "jane.smith@example.com",
            Name = "Jane Smith",
            Language = "English"
        },
        new ReadUserResponse
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
        var newUser = new ReadUserResponse
        {
            Id = Guid.NewGuid(),
            UserName = entityRequest.UserName,
            Email = entityRequest.Email,
            Name = entityRequest.Name,
            Language = entityRequest.Language
        };

        _users.Add(newUser);
        return _mapper.Map<CreateUserResponse>(newUser);
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