using Generics.Pagination;

namespace Generics.BaseEntities;

public interface IRepository<TEntity> where TEntity : class
{
    Task<int> GetCount();
    Task<List<TEntity>> GetAllAsync();
    Task<List<TEntity>> GetAllAsync(PaginationQuery? paginationQuery);
    Task<TEntity?> GetByIdAsync(Guid entityId);
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> UpdateAsync(Guid entityId, TEntity updatedEntity);
    Task DeleteAsync(Guid entityId);
}