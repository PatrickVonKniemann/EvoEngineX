using System.Net.Http.Json;
using ExternalDomainEntities.CodeBaseDto.Command;
using ExternalDomainEntities.CodeBaseDto.Query;

namespace ClientApp.Services;

public class CodeBaseService(
    HttpClient httpClient,
    ILogger<CodeBaseService> logger, ServiceUrls serviceUrls)
    : GenericService<ReadCodeBaseListByUserIdResponse, ReadCodeBaseResponse, ReadCodeBaseListRequest,
        CreateCodeBaseRequest, UpdateCodeBaseRequest>(httpClient, $"{serviceUrls.CodeBaseServiceUrl}/code-base", logger)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task IncreaseSuccessFullRunCounter(Guid codeBaseId)
    {
        await _httpClient.PostAsJsonAsync($"{serviceUrls.CodeBaseServiceUrl}/code-base/increase", codeBaseId);
    }
}