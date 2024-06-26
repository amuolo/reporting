namespace Enterprise.Reporting.Utils;

public class DataTensor<TData> where TData : class
{
    private DataTensor(IEnumerable<TData> data)
    {
        Data = data.ToList();
        Accessor = PropertyAccessor<TData>.Instance;
    }

    private List<TData> Data { get; set; }

    public bool Initialized { get; private set; } = false;

    public string[] DimensionNames { get; } = PropertyAccessor<TData>.StringPropertyNames;

    public static DataTensor<TData> Instance<TData>(IEnumerable<TData> data) where TData : class => new DataTensor<TData>(data);

    public PropertyAccessor<TData> Accessor { get; internal set; }

    public DimensionsRegister DimensionsRegister { get; private set; } = DimensionsRegister.Instance;

    public bool IncludeDimension<TDimension>(IEnumerable<TDimension> dimensions) where TDimension : IDimension
    {
        DimensionsRegister.Insert(dimensions);
        return true;
    }

    public void Initialize()
    {
        if (Initialized) return;

        if (DimensionsRegister.Size() == 0)
            InitializeDimensionsRegister();
        
        // evaluate aggregates of aggregates

        Initialized = true;
    }

    private void InitializeDimensionsRegister()
    {
        // TODO
    }
}
