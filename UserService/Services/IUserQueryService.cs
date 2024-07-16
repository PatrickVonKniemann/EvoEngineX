using DomainEntities.UserDto.Query;
using Generics.BaseEntities;

namespace UserService.Services;

public interface IUserQueryService : IGenericEntityQueryService<ReadUserResponse, ReadUserListResponse>
{
    
}