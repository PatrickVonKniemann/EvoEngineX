using ExternalDomainEntities.UserDto.Query;

namespace ClientApp.Services;

public class UserService(HttpClient httpClient) : GenericService<ReadUserListResponse, ReadUserResponse, ReadUserListRequest>(httpClient, "http://localhost:5003/user");