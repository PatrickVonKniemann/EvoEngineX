using ExternalDomainEntities.CodeRunDto.Command;
using ExternalDomainEntities.CodeRunDto.Query;

namespace ClientApp.Services;

public class CodeRunService(
    HttpClient httpClient,
    ILogger<CodeRunService> logger,
    ServiceUrls serviceUrls)
    : GenericService<ReadCodeRunListByCodeBaseIdResponse, ReadCodeRunResponse, ReadCodeRunListRequest,
        CreateCodeRunRequest, UpdateCodeRunRequest>(httpClient, $"{serviceUrls.CodeRunServiceUrl}/code-run", logger)
{
    private readonly HttpClient _httpClient = httpClient;

    // You can add any additional methods specific to CodeRunService here if needed
    // Method to call API and retrieve file bytes
    public async Task<byte[]> GetCodeRunFileAsync(Guid codeRunId)
    {
        var response = await _httpClient.GetAsync($"{serviceUrls.CodeRunServiceUrl}/code-run/{codeRunId}/file");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsByteArrayAsync();
        }

        throw new Exception("Failed to download the file from the server");
    }
}