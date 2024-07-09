using AutoMapper;
using DomainEntities.Users.Query;
using DomainEntities.Users.Response;
using Generics.BaseEntities;
using Generics.Pagination;
using UsersService.Database;

namespace UsersService.Services;

public class UserQueryService : IUserQueryService
{
    private readonly ILogger<UserQueryService> _logger;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserQueryService(IMapper mapper, IUserRepository userRepository, ILogger<UserQueryService> logger)
    {
        _mapper = mapper;
        _userRepository = userRepository;
        _logger = logger;
    }

    public ReadUserListResponse GetAll(PaginationQuery paginationQuery)
    {
        _logger.LogInformation($"{nameof(UserQueryService)} {nameof(GetAll)}");

        var users = _userRepository.GetAll(paginationQuery);

        return new ReadUserListResponse
        {
            Items = new ItemWrapper<UserListResponseItem>
            {
                Values = _mapper.Map<List<UserListResponseItem>>(users)
            },
            Pagination = new PaginationResponse
            {
                PageNumber = paginationQuery.PageNumber,
                PageSize = paginationQuery.PageSize,
                ItemsCount = users.Count
            }
        };
    }

    public ReadUserResponse GetById(Guid entityId)
    {
        var user = _userRepository.GetById(entityId);
        return _mapper.Map<ReadUserResponse>(user);
    }
}