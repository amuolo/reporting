namespace Enterprise.Reporting.Utils;

public class DimensionsRegister
{
    private Dictionary<string, Dictionary<string, DimensionNode>> Store { get; set; } = [];

    private DimensionsRegister() { }

    public static DimensionsRegister Instance => new DimensionsRegister();

    public int Size() => Store.Count;

    public bool Insert<TDimension>(IEnumerable<TDimension> items) where TDimension : IDimension
    {
        var (status, nodes) = items.ExtractNodes();
        if (status)
            Store[typeof(TDimension).Name] = nodes.ToDictionary(x => x.Item.SystemName);
        return status;
    }    

    public bool TryGet<TDimension>(string systemName, out DimensionNode? node) where TDimension : IDimension
    {
        return TryGet(typeof(TDimension).Name, systemName, out node);
    }

    public bool TryGet(string dimension, string systemName, out DimensionNode? node)
    {
        node = null;
        var status = Store.TryGetValue(dimension, out var nodesBySystemName);
        if (status && nodesBySystemName is not null)
        {
            status = nodesBySystemName.TryGetValue(systemName, out var item);
            if (status && item is not null)
                node = item;
            status = item is not null;
        }
        return status;
    }
}
