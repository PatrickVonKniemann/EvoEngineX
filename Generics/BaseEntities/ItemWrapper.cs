namespace Generics.BaseEntities;

public class ItemWrapper<T>
{
    public List<T> Values { get; set; } = new();
}