namespace Enterprise.Reporting.Utils;

internal class DimensionInfo<T>
{
    public T Item { get; private set; }
    
    public T[]? Children { get; private set; }

    public T? Parent { get; private set; }

    public DimensionInfo(T item)
    {
        Item = item;
    }
}
