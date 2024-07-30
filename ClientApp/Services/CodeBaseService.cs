using ExternalDomainEntities.CodeBaseDto.Query;

namespace ClientApp.Services;

public class CodeBaseService(HttpClient httpClient) : GenericService<ReadCodeBaseListResponse, ReadCodeBaseResponse, ReadCodeBaseListRequest>(httpClient, "http://localhost:5002/code-base");