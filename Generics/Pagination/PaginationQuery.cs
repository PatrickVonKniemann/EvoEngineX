namespace Generics.Pagination;

/// <summary>
/// Pagination parameters response
/// </summary>
public class PaginationQuery
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; }
    public SortingQuery SortingQuery { get; set; } = new();
    public Dictionary<string, string> FilterParams { get; set; } = new();
    public FilterCondition FilterCondition { get; set; } = FilterCondition.OR;
}

