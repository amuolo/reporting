using System.Runtime;

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
    // O(leaves * num_levels)
    public static (bool status, DimensionNode<TDimension>[] info) ExtractNodes<TDimension> (this ICollection<TDimension> items)
        where TDimension : IDimension
    {
        if (typeof(TDimension).IsAssignableTo(typeof(IHierarchicalDimension)))
        {
            bool status = true;
            var nodes = items.Select(x => {                                                // O(n)
                if (x.SystemName is null)
                    status = false;
                return new DimensionNode<TDimension>(x);
            }).ToArray();   

            if (!status)
                return (false, nodes);

            var nodesWithParent = nodes.Where(x => ((IHierarchicalDimension)x.Item).Parent != null).ToArray();        // O(n)

            var itemsBySystemName = nodes.ToDictionary(x => x.Item.SystemName);                                       // O(n)
            var itemsByParent = nodesWithParent.GroupBy(x => ((IHierarchicalDimension)x.Item).Parent!).ToDictionary(x => x.Key, x => x.ToArray()); // O(n)

            if (nodesWithParent.Any(x => !itemsBySystemName.ContainsKey(((IHierarchicalDimension)x.Item).Parent!)))   // O(n)
                return (false, nodes);

            foreach (var node in nodes)                                                                               // O(n)
            {
                itemsBySystemName.TryGetValue(((IHierarchicalDimension)node.Item).Parent?? Guid.NewGuid().ToString(), out var parent);  // O(1)
                itemsByParent.TryGetValue(node.Item.SystemName, out var children);                                    // O(1)

                node.Parent = parent;
                node.Children = children;
                node.AllAncestors = parent is null ? [] : [parent];
                node.AllChildren = children;
                node.Leaves = children is null ? [node] : [];
                node.Root = parent is null ? node : null;
            }

            foreach (var node in nodes.Where(x => x.Children is null))                     // O(leaves)
            { 
                var tmp = node;

                while (true)                                                               // O(num_levels)
                {
                    if (((IHierarchicalDimension)tmp.Item).Parent is null)
                    {
                        node.Root = tmp;
                        break;
                    }

                    node.Level++;
                    itemsBySystemName.TryGetValue(((IHierarchicalDimension)tmp.Item).Parent!, out var tmpParent);

                    if (tmpParent is null)
                        throw new Exception("DimensionNode ExtractDimension unexpected null");

                    tmp.AllAncestors = tmp.AllAncestors?.Concat([tmpParent]).DistinctBy(x => x.Item.SystemName).ToArray();

                    tmpParent.AllChildren = tmpParent.AllChildren?.Concat([tmp]).Concat(tmp.AllChildren?? []).DistinctBy(x => x.Item.SystemName).ToArray();
                    tmpParent.Leaves = tmpParent.Leaves?.Concat(tmp.Leaves!).DistinctBy(x => x.Item.SystemName).ToArray();
                    
                    tmp = tmpParent;
                }
            }

            foreach (var node in nodes.Where(x => x.Children is null))                       // O(leaves)
            {
                var tmp = node;
                while (((IHierarchicalDimension)tmp.Item).Parent is not null)                // O(num_levels)
                {
                    itemsBySystemName.TryGetValue(((IHierarchicalDimension)tmp.Item).Parent!, out var tmpParent);
                    tmpParent!.Level = tmp.Level - 1;
                    tmpParent!.Root = tmp.Root;
                    tmp = tmpParent;
                }
            }

            return (status && items.Count() == nodes.Length, nodes);
        }
        else
        {
            return (true, items.Select(x => new DimensionNode<TDimension>(x)).ToArray());
        }
    }
}
