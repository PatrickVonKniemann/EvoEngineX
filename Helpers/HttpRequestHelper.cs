using System.Net.Http.Json;

namespace Helpers;

public static class HttpRequestHelper
{
    public static HttpRequestMessage CreateGetRequestWithBody(string url, object body)
    {
        if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            throw new UriFormatException("The provided URL must be relative.");
        }

        var request = new HttpRequestMessage(HttpMethod.Get, new Uri(url, UriKind.Relative));
        request.Content = JsonContent.Create(body);
        return request;
    }
}