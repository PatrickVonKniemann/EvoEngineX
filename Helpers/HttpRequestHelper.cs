using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace Helpers;

public static class HttpRequestHelper
{
    public static HttpRequestMessage CreateGetRequestWithQuery(string baseUrl, string relativeUrl, Dictionary<string, string> queryParams)
    {
        var uriBuilder = new UriBuilder(new Uri(new Uri(baseUrl), relativeUrl));
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        foreach (var param in queryParams)
        {
            query[param.Key] = param.Value;
        }
        uriBuilder.Query = query.ToString();
    
        return new HttpRequestMessage(HttpMethod.Get, uriBuilder.Uri);
    }


}