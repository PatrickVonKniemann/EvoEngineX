using System.Net.Http.Json;
using ExternalDomainEntities.UserDto.Query;
using Generics.Pagination;

namespace ClientApp.Services;

public class UserService(HttpClient httpClient)
{
    private const string UserServiceEndpoint = "http://localhost:5003";

    public async Task<ReadUserListResponse?> GetDataAsync(int pageNumber = 1, int pageSize = 10)
    {
        var paginationQuery = new PaginationQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var readUserListRequest = new ReadUserListRequest
        {
            PaginationQuery = paginationQuery
        };

        var response = await httpClient.PostAsJsonAsync($"{UserServiceEndpoint}/user/all", readUserListRequest);
        return await response.Content.ReadFromJsonAsync<ReadUserListResponse>();
    }
}