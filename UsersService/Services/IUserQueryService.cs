using DomainEntities.Users.Query;
using DomainEntities.Users.Response;
using Generics.BaseEntities;

namespace UsersService.Services;

public interface IUserQueryService : IGenericEntityQueryService<ReadUserResponse, ReadUserListResponse>
{
    
}