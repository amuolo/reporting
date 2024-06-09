using System.Collections.Generic;
using System.Reflection;

namespace Enterprise.Reporting.Utils;

public class DimensionRegister
{
    public Dictionary<string, IDimension[]> Dimensions { get; private set; } = [];

    public Dictionary<string, IHierarchicalDimension[]> HierarchicalDimensions { get; private set; } = [];

    public Dictionary<string, IOrderedDimension[]> OrderedDimensions { get; private set; } = [];

    public Dictionary<string, IOrderedHierarchicalDimension[]> OrderedHierarchicalDimensions { get; private set; } = [];

    public void Insert<T>(T[] items)
    {
        if (items is null || items.Length == 0)
            return;

        if (typeof(T).IsAssignableTo(typeof(IOrderedHierarchicalDimension)))
        {
            OrderedHierarchicalDimensions.TryAdd(typeof(T).Name, items.Cast<IOrderedHierarchicalDimension>().ToArray());
        }
        else if (typeof(T).IsAssignableTo(typeof(IOrderedDimension)))
        {
            OrderedDimensions.TryAdd(typeof(T).Name, items.Cast<IOrderedDimension>().ToArray());
        }
        else if (typeof(T).IsAssignableTo(typeof(IHierarchicalDimension)))
        {
            HierarchicalDimensions.TryAdd(typeof(T).Name, items.Cast<IHierarchicalDimension>().ToArray());
        }
        else if (typeof(T).IsAssignableTo(typeof(IDimension)))
        {
            Dimensions.TryAdd(typeof(T).Name, items.Cast<IDimension>().ToArray());
        }
    }

}
