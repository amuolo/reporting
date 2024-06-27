namespace Enterprise.Reporting.Utils;

public class DataTensor<TData> where TData : class
{
    private struct IndexedData
    {
        public TData Data { get; set; }

        public Dictionary<string, DimensionNode> Nodes { get; set; }
    }

    private DataTensor(IEnumerable<TData> data)
    {
        Accessor = PropertyAccessor<TData>.Instance;
        RawData = data;
    }

    private IEnumerable<TData> RawData { get; set; }

    private List<IndexedData> Data { get; set; }

    public bool Initialized { get; private set; } = false;

    public string[] DimensionNames { get; } = PropertyAccessor<TData>.StringPropertyNames;

    public static DataTensor<TData> Instance<TData>(IEnumerable<TData> data) where TData : class => new DataTensor<TData>(data);

    public PropertyAccessor<TData> Accessor { get; internal set; }

    public DimensionsRegister DimensionsRegister { get; private set; } = DimensionsRegister.Instance;

    public string? Error { get; private set; }

    public bool IncludeDimension<TDimension>(IEnumerable<TDimension> dimensions) where TDimension : IDimension
    {
        DimensionsRegister.Insert(dimensions);
        return true;
    }

    public bool Initialize()
    {
        // O(1)
        DimensionNode GetNode(TData obj, string dim)
        {
            DimensionsRegister.TryGet(dim, (string)Accessor.Get(obj, dim), out var node);
            return node?? throw new Exception($"Dimension Not Found {dim}");
        }

        if (Initialized) return true;

        if (DimensionsRegister.Size() == 0)
            DefaultInitializeDimensionsRegister();

        if (Error is not null) return false;

        try
        {
            // O(size(data)*size(num_dimensions))
            Data = RawData.Select(x => new IndexedData { Data = x, Nodes = DimensionNames.ToDictionary(dim => dim, dim => GetNode(x, dim)) }).ToList();
        }
        catch (Exception ex)
        {
            Error = ex.Message;            
        }

        Initialized = Error is null;
        return Error is null;
    }

    private void DefaultInitializeDimensionsRegister()
    {
        foreach (var dim in DimensionNames)
        {
            var names = RawData.Select(x => (string)Accessor.Get(x, dim)).Distinct();
            DimensionsRegister.Insert(dim, names);
        }
    }
}

