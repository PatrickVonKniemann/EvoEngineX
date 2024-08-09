using System.Net.Http.Json;
using System.Text;
using ExternalDomainEntities;
using ExternalDomainEntities.CodeBaseDto.Command;
using ExternalDomainEntities.CodeBaseDto.Query;
using Helpers;

namespace ClientApp.Services;

public class CodeBaseService(HttpClient httpClient)
    : GenericService<ReadCodeBaseListByUserIdResponse, ReadCodeBaseResponse, ReadCodeBaseListRequest,
        CreateCodeBaseRequest, UpdateCodeBaseRequest>(httpClient, "http://localhost:5002/code-base")
{
    public async Task<string?> FormatCodeAsync(string codeToFormat, string platformLanguage)
    {
        HttpClient formaterHttpClient = new HttpClient();
        var url = $"http://localhost:5004/format/{platformLanguage.ToLower()}";
        var content = new CodeRequest { Code = codeToFormat };
        HttpResponseMessage response =
            await formaterHttpClient.PostAsync(url, DeserializationHelper.CreateJsonContent(content));

        response.EnsureSuccessStatusCode();
        try
        {
            var codeResponse = await response.Content.ReadFromJsonAsync<CodeResponse>();
            return codeResponse.Code;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}