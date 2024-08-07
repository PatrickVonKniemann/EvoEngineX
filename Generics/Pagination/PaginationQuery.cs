namespace Generics.Pagination;

/// <summary>
/// Pagination parameters response
/// </summary>
public class PaginationQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public SortingQuery? SortingQuery { get; init; }
    public Dictionary<string, string> FilterParams { get; } = new();
    public FilterCondition FilterCondition => FilterCondition.Or;
}