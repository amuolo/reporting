namespace Enterprise.Reporting.Utils;

public class DimensionInfo<T> where T : IDimension
{
    public T Item { get; private set; }
    
    public T[]? Children { get; private set; }

    public T? Parent { get; private set; }

    public static (bool status, DimensionInfo<T>[] info) ExtractInfo (ICollection<T> items)
    {
        if (typeof(T).IsAssignableTo(typeof(IHierarchicalDimension)))
        {
            var convertedItems = items.Cast<IHierarchicalDimension>().ToArray();

            if (convertedItems is null)
                return (false, Array.Empty<DimensionInfo<T>>());

            var itemsByParent = convertedItems.GroupBy(x => x.Parent).ToDictionary(x => x.Key, x => x.ToArray());
            bool status = true;

            if (convertedItems.Any(x => x.Parent != "" && convertedItems.FirstOrDefault(y => y.SystemName == x.Parent) is null))
                return (false, []);

            var result = convertedItems.Select(convertedItem => {
                if (convertedItem.Parent is null) {
                    status = false;
                    return new DimensionInfo<T>();
                }
                var item = (T)convertedItem;
                var parent = items.FirstOrDefault(x => x.SystemName == convertedItem.Parent);
                itemsByParent.TryGetValue(convertedItem.SystemName, out var children);
                return new DimensionInfo<T>() { Item = item, Parent = parent, Children = children?.Cast<T>()?.ToArray() };
                }).ToArray();

            return (status && items.Count() == result.Length, result);
        }
        else
        {
            return (true, items.Select(x => new DimensionInfo<T> { Item = x }).ToArray());
        }
    }
}
