using System.Net.Http.Json;
using Generics.Pagination;
using Helpers;

namespace ClientApp.Services
{
    public class GenericService<TListResponse, TEntityResponse, TQueryRequest, TCreateRequest, TUpdateRequest>(
        HttpClient httpClient,
        string serviceEndpoint)
    {
        public async Task<TListResponse?> GetDataAsync(string subPath, int pageNumber = 1, int pageSize = 10)
        {
            var paginationQuery = new PaginationQuery
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var queryRequest = Activator.CreateInstance<TQueryRequest>();
            var paginationProperty = typeof(TQueryRequest).GetProperty("PaginationQuery");
            if (paginationProperty != null && paginationProperty.CanWrite)
            {
                paginationProperty.SetValue(queryRequest, paginationQuery);
            }

            var response = await httpClient.PostAsJsonAsync($"{serviceEndpoint}/{subPath}", queryRequest);
            return await response.Content.ReadFromJsonAsync<TListResponse>();
        }

        public async Task<TEntityResponse?> GetEntityAsync(string entityId)
        {
            var response = await httpClient.GetAsync($"{serviceEndpoint}/{entityId}");
            return await response.Content.ReadFromJsonAsync<TEntityResponse>();
        }

        public async Task AddEntityAsync(TCreateRequest request)
        {
            var response = await httpClient.PostAsJsonAsync($"{serviceEndpoint}/add", request);
            response.EnsureSuccessStatusCode();
        }

        public async Task UpdateEntityAsync(string entityId, TUpdateRequest request)
        {
            var content =
                DeserializationHelper.CreateJsonContent(request ?? throw new ArgumentNullException(nameof(request)));
            var response = await httpClient.PatchAsync($"{serviceEndpoint}/{entityId}", content);
            response.EnsureSuccessStatusCode();
        }
    }
}