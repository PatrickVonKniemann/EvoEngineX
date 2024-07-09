using Generics.Pagination;

namespace Generics.BaseEntities;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

public class BaseRepository<TEntity> where TEntity : class
{
    private readonly List<string> _propertyNames;

    public BaseRepository()
    {
        _propertyNames = typeof(TEntity).GetProperties().Select(p => p.Name).ToList();
    }

    public List<TEntity> GetAll(IQueryable<TEntity> query, PaginationQuery paginationQuery)
    {
        ValidateQueryParams(paginationQuery);
        
        query = ApplyFiltering(query, paginationQuery);
        query = ApplySorting(query, paginationQuery.SortingQuery);
        query = ApplyPagination(query, paginationQuery);

        return query.ToList();
    }

    private void ValidateQueryParams(PaginationQuery paginationQuery)
    {
        // Check if the PageNumber and PageSize are positive
        if (paginationQuery != null && paginationQuery.PageNumber <= 0)
        {
            throw new ArgumentException("CoreMessages.PagingWrongPage");
        }

        // Validate the sorting parameter if it's specified
        if (paginationQuery != null && !string.IsNullOrEmpty(paginationQuery.SortingQuery.SortParam))
        {
            ValidateSortingParameter(paginationQuery.SortingQuery);
        }

        // Validate the filter parameter if it's specified
        if (paginationQuery != null && !paginationQuery.FilterParams.Any())
        {
            ValidateFilterParameter(paginationQuery.FilterParams);
        }
    }

    private void ValidateSortingParameter(SortingQuery sortingQuery)
    {
        // Validate that the sort direction is either "asc" or "desc"
        bool isSortParamProvided = !string.IsNullOrEmpty(sortingQuery.SortParam);

        // If neither is provided, there's nothing more to validate
        if (!isSortParamProvided) return;

        if (!_propertyNames.Contains(sortingQuery.SortParam))
            throw new ArgumentException("Sort Params doesnt match");
    }

    private void ValidateFilterParameter(Dictionary<string, string> filterParams)
    {
        bool isFilterParamAndValueProvided = filterParams.Count > 0;

        if (!isFilterParamAndValueProvided) return;

        if (filterParams.Any(param => !_propertyNames.Contains(param.Key)))
            throw new ArgumentException("Filter Params doesnt match");
    }

    private IQueryable<TEntity> ApplyFiltering(IQueryable<TEntity> query, PaginationQuery paginationQuery)
    {
        if (paginationQuery != null && paginationQuery.FilterParams.Any())
        {
            var parameterExp = Expression.Parameter(typeof(TEntity), "type");
            Expression? finalExp = null;
            var method = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            foreach (var (paramName, paramValue) in paginationQuery.FilterParams)
            {
                var propertyExp = Expression.Property(parameterExp, paramName);
                var someValue = Expression.Constant(paramValue, typeof(string));
                if (method != null)
                {
                    var containsMethodExp = Expression.Call(propertyExp, method, someValue);

                    // Use AND or OR condition based on FilterCondition
                    if (finalExp == null)
                    {
                        finalExp = containsMethodExp;
                    }
                    else
                    {
                        finalExp = paginationQuery.FilterCondition == FilterCondition.AND
                            ? Expression.AndAlso(finalExp, containsMethodExp)
                            : Expression.OrElse(finalExp, containsMethodExp);
                    }
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

    private IQueryable<TEntity> ApplySorting(IQueryable<TEntity> query, SortingQuery sortingQuery)
    {
// Check if SortBy is not null or empty to apply sorting.
        if (!string.IsNullOrWhiteSpace(sortingQuery.SortParam))
        {
            // Get the property by name from TEntity to build the expression.
            PropertyInfo? propertyInfo = typeof(TEntity).GetProperty(sortingQuery.SortParam,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo != null)
            {
                // Create an expression to represent the sort property.
                ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
                Expression property = Expression.Property(parameter, propertyInfo);
                LambdaExpression lambda = Expression.Lambda(property, parameter);

                // Determine whether to sort ascending or descending.
                if (sortingQuery.SortDirection == SortDirection.ASC)
                {
                    MethodCallExpression orderByCall = Expression.Call(
                        typeof(Queryable),
                        "OrderBy",
                        new[] { typeof(TEntity), property.Type },
                        query.Expression,
                        Expression.Quote(lambda));

                    return query.Provider.CreateQuery<TEntity>(orderByCall);
                }
                else if (sortingQuery.SortDirection == SortDirection.DESC)
                {
                    MethodCallExpression orderByDescendingCall = Expression.Call(
                        typeof(Queryable),
                        "OrderByDescending",
                        new[] { typeof(TEntity), property.Type },
                        query.Expression,
                        Expression.Quote(lambda));

                    return query.Provider.CreateQuery<TEntity>(orderByDescendingCall);
                }
            }
        }


        return query;
    }

    private IQueryable<TEntity> ApplyPagination(IQueryable<TEntity> query, PaginationQuery paginationQuery)
    {
        if (paginationQuery.PageSize > 0)
        {
            int skipAmount = (paginationQuery.PageNumber - 1) * paginationQuery.PageSize;
            query = query.Skip(skipAmount).Take(paginationQuery.PageSize);
        }

        return query;
    }
}
