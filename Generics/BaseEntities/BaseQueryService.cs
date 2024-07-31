using AutoMapper;
using Generics.Exceptions;
using Generics.Pagination;
using Microsoft.Extensions.Logging;

namespace Generics.BaseEntities;

public class BaseQueryService<TEntity, TReadResponse, TListResponseItem, TListResponse> : IGenericEntityQueryService<TReadResponse, TListResponse>
    where TEntity : class
    where TReadResponse : class
    where TListResponse : ListResponseDtoBase<TListResponseItem>, new()
{
    private readonly IMapper _mapper;
    private readonly IRepository<TEntity> _repository;
    private readonly ILogger<BaseQueryService<TEntity, TReadResponse, TListResponseItem, TListResponse>> _logger;

    protected BaseQueryService(IMapper mapper, IRepository<TEntity> repository, ILogger<BaseQueryService<TEntity, TReadResponse, TListResponseItem, TListResponse>> logger)
    {
        _mapper = mapper;
        _repository = repository;
        _logger = logger;
    }

    public async Task<TListResponse> GetAllAsync(PaginationQuery? paginationQuery)
    {
        _logger.LogInformation($"{nameof(BaseQueryService<TEntity, TReadResponse, TListResponseItem, TListResponse>)} {nameof(GetAllAsync)}");

        List<TEntity> entities;
        if (paginationQuery != null)
        {
            entities = await _repository.GetAllAsync(paginationQuery);
            var itemsCount = await _repository.GetCount();
            return new TListResponse
            {
                Items = new ItemWrapper<TListResponseItem>
                {
                    Values = _mapper.Map<List<TListResponseItem>>(entities)
                },
                Pagination = new PaginationResponse
                {
                    PageNumber = paginationQuery.PageNumber,
                    PageSize = paginationQuery.PageSize,
                    ItemsCount = itemsCount,
                    TotalPages = (int)Math.Ceiling((double)itemsCount / paginationQuery.PageSize)
                }
            };
        }
        else
        {
            entities = await _repository.GetAllAsync();
            return new TListResponse
            {
                Items = new ItemWrapper<TListResponseItem>
                {
                    Values = _mapper.Map<List<TListResponseItem>>(entities)
                }
            };
        }
    }

    public async Task<TReadResponse> GetByIdAsync(Guid entityId)
    {
        _logger.LogInformation($"{nameof(BaseQueryService<TEntity, TReadResponse, TListResponseItem, TListResponse>)} {nameof(GetByIdAsync)}");

        var entity = await _repository.GetByIdAsync(entityId);
        if (entity == null)
        {
            throw new DbEntityNotFoundException(typeof(TEntity).Name, entityId);
        }

        return _mapper.Map<TReadResponse>(entity);
    }
}