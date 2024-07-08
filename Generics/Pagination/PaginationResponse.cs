namespace Generics.Pagination;

public class PaginationResponse
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; }
    public int ItemsCount { get; init; }
    public int[] PageSizeOptions { get; } = { 5, 10, 25 };
    public SortingQuery SortingQuery { get; set; } = new();
    public Dictionary<string, string>  FilterParams { get; set; } = new();
}