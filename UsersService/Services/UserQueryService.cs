using AutoMapper;
using DomainEntities.Users.Response;
using Generics.BaseEntities;
using Generics.Pagination;

namespace UsersService.Services;

public class UserQueryService : IUserQueryService
{
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

    public UserQueryService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public ReadUserListResponse? GetAll(PaginationQuery? paginationQuery)
    {
        if (paginationQuery == null)
        {
            paginationQuery = new PaginationQuery();
        }

        var paginatedUsers = _users
            .Skip((paginationQuery.PageNumber - 1) * paginationQuery.PageSize)
            .Take(paginationQuery.PageSize)
            .ToList();

        var totalItems = _users.Count;

        return new ReadUserListResponse
        {
            Items = new ItemWrapper<UserListResponseItem>
            {
                Values = _mapper.Map<List<UserListResponseItem>>(paginatedUsers)
            },
            Pagination = new PaginationResponse
            {
                PageNumber = paginationQuery.PageNumber,
                PageSize = paginationQuery.PageSize,
                ItemsCount = totalItems
            }
        };
    }

    public ReadUserResponse GetById(Guid entityId)
    {
        var user = _users.FirstOrDefault(u => u.Id == entityId);
        return _mapper.Map<ReadUserResponse>(user);
    }
}