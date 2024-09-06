using Generics.Pagination;

namespace Generics.BaseEntities;

public class ListResponseDtoBase<T>
{
    public PaginationResponse? Pagination { get; init; }
    public ItemWrapper<T> Items { get; init; } = new();
}