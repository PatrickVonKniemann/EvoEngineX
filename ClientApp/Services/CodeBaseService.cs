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
    ILogger<CodeBaseService> logger,
    NotificationService notificationService)
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
            var msg = $"An HTTP request error occurred while formatting code for language {platformLanguage}";
            logger.LogError(httpEx, msg);
            notificationService.ShowMessage(msg);
            throw new Exception("An unexpected error occurred. Please try again later.");
        }
        catch (Exception ex)
        {
            var msg = $"An HTTP request error occurred while formatting code for language {platformLanguage}";
            logger.LogError(ex, msg);
            notificationService.ShowMessage(msg);
            throw new Exception("An unexpected error occurred. Please try again later.");
        }
    }
}