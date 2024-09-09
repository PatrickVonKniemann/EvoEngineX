using ExternalDomainEntities.CodeRunDto.Command;
using ExternalDomainEntities.CodeRunDto.Query;

namespace ClientApp.Services;

public class CodeRunService(HttpClient httpClient, ILogger<CodeRunService> logger)
    : GenericService<ReadCodeRunListByCodeBaseIdResponse, ReadCodeRunResponse, ReadCodeRunListRequest,
        CreateCodeRunRequest, UpdateCodeRunRequest>(httpClient, "http://localhost:5001/code-run", logger)
{
    // You can add any additional methods specific to CodeRunService here if needed
    // Method to call API and retrieve file bytes
    public async Task<byte[]> GetCodeRunFileAsync(Guid codeRunId)
    {
        var response = await httpClient.GetAsync($"http://localhost:5001/code-run/{codeRunId}/file");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadAsByteArrayAsync();
        }

        throw new Exception("Failed to download the file from the server");
    }
}