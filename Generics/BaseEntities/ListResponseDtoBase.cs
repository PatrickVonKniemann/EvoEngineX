using Generics.Pagination;

namespace Generics.BaseEntities;

public class ListResponseDtoBase<T>
{
    public PaginationResponse? Pagination { get; set; }
    public ItemWrapper<T> Items { get; init; } = new();
}