namespace Generics.Pagination;

public class SortingQuery
{
    public required string SortParam { get; init; }
    public SortDirection SortDirection { get; init; }
}
