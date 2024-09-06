using System.Linq.Expressions;
using System.Reflection;
using Generics.Exceptions;
using Generics.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Generics.BaseEntities;

public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    private readonly DbContext _context;
    private readonly ILogger<BaseRepository<TEntity>> _logger;
    private readonly List<string> _propertyNames;

    protected BaseRepository(DbContext context, ILogger<BaseRepository<TEntity>> logger)
    {
        _context = context;
        _logger = logger;
        _propertyNames = typeof(TEntity).GetProperties().Select(p => p.Name).ToList();
    }

    public Task<int> GetCount()
    {
        _logger.LogInformation("Getting count of {Entity}", typeof(TEntity).Name);
        return _context.Set<TEntity>().CountAsync();
    }

    protected async Task<int> GetCountByParameterAsync<TValue>(string parameterName, TValue parameterValue)
    {
        _logger.LogInformation("Getting count of {Entity} with {ParameterName} = {ParameterValue}",
            typeof(TEntity).Name, parameterName, parameterValue);

        var parameter = Expression.Parameter(typeof(TEntity), "entity");
        var property = Expression.Property(parameter, parameterName);
        var value = Expression.Constant(parameterValue);
        var equals = Expression.Equal(property, value);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(equals, parameter);

        return await _context.Set<TEntity>().CountAsync(lambda);
    }

    // Merged GetAllAsync with optional pagination, filtering, sorting, and includes
    public async Task<List<TEntity>> GetAllAsync(
        PaginationQuery? paginationQuery = null,
        Expression<Func<TEntity, bool>>? filter = null,
        params Expression<Func<TEntity, object>>[] includes)
    {
        _logger.LogInformation("Getting all {Entity} with optional pagination, filtering, sorting, and includes", typeof(TEntity).Name);
        IQueryable<TEntity> query = _context.Set<TEntity>();

        // Apply includes if provided
        if (includes != null && includes.Any())
        {
            query = ApplyIncludes(query, includes);
        }

        // Apply filtering if provided
        if (filter != null)
        {
            query = query.Where(filter);
        }

        // Apply pagination and sorting if provided
        if (paginationQuery != null)
        {
            query = ApplyFiltering(query, paginationQuery);
            query = ApplySorting(query, paginationQuery);
            query = ApplyPagination(query, paginationQuery);
        }

        return await query.ToListAsync();
    }

    public async Task<TEntity?> GetByIdAsync(Guid entityId, params Expression<Func<TEntity, object>>[] includes)
    {
        _logger.LogInformation("Getting {Entity} by Id: {EntityId} with optional includes", typeof(TEntity).Name, entityId);
        IQueryable<TEntity> query = _context.Set<TEntity>();

        // Apply includes if provided
        if (includes != null && includes.Any())
        {
            query = ApplyIncludes(query, includes);
        }

        return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == entityId);
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        _logger.LogInformation("Adding a new {Entity}", typeof(TEntity).Name);
        SetGuidIdIfExists(entity);
        await _context.Set<TEntity>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    private void SetGuidIdIfExists(TEntity entity)
    {
        var idProperty = typeof(TEntity).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
        if (idProperty != null && idProperty.PropertyType == typeof(Guid))
        {
            var currentValue = idProperty.GetValue(entity);
            if (currentValue == null || (Guid)currentValue == Guid.Empty)
            {
                idProperty.SetValue(entity, Guid.NewGuid());
            }
        }
    }

    public async Task<TEntity> UpdateAsync(Guid entityId, TEntity updatedEntity)
    {
        _logger.LogInformation("Updating {Entity} with Id: {EntityId}", typeof(TEntity).Name, entityId);
        var entity = await _context.Set<TEntity>().FindAsync(entityId);
        if (entity == null)
            throw new DbEntityNotFoundException(nameof(TEntity), entityId);

        foreach (var property in typeof(TEntity).GetProperties())
        {
            var currentValue = property.GetValue(entity);
            var updatedValue = property.GetValue(updatedEntity);

            if (updatedValue != null && !updatedValue.Equals(currentValue))
            {
                property.SetValue(entity, updatedValue);
            }
        }

        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid entityId)
    {
        _logger.LogInformation("Deleting {Entity} with Id: {EntityId}", typeof(TEntity).Name, entityId);
        var entity = await _context.Set<TEntity>().FindAsync(entityId);
        if (entity == null)
            throw new DbEntityNotFoundException(nameof(TEntity), entityId);

        _context.Set<TEntity>().Remove(entity);
        await _context.SaveChangesAsync();
    }

    // Helper methods

    private IQueryable<TEntity> ApplyIncludes(IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes)
    {
        foreach (var include in includes)
        {
            query = query.Include(include);
        }

        return query;
    }

    private IQueryable<TEntity> ApplyFiltering(IQueryable<TEntity> query, PaginationQuery? paginationQuery)
    {
        if (paginationQuery != null && paginationQuery.FilterParams.Any())
        {
            ValidateFilterParameter(paginationQuery.FilterParams);
            var parameterExp = Expression.Parameter(typeof(TEntity), "type");
            Expression? finalExp = null;
            var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            foreach (var (paramName, paramValue) in paginationQuery.FilterParams)
            {
                var propertyExp = Expression.Property(parameterExp, paramName);
                var someValue = Expression.Constant(paramValue, typeof(string));
                if (method == null) continue;
                var containsMethodExp = Expression.Call(propertyExp, method, someValue);

                if (finalExp == null)
                {
                    finalExp = containsMethodExp;
                }
                else
                {
                    finalExp = paginationQuery.FilterCondition == FilterCondition.And
                        ? Expression.AndAlso(finalExp, containsMethodExp)
                        : Expression.OrElse(finalExp, containsMethodExp);
                }
            }

            if (finalExp != null)
            {
                var lambda = Expression.Lambda<Func<TEntity, bool>>(finalExp, parameterExp);
                query = query.Where(lambda);
            }
        }

        return query;
    }

    private IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, PaginationQuery? paginationQuery)
    {
        if (paginationQuery?.SortingQuery != null)
        {
            var sortingQuery = paginationQuery.SortingQuery;
            ValidateSortingParameter(sortingQuery);
            PropertyInfo? propertyInfo = typeof(TEntity).GetProperty(sortingQuery.SortParam,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null) return query;
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
            Expression property = Expression.Property(parameter, propertyInfo);
            LambdaExpression lambda = Expression.Lambda(property, parameter);

            var orderByMethod = sortingQuery.SortDirection == SortDirection.Asc ? "OrderBy" : "OrderByDescending";
            MethodCallExpression orderByCall = Expression.Call(
                typeof(Queryable),
                orderByMethod,
                new[] { typeof(TEntity), property.Type },
                query.Expression,
                Expression.Quote(lambda));

            query = query.Provider.CreateQuery<TEntity>(orderByCall);
        }

        return query;
    }

    private IQueryable<TEntity> ApplyPagination(IQueryable<TEntity> query, PaginationQuery? paginationQuery)
    {
        if (paginationQuery?.PageSize > 0)
        {
            int skipAmount = (paginationQuery.PageNumber - 1) * paginationQuery.PageSize;
            _logger.LogInformation(
                "Applying pagination on {Entity}: PageNumber = {PageNumber}, PageSize = {PageSize}",
                typeof(TEntity).Name, paginationQuery.PageNumber, paginationQuery.PageSize);
            query = query.Skip(skipAmount).Take(paginationQuery.PageSize);
        }

        return query;
    }

    private void ValidateFilterParameter(Dictionary<string, string> filterParams)
    {
        if (!filterParams.Any()) return;

        if (filterParams.Any(param => !_propertyNames.Contains(param.Key)))
        {
            _logger.LogWarning("Invalid filter parameters: {FilterParams}", filterParams);
            throw new ArgumentException(CoreMessages.FilterParametersDoesntMatch);
        }
    }

    private void ValidateSortingParameter(SortingQuery sortingQuery)
    {
        if (string.IsNullOrEmpty(sortingQuery.SortParam)) return;

        if (!_propertyNames.Contains(sortingQuery.SortParam))
        {
            _logger.LogWarning("Invalid sorting parameter: {SortParam}", sortingQuery.SortParam);
            throw new ArgumentException(CoreMessages.SortParametersDoesntMatch);
        }
    }
}
