using System.Text;
using ExternalDomainEntities.CodeBaseDto.Command;
using ExternalDomainEntities.CodeBaseDto.Query;

namespace ClientApp.Services;

public class CodeBaseService(HttpClient httpClient) : GenericService<ReadCodeBaseListByUserIdResponse, ReadCodeBaseResponse, ReadCodeBaseListRequest, CreateCodeBaseRequest, UpdateCodeBaseRequest>(httpClient, "http://localhost:5002/code-base")
{
    public async Task<string> FormatCodeAsync(string codeToFormat, string platformLanguage)
    {
        HttpClient formaterHttpClient = new HttpClient();
        var url = $"http://localhost:5004/{platformLanguage}";
        var content = new StringContent(codeToFormat, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await formaterHttpClient.PostAsync(url, content);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}