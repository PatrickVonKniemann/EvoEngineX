using ExternalDomainEntities.UserDto.Command;
using ExternalDomainEntities.UserDto.Query;

namespace ClientApp.Services;

public class UserService(HttpClient httpClient, ILogger<UserService> logger, ServiceUrls serviceUrls)
    : GenericService<ReadUserListResponse, ReadUserResponse, ReadUserListRequest, CreateUserRequest, UpdateUserRequest>(
        httpClient, $"{serviceUrls.UserServiceUrl}/user", logger);