using ExternalDomainEntities.CodeRunDto.Query;

namespace ClientApp.Services;

public class CodeRunService(HttpClient httpClient) : GenericService<ReadCodeRunListByCodeBaseIdResponse, ReadCodeRunResponse, ReadCodeRunListRequest>(httpClient, "http://localhost:5001/code-run");
