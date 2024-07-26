using System.Net.Http.Json;
using ExternalDomainEntities.UserDto.Query;

namespace ClientApp.Services;

public class UserService
{
    private readonly HttpClient _httpClient;
    private const string UserServiceEndpoint = "http://localhost:5003";

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ReadUserListResponse?> GetDataAsync()
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<ReadUserListResponse>($"{UserServiceEndpoint}/user/all?pageNumber=1&pageSize=10&sortBy=name&sortOrder=asc");
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Request error: {ex.Message}");
            return null;
        }
    }
}