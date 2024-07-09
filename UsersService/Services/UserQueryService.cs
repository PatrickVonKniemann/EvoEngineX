using AutoMapper;
using DomainEntities.Users.Query;
using DomainEntities.Users.Response;
using Generics.BaseEntities;
using Generics.Pagination;
using UsersService.Database;

namespace UsersService.Services;

public class UserQueryService : IUserQueryService
{
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;

    public UserQueryService(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public ReadUserListResponse GetAll(PaginationQuery paginationQuery)
    {
        paginationQuery ??= new PaginationQuery();

        var (paginatedUsers, totalCount) = _userRepository.GetAll(paginationQuery);

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
                ItemsCount = totalCount
            }
        };
    }
    
    public ReadUserResponse GetById(Guid entityId)
    {
        var user = _userRepository.GetById(entityId);
        return _mapper.Map<ReadUserResponse>(user);
    }
}