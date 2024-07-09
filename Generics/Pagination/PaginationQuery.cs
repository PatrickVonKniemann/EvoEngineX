using FastEndpoints;

namespace Generics.Pagination;

/// <summary>
/// Pagination parameters response
/// </summary>
public class PaginationQuery
{
    [QueryParam] public int PageNumber { get; set; } = 1;
    [QueryParam] public int PageSize { get; set; }
    [QueryParam] public SortingQuery SortingQuery { get; set; } = new();
    [QueryParam] public Dictionary<string, string> FilterParams { get; set; } = new();
    [QueryParam] public FilterCondition FilterCondition { get; set; } = FilterCondition.OR;
}