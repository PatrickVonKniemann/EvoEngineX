using Generics.BaseEntities;

namespace Generics.Pagination;

public class ReadListResponse<T>
{
    public ItemWrapper<T>? Items { get; set; }
    public PaginationResponse? Pagination { get; set; }
}