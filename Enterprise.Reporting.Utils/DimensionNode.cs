namespace Enterprise.Reporting.Utils;

public class DimensionNode<T> where T : IDimension
{
    public T Item { get; internal set; }

    public DimensionNode<T>[]? Children { get; internal set; }

    public DimensionNode<T>? Parent { get; internal set; }

    public int Level { get; internal set; } = 0;

    public DimensionNode<T>[]? AllAncestors { get; internal set; }

    public DimensionNode<T>[]? AllChildren { get; internal set; }

    public DimensionNode<T>[]? Leaves { get; internal set; }

    public DimensionNode<T>? Root { get; internal set; }

    internal DimensionNode(T item) => Item = item;
}

public static class DimensionNode
{
    // O(n)
    public static (bool status, DimensionNode<TDimension>[] info) ExtractInfo<TDimension> (this ICollection<TDimension> items)
        where TDimension : IDimension
    {
        if (typeof(TDimension).IsAssignableTo(typeof(IHierarchicalDimension)))
        {
            bool status = true;
            var nodes = items.Select(x => {                                                // O(n)
                if (x.SystemName is null || ((IHierarchicalDimension)x).Parent is null)
                    status = false;
                return new DimensionNode<TDimension>(x);
            }).ToArray();   

            if (!status)
                return (false, nodes);

            var itemsBySystemName = nodes.ToDictionary(x => x.Item.SystemName);            // O(n)
            var itemsByParent = nodes.GroupBy(x => ((IHierarchicalDimension)x.Item).Parent).ToDictionary(x => x.Key, x => x.ToArray()); // O(n)

            if (nodes.Any(x => ((IHierarchicalDimension)x.Item).Parent != "" && 
                !itemsBySystemName.ContainsKey(((IHierarchicalDimension)x.Item).Parent)))  // O(n)
                return (false, nodes);    

            foreach (var node in nodes)                                                    // O(n)
            {
                itemsBySystemName.TryGetValue(((IHierarchicalDimension)node.Item).Parent, out var parent);  // O(1)
                itemsByParent.TryGetValue(node.Item.SystemName, out var children);                          // O(1)

                node.Parent = parent;
                node.Children = children;
                node.AllAncestors = [];
                node.AllChildren = [];
                node.Leaves = [];
            }

            foreach (var dimInfo in result.Where(x => x.Parent?.SystemName == string.Empty))   // O(n)
            {
                int level;
                DimensionNode<TDimension> dim = dimInfo;
                bool searchParent = true;

                while(ok)
                {
                    dim.Parent
                    itemsByParent.TryGetValue(dimInfo.Item.SystemName, out var children);
                    foreach (var child in children)
                    {
                        child.
                    }
                }
                

                //result.FirstOrDefault(x => x.Item.SystemName == dimInfo.Item.Parent).Leaves = [dimInfo.Item];

            }

            return (status && items.Count() == nodes.Length, nodes);
        }
        else
        {
            return (true, items.Select(x => new DimensionNode<TDimension>(x)).ToArray());
        }
    }
}
