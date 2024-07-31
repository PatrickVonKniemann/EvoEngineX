
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Generics.BaseEntities;

public class BaseCommandService<TEntity, TCreateRequest, TCreateResponse, TUpdateRequest, TUpdateResponse> : IGenericEntityCommandService<TCreateRequest, TCreateResponse, TUpdateRequest, TUpdateResponse>
    where TEntity : class
{
    private readonly IMapper _mapper;
    private readonly IRepository<TEntity> _repository;
    private readonly ILogger<BaseCommandService<TEntity, TCreateRequest, TCreateResponse, TUpdateRequest, TUpdateResponse>> _logger;

    protected BaseCommandService(IMapper mapper, IRepository<TEntity> repository, ILogger<BaseCommandService<TEntity, TCreateRequest, TCreateResponse, TUpdateRequest, TUpdateResponse>> logger)
    {
        _mapper = mapper;
        _repository = repository;
        _logger = logger;
    }

    public async Task<TCreateResponse> AddAsync(TCreateRequest entityRequest)
    {
        _logger.LogInformation($"{nameof(BaseCommandService<TEntity, TCreateRequest, TCreateResponse, TUpdateRequest, TUpdateResponse>)} {nameof(AddAsync)}");
        var entity = await _repository.AddAsync(_mapper.Map<TEntity>(entityRequest));
        return _mapper.Map<TCreateResponse>(entity);
    }

    public async Task<TUpdateResponse> UpdateAsync(Guid entityId, TUpdateRequest entityRequest)
    {
        _logger.LogInformation($"{nameof(BaseCommandService<TEntity, TCreateRequest, TCreateResponse, TUpdateRequest, TUpdateResponse>)} {nameof(UpdateAsync)}");
        var updatedEntity = await _repository.UpdateAsync(entityId, _mapper.Map<TEntity>(entityRequest));
        return _mapper.Map<TUpdateResponse>(updatedEntity);
    }

    public async Task DeleteAsync(Guid entityId)
    {
        _logger.LogInformation($"{nameof(BaseCommandService<TEntity, TCreateRequest, TCreateResponse, TUpdateRequest, TUpdateResponse>)} {nameof(DeleteAsync)}");
        await _repository.DeleteAsync(entityId);
    }
}