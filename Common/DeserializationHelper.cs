using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;

namespace Common;

public abstract class DeserializationHelper
{
    public static async Task<T> DeserializeResponse<T>(HttpResponseMessage response)
    {

        var responseContent = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var deserializedResponse = JsonSerializer.Deserialize<T>(responseContent, options);
        deserializedResponse.Should().NotBeNull();

        return deserializedResponse!;
    }

    public static StringContent CreateJsonContent(object obj)
    {
        return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, "application/json");
    }
}