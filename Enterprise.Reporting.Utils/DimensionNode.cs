namespace Enterprise.Reporting.Utils;

public class DimensionNode
{
    public IDimension Item { get; internal set; }

    public HashSet<DimensionNode>? Children { get; internal set; }

    public DimensionNode? Parent { get; internal set; }

    public int Level { get; internal set; } = 0;

    public HashSet<DimensionNode>? AllAncestors { get; internal set; }

    public HashSet<DimensionNode>? AllChildren { get; internal set; }

    public HashSet<DimensionNode>? Siblings { get; internal set; }

    public HashSet<DimensionNode>? Leaves { get; internal set; }

    public DimensionNode? Root { get; internal set; }

    internal DimensionNode(IDimension item) => Item = item;
}

public static class DimensionNodeUtils
{
    /*  This method finds all nodes and caches the hierarchy information. 
     *  The complexity of this operation is: O(max(leaves * num_levels^2, n * children))
     */
    public static (bool status, DimensionNode[] info) ExtractNodes<TDimension>(this IEnumerable<TDimension> input)
        where TDimension : IDimension
    {
        if (input is null || !input.Any())
            return (false, []);

        var items = input;
        if (!(input is TDimension[] || input is List<TDimension>))
            items = input.ToArray();

        var nodes = items.Select(x => new DimensionNode(x)).ToArray();                                            // O(n)

        if (!typeof(TDimension).IsAssignableTo(typeof(IHierarchicalDimension)))
            return (true, items.Select(x => new DimensionNode(x)).ToArray());

        if (items.Any(x => x.SystemName is null))                                                                 // O(n)
            return (false, nodes);

        var nodesWithParent = nodes.Where(x => ((IHierarchicalDimension)x.Item).Parent != null).ToArray();        // O(n)
        var itemsBySystemName = nodes.ToDictionary(x => x.Item.SystemName);                                       // O(n)
        var itemsByParent = nodesWithParent.GroupBy(x => ((IHierarchicalDimension)x.Item).Parent!)
                                           .ToDictionary(x => x.Key, x => x.ToHashSet());                         // O(n)

        if (nodesWithParent.Any(x => !itemsBySystemName.ContainsKey(((IHierarchicalDimension)x.Item).Parent!)))   // O(n)
            return (false, nodes);

        foreach (var node in nodes)                                                                               // O(n)
        {
            var parentSystemName = ((IHierarchicalDimension)node.Item).Parent?? Guid.NewGuid().ToString();
            itemsBySystemName.TryGetValue(parentSystemName, out var parent);                                      // O(1)
            itemsByParent.TryGetValue(node.Item.SystemName, out var children);                                    // O(1)
            itemsByParent.TryGetValue(parentSystemName, out var siblings);                                        // O(1)

            node.Parent = parent;
            node.Children = children?? [];
            node.AllAncestors = parent is null ? [] : [parent];
            node.AllChildren = (children?? []).ToHashSet();                                                       // O(children)
            node.Siblings = (siblings?? []).Except([node]).ToHashSet();                                           // O(siblings)
            node.Leaves = children is null ? [node] : [];
            node.Root = parent is null ? node : null;
        }

        foreach (var node in nodes.Where(x => !x.Children!.Any()))                                                // O(leaves)
        {
            var tmp = node;
            HashSet<DimensionNode> ancestors = [];

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

    public static (bool status, DimensionNode[] info) ExtractNodes(this IEnumerable<string> input)
    {
        return input.Select(x => new SimpleDimension(x)).ExtractNodes();
    }
}

public record SimpleDimension : Dimension
{
    public SimpleDimension(string x)
    {
        SystemName = x;
        DisplayName = x;
    }
}
