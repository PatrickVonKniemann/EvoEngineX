using ExternalDomainEntities.CodeRunDto.Command;
using ExternalDomainEntities.CodeRunDto.Query;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace ClientApp.Services;

public class CodeRunService(HttpClient httpClient, ILogger<CodeRunService> logger)
    : GenericService<ReadCodeRunListByCodeBaseIdResponse, ReadCodeRunResponse, ReadCodeRunListRequest,
        CreateCodeRunRequest, UpdateCodeRunRequest>(httpClient, "http://localhost:5001/code-run", logger)
{
    // You can add any additional methods specific to CodeRunService here if needed
}