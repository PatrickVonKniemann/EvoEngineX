namespace Generics.Pagination;

public class PaginationResponse
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int ItemsCount { get; init; }
    public int TotalPages { get; init; }
    public int[]? PageSizeOptions { get; }
    public SortingQuery? SortingQuery { get; set; }
    public Dictionary<string, string> FilterParams { get; set; } = new();
}