using System.Linq.Expressions;
using Generics.Pagination;

namespace Generics.BaseEntities;

public interface IRepository<TEntity> where TEntity : class
{
    // Get total count of all entities
    Task<int> GetCount();

    // Get all entities with optional pagination, filtering, sorting, and includes
    Task<List<TEntity>> GetAllAsync(
        PaginationQuery? paginationQuery = null,
        Expression<Func<TEntity, bool>>? filter = null,
        params Expression<Func<TEntity, object>>[] includes
    );

    // Get entity by Id with optional includes
    Task<TEntity?> GetByIdAsync(Guid entityId, params Expression<Func<TEntity, object>>[] includes);

    // Add a new entity
    Task<TEntity> AddAsync(TEntity entity);

    // Update an entity by Id
    Task<TEntity> UpdateAsync(Guid entityId, TEntity updatedEntity);

    // Delete an entity by Id
    Task DeleteAsync(Guid entityId);
}