namespace Generics.Pagination;

public class SortingQuery
{
    public string SortParam { get; set; } = string.Empty;
    public SortDirection SortDirection { get; set; } = SortDirection.ASC; // 'asc' or 'desc'
}
