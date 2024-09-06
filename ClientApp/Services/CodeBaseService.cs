using System.Net.Http.Json;
using System.Text;
using ExternalDomainEntities;
using ExternalDomainEntities.CodeBaseDto.Command;
using ExternalDomainEntities.CodeBaseDto.Query;
using Helpers;
using Microsoft.Extensions.Logging;

namespace ClientApp.Services;

public class CodeBaseService(
    HttpClient httpClient,
    ILogger<CodeBaseService> logger)
    : GenericService<ReadCodeBaseListByUserIdResponse, ReadCodeBaseResponse, ReadCodeBaseListRequest,
        CreateCodeBaseRequest, UpdateCodeBaseRequest>(httpClient, "http://localhost:5002/code-base", logger)
{
    public async Task IncreaseSuccessFullRunCounter(Guid codeBaseId)
    {
        await httpClient.PostAsJsonAsync("http://localhost:5002/code-base/increase", codeBaseId);
    }
}