using ExternalDomainEntities.CodeRunDto.Command;
using ExternalDomainEntities.CodeRunDto.Query;

namespace ClientApp.Services;

public class CodeRunService(HttpClient httpClient) : GenericService<ReadCodeRunListByCodeBaseIdResponse, ReadCodeRunResponse, ReadCodeRunListRequest, CreateCodeRunRequest, UpdateCodeRunRequest>(httpClient, "http://localhost:5001/code-run");