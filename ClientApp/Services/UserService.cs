using ExternalDomainEntities.UserDto.Query;

namespace ClientApp.Services;

public class UserService
{
    private readonly HttpClient _httpClient;
    private readonly UserServiceEndpoint = "127.0.0.1:5003"

    public UserService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ReadUserListResponse> GetDataAsync()
    {
        return await _httpClient.GetFromJsonAsync<ReadUserListResponse>("api/your-endpoint");
    }

    public async Task PostDataAsync(YourDataType data)
    {
        await _httpClient.PostAsJsonAsync("api/your-endpoint", data);
    }
}