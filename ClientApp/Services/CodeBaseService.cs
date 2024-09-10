using System.Net.Http.Json;
using ExternalDomainEntities.CodeBaseDto.Command;
using ExternalDomainEntities.CodeBaseDto.Query;

namespace ClientApp.Services;

public class CodeBaseService(
    HttpClient httpClient,
    ILogger<CodeBaseService> logger)
    : GenericService<ReadCodeBaseListByUserIdResponse, ReadCodeBaseResponse, ReadCodeBaseListRequest,
        CreateCodeBaseRequest, UpdateCodeBaseRequest>(httpClient, $"{ServiceUrls.CodeBaseServiceUrl}/code-base", logger)
{
    public async Task IncreaseSuccessFullRunCounter(Guid codeBaseId)
    {
        await httpClient.PostAsJsonAsync($"{ServiceUrls.CodeBaseServiceUrl}/code-base/increase", codeBaseId);
    }
}