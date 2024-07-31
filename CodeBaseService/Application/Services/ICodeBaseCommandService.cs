using ExternalDomainEntities.CodeBaseDto.Command;
using Generics.BaseEntities;

namespace CodebaseService.Application.Services;

public interface ICodeBaseCommandService : IGenericEntityCommandService<CreateCodeBaseRequest, CreateCodeBaseResponse,
    UpdateCodeBaseRequest, UpdateCodeBaseResponse>;