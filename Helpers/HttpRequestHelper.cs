using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Helpers;

public static class HttpRequestHelper
{
    public static HttpRequestMessage CreateGetRequestWithBody(string url, object body)
    {
        var content = new StringContent(JsonConvert.SerializeObject(body), Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri(url, UriKind.Relative),
            Content = content
        };
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
        return request;
    }
}
