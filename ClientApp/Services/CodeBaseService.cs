using System.Net.Http.Json;
using System.Text;
using ExternalDomainEntities;
using ExternalDomainEntities.CodeBaseDto.Command;
using ExternalDomainEntities.CodeBaseDto.Query;
using Helpers;
using Microsoft.Extensions.Logging;

namespace ClientApp.Services;

public class CodeBaseService(HttpClient httpClient, ILogger<CodeBaseService> logger)
    : GenericService<ReadCodeBaseListByUserIdResponse, ReadCodeBaseResponse, ReadCodeBaseListRequest,
        CreateCodeBaseRequest, UpdateCodeBaseRequest>(httpClient, "http://localhost:5002/code-base", logger)
{
    public async Task<string?> FormatCodeAsync(string codeToFormat, string platformLanguage)
    {
        try
        {
            HttpClient formaterHttpClient = new HttpClient();
            var url = $"http://localhost:5004/format/{platformLanguage.ToLower()}";
            var content = new CodeRequest { Code = codeToFormat };

            HttpResponseMessage response =
                await formaterHttpClient.PostAsync(url, DeserializationHelper.CreateJsonContent(content));

            response.EnsureSuccessStatusCode();

            var codeResponse = await response.Content.ReadFromJsonAsync<CodeResponse>();
            return codeResponse?.Code;
        }
        catch (HttpRequestException httpEx)
        {
            logger.LogError(httpEx,
                "An HTTP request error occurred while formatting code for language {PlatformLanguage}",
                platformLanguage);
            throw new Exception("An error occurred while formatting the code. Please try again later.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unexpected error occurred while formatting code for language {PlatformLanguage}",
                platformLanguage);
            throw new Exception("An unexpected error occurred. Please try again later.");
        }
    }
}