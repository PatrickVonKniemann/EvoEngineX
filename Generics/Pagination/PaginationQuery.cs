namespace Generics.Pagination;

/// <summary>
/// Pagination parameters response
/// </summary>
public class PaginationQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public SortingQuery SortingQuery { get; } = new();
    public Dictionary<string, string> FilterParams { get; } = new();
    public FilterCondition FilterCondition => FilterCondition.Or;
}