using AutoMapper;
using DomainEntities.Users;
using DomainEntities.Users.Query;
using DomainEntities.Users.Response;
using Generics.BaseEntities;
using Generics.Pagination;
using UsersService.Database;


namespace UsersService.Services
{
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

        public async Task<ReadUserListResponse> GetAllAsync(PaginationQuery paginationQuery)
        {
            _logger.LogInformation($"{nameof(UserQueryService)} {nameof(GetAllAsync)}");

            var users = await _userRepository.GetAllAsync(paginationQuery);

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

        public async Task<ReadUserResponse> GetByIdAsync(Guid entityId)
        {
            var user = await _userRepository.GetByIdAsync(entityId);
            return _mapper.Map<ReadUserResponse>(user);
        }
    }
}
