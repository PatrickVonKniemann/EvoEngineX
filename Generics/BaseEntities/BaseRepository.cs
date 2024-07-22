using Generics.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Common.Exceptions;

namespace Generics.BaseEntities
{
    public class BaseRepository<TEntity> where TEntity : class
    {
        private readonly List<string> _propertyNames;

        public BaseRepository()
        {
            _propertyNames = typeof(TEntity).GetProperties().Select(p => p.Name).ToList();
        }

        public async Task<List<TEntity>> GetAllAsync(IQueryable<TEntity> query)
        {
            return await Task.FromResult(query.ToList());
        }
        
        public async Task<List<TEntity>> GetAllAsync(IQueryable<TEntity> query, PaginationQuery paginationQuery)
        {
            ValidateQueryParams(paginationQuery);

            query = ApplyFiltering(query, paginationQuery);
            query = ApplySorting(query, paginationQuery.SortingQuery);
            query = ApplyPagination(query, paginationQuery);

            return await Task.FromResult(query.ToList());
        }

        private void ValidateQueryParams(PaginationQuery paginationQuery)
        {
            if (paginationQuery != null && paginationQuery.PageNumber <= 0)
            {
                throw new ArgumentException(CoreMessages.PagingWrongPage);
            }

            if (paginationQuery != null && !string.IsNullOrEmpty(paginationQuery.SortingQuery.SortParam))
            {
                ValidateSortingParameter(paginationQuery.SortingQuery);
            }

            if (paginationQuery != null && !paginationQuery.FilterParams.Any())
            {
                ValidateFilterParameter(paginationQuery.FilterParams);
            }
        }

        private void ValidateSortingParameter(SortingQuery sortingQuery)
        {
            bool isSortParamProvided = !string.IsNullOrEmpty(sortingQuery.SortParam);

            if (!isSortParamProvided) return;

            if (!_propertyNames.Contains(sortingQuery.SortParam))
                throw new ArgumentException(CoreMessages.SortParametersDoesntMatch);
        }

        private void ValidateFilterParameter(Dictionary<string, string> filterParams)
        {
            bool isFilterParamAndValueProvided = filterParams.Count > 0;

            if (!isFilterParamAndValueProvided) return;

            if (filterParams.Any(param => !_propertyNames.Contains(param.Key)))
                throw new ArgumentException(CoreMessages.FilterParametersDoesntMatch);
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
            if (!string.IsNullOrWhiteSpace(sortingQuery.SortParam))
            {
                PropertyInfo? propertyInfo = typeof(TEntity).GetProperty(sortingQuery.SortParam,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo != null)
                {
                    ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
                    Expression property = Expression.Property(parameter, propertyInfo);
                    LambdaExpression lambda = Expression.Lambda(property, parameter);

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
}
