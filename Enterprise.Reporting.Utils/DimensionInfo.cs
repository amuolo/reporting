namespace Enterprise.Reporting.Utils;

public class DimensionInfo<T> where T : IDimension
{
    public T Item { get; internal set; }

    public T[]? Children { get; internal set; }

    public T? Parent { get; internal set; }

    public int Level { get; internal set; } = 0;

    public T[]? Leaves { get; internal set; }

    public T? Root { get; internal set; }
}

public static class DimensionInfo
{ 
    public static (bool status, DimensionInfo<TDimension>[] info) ExtractInfo<TDimension> (this ICollection<TDimension> items)  // O(n)
        where TDimension : IDimension
    {
        if (typeof(TDimension).IsAssignableTo(typeof(IHierarchicalDimension)))
        {
            var convertedItems = items.Cast<IHierarchicalDimension>().ToArray();  // O(n)

            if (convertedItems is null)
                return (false, Array.Empty<DimensionInfo<TDimension>>());

            var itemsBySystemName = convertedItems.ToDictionary(x => x.SystemName);                                // O(n)
            var itemsByParent = convertedItems.GroupBy(x => x.Parent).ToDictionary(x => x.Key, x => x.ToArray());  // O(n)
            bool status = true;

            if (convertedItems.Any(x => x.Parent != "" && convertedItems.FirstOrDefault(y => y.SystemName == x.Parent) is null))  // O(n)
                return (false, []);

            var result = convertedItems.Select(dim =>   // O(n)
            {
                if (dim.Parent is null)
                {
                    status = false;
                    return new DimensionInfo<TDimension>();
                }

                itemsBySystemName.TryGetValue(dim.Parent, out var parent);    // O(1)
                itemsByParent.TryGetValue(dim.SystemName, out var children);  // O(1)

                return new DimensionInfo<TDimension>() { 
                    Item = (TDimension)dim, 
                    Parent = (TDimension?)parent, 
                    Children = children?.Cast<TDimension>()?.ToArray() };
            }).ToArray();

            foreach (var item in result)
            {
                //var x = 
                //var root = item.Parent;

            }

            return (status && items.Count() == result.Length, result);
        }
        else
        {
            return (true, items.Select(x => new DimensionInfo<TDimension> { Item = x }).ToArray());
        }
    }
}
