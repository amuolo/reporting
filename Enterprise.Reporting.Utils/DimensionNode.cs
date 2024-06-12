using System.Runtime;

namespace Enterprise.Reporting.Utils;

public class DimensionNode<T> where T : IDimension
{
    public T Item { get; internal set; }

    public HashSet<DimensionNode<T>>? Children { get; internal set; }

    public DimensionNode<T>? Parent { get; internal set; }

    public int Level { get; internal set; } = 0;

    public HashSet<DimensionNode<T>>? AllAncestors { get; internal set; }

    public HashSet<DimensionNode<T>>? AllChildren { get; internal set; }

    public HashSet<DimensionNode<T>>? Leaves { get; internal set; }

    public DimensionNode<T>? Root { get; internal set; }

    internal DimensionNode(T item) => Item = item;
}

public static class DimensionNode
{
    // O(leaves * num_levels^2)
    public static (bool status, DimensionNode<TDimension>[] info) ExtractNodes<TDimension> (this ICollection<TDimension> items)
        where TDimension : IDimension
    {
        if (typeof(TDimension).IsAssignableTo(typeof(IHierarchicalDimension)))
        {
            var nodes = items.Select(x => new DimensionNode<TDimension>(x)).ToArray();                                // O(n)

            if (items.Any(x => x.SystemName is null))                                                                 // O(n)
                return (false, nodes);

            var nodesWithParent = nodes.Where(x => ((IHierarchicalDimension)x.Item).Parent != null).ToArray();        // O(n)
            var itemsBySystemName = nodes.ToDictionary(x => x.Item.SystemName);                                       // O(n)
            var itemsByParent = nodesWithParent.GroupBy(x => ((IHierarchicalDimension)x.Item).Parent!)
                                               .ToDictionary(x => x.Key, x => x.ToArray());                           // O(n)

            if (nodesWithParent.Any(x => !itemsBySystemName.ContainsKey(((IHierarchicalDimension)x.Item).Parent!)))   // O(n)
                return (false, nodes);

            foreach (var node in nodes)                                                                               // O(n)
            {
                var parentSystemName = ((IHierarchicalDimension)node.Item).Parent?? Guid.NewGuid().ToString();
                itemsBySystemName.TryGetValue(parentSystemName, out var parent);                                      // O(1)
                itemsByParent.TryGetValue(node.Item.SystemName, out var children);                                    // O(1)

                node.Parent = parent;
                node.Children = children?.ToHashSet()?? [];
                node.AllAncestors = parent is null ? [] : [parent];
                node.AllChildren = children?.ToHashSet()?? [];
                node.Leaves = children is null ? [node] : [];
                node.Root = parent is null ? node : null;
            }

            foreach (var node in nodes.Where(x => !x.Children!.Any()))                                                // O(leaves)
            { 
                var tmp = node;
                HashSet<DimensionNode<TDimension>> ancestors = [];

                while (((IHierarchicalDimension)tmp.Item).Parent is not null)                                         // O(num_levels)
                {
                    node.Level++;
                    itemsBySystemName.TryGetValue(((IHierarchicalDimension)tmp.Item).Parent!, out var tmpParent);

                    if (tmpParent is null)
                        throw new Exception("DimensionNode ExtractDimension unexpected null");

                    tmpParent.AllChildren?.Add(tmp);                                                                  // O(1)
                    tmpParent.AllChildren?.Add(node);                                                                 // O(1)
                    tmpParent.Leaves?.Add(node);                                                                      // O(1)
                    ancestors.Add(tmpParent);                                                                         // O(1)
                    tmp = tmpParent;
                }
                node.Root = tmp;
                node.AllAncestors = ancestors.ToHashSet();                                                            // O(num_levels)                                                            
                foreach (var ancestor in ancestors)                                                                   // O(num_levels)
                {
                    ancestor.Root = tmp;
                    ancestor.Level = ancestor.Children!.First().Level - 1;
                    ancestors.Remove(ancestor);
                    ancestor.AllAncestors = ancestors.ToHashSet();                                                    // O(num_levels)
                }
            }

            return (true && items.Count() == nodes.Length, nodes);
        }
        else
        {
            return (true, items.Select(x => new DimensionNode<TDimension>(x)).ToArray());
        }
    }
}
