using System.Net.Http.Json;
using ExternalDomainEntities.UserDto.Query;

namespace ClientApp.Services;

public class UserService
{
    private readonly HttpClient _httpClient;
    private readonly string UserServiceEndpoint = "127.0.0.1:5003";

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ReadUserListResponse> GetDataAsync()
    {
        return await _httpClient.GetFromJsonAsync<ReadUserListResponse>($"{UserServiceEndpoint}/user/all");
    }
}