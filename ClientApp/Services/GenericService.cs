using System.Net.Http.Json;
using Generics.Pagination;

namespace ClientApp.Services
{
    public class GenericService<TListResponse, TEntityResponse, TQueryRequest>
    {
        private readonly HttpClient _httpClient;
        private readonly string _serviceEndpoint;

        public GenericService(HttpClient httpClient, string serviceEndpoint)
        {
            _httpClient = httpClient;
            _serviceEndpoint = serviceEndpoint;
        }

        public async Task<TListResponse?> GetDataAsync(int pageNumber = 1, int pageSize = 10)
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

            var response = await _httpClient.PostAsJsonAsync($"{_serviceEndpoint}/all", queryRequest);
            return await response.Content.ReadFromJsonAsync<TListResponse>();
        }

        public async Task<TEntityResponse?> GetEntityAsync(string entityId)
        {
            var response = await _httpClient.GetAsync($"{_serviceEndpoint}/{entityId}");
            return await response.Content.ReadFromJsonAsync<TEntityResponse>();
        }
    }
}