using System.Net.Http.Json;
using ExternalDomainEntities;
using Helpers;

namespace ClientApp.Services;

public class CodeFormatService(
    HttpClient httpClient,
    ILogger<CodeFormatService> logger,
    NotificationService notificationService)
{
    private const string BaseUrl = "http://localhost:5004/format";

    public async Task<string?> FormatCodeAsync(string codeToFormat, string platformLanguage)
    {
        return await ProcessRequestAsync<string?>(codeToFormat, platformLanguage, "formatting");
    }

    public async Task<bool> ValidateFormatAsync(string codeToValidate, string platformLanguage)
    {
        return await ProcessRequestAsync<bool>(codeToValidate, platformLanguage, "validating");
    }

    private async Task<T> ProcessRequestAsync<T>(string code, string platformLanguage, string operation)
    {
        try
        {
            var url = $"{BaseUrl}/{platformLanguage.ToLower()}";
            var content = new CodeRequest { Code = code };

            var response = await httpClient.PostAsync(url, DeserializationHelper.CreateJsonContent(content));
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<T>();
            return result;
        }
        catch (HttpRequestException httpEx)
        {
            var msg = $"An HTTP request error occurred while {operation} code for language {platformLanguage}";
            logger.LogError(httpEx, msg);
            notificationService.ShowMessage(msg);
            throw new Exception("An unexpected error occurred. Please try again later.");
        }
        catch (Exception ex)
        {
            var msg = $"An error occurred while {operation} code for language {platformLanguage}";
            logger.LogError(ex, msg);
            notificationService.ShowMessage(msg);
            throw new Exception("An unexpected error occurred. Please try again later.");
        }
    }
}
