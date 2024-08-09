using System.Net.Http.Json;
using Generics.Pagination;
using Helpers;
using Microsoft.Extensions.Logging;

namespace ClientApp.Services
{
    public class GenericService<TListResponse, TEntityResponse, TQueryRequest, TCreateRequest, TUpdateRequest>(
        HttpClient httpClient,
        string serviceEndpoint,
        ILogger<GenericService<TListResponse, TEntityResponse, TQueryRequest, TCreateRequest, TUpdateRequest>> logger)
    {
        public async Task<TListResponse?> GetDataAsync(string subPath, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var paginationQuery = new PaginationQuery
                {
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    SortingQuery = new SortingQuery
                    {
                        SortParam = "CreatedAt",
                        SortDirection = SortDirection.Desc
                    }
                };

                var queryRequest = Activator.CreateInstance<TQueryRequest>();
                var paginationProperty = typeof(TQueryRequest).GetProperty("PaginationQuery");
                if (paginationProperty != null && paginationProperty.CanWrite)
                {
                    paginationProperty.SetValue(queryRequest, paginationQuery);
                }

                var response = await httpClient.PostAsJsonAsync($"{serviceEndpoint}/{subPath}", queryRequest);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TListResponse>();
                }

                logger.LogError("Failed to get data: {StatusCode} - {ReasonPhrase}", response.StatusCode,
                    response.ReasonPhrase);
                throw new Exception("Failed to get data from the server.");
            }
            catch (HttpRequestException httpEx)
            {
                logger.LogError(httpEx, "An HTTP request error occurred while fetching data from {ServiceEndpoint}",
                    serviceEndpoint);
                throw new Exception("An error occurred while fetching data. Please try again later.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while fetching data from {ServiceEndpoint}",
                    serviceEndpoint);
                throw;
            }
        }

        public async Task<TEntityResponse?> GetEntityAsync(string entityId)
        {
            try
            {
                var response = await httpClient.GetAsync($"{serviceEndpoint}/{entityId}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TEntityResponse>();
                }

                logger.LogError("Failed to get entity: {StatusCode} - {ReasonPhrase}", response.StatusCode,
                    response.ReasonPhrase);
                throw new Exception("Failed to get entity from the server.");
            }
            catch (HttpRequestException httpEx)
            {
                logger.LogError(httpEx,
                    "An HTTP request error occurred while fetching the entity {EntityId} from {ServiceEndpoint}",
                    entityId, serviceEndpoint);
                throw new Exception("An error occurred while fetching the entity. Please try again later.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "An unexpected error occurred while fetching the entity {EntityId} from {ServiceEndpoint}",
                    entityId, serviceEndpoint);
                throw;
            }
        }

        public async Task AddEntityAsync(TCreateRequest request)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync($"{serviceEndpoint}/add", request);

                if (response.IsSuccessStatusCode)
                {
                    return;
                }

                logger.LogError("Failed to add entity: {StatusCode} - {ReasonPhrase}", response.StatusCode,
                    response.ReasonPhrase);
                throw new Exception("Failed to add the entity to the server.");
            }
            catch (HttpRequestException httpEx)
            {
                logger.LogError(httpEx, "An HTTP request error occurred while adding an entity to {ServiceEndpoint}",
                    serviceEndpoint);
                throw new Exception("An error occurred while adding the entity. Please try again later.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An unexpected error occurred while adding an entity to {ServiceEndpoint}",
                    serviceEndpoint);
                throw;
            }
        }

        public async Task UpdateEntityAsync(string entityId, TUpdateRequest request)
        {
            try
            {
                var content =
                    DeserializationHelper.CreateJsonContent(request ??
                                                            throw new ArgumentNullException(nameof(request)));
                var response = await httpClient.PatchAsync($"{serviceEndpoint}/{entityId}", content);

                if (response.IsSuccessStatusCode)
                {
                    return;
                }

                logger.LogError("Failed to update entity {EntityId}: {StatusCode} - {ReasonPhrase}", entityId,
                    response.StatusCode, response.ReasonPhrase);
                throw new Exception("Failed to update the entity on the server.");
            }
            catch (HttpRequestException httpEx)
            {
                logger.LogError(httpEx,
                    "An HTTP request error occurred while updating the entity {EntityId} at {ServiceEndpoint}",
                    entityId, serviceEndpoint);
                throw new Exception("An error occurred while updating the entity. Please try again later.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "An unexpected error occurred while updating the entity {EntityId} at {ServiceEndpoint}", entityId,
                    serviceEndpoint);
                throw;
            }
        }
    }
}