﻿namespace Enterprise.Reporting.Utils;

public class DataNetwork<TData> where TData : class
{
    private DataNetwork(IEnumerable<TData> data)
    {
        Data = data.ToList();
        Accessor = PropertyAccessor<TData>.Instance;
    }

    private List<TData> Data { get; set; }

    public static DataNetwork<TData> Instance<TData>(IEnumerable<TData> data) where TData : class => new DataNetwork<TData>(data);

    public PropertyAccessor<TData> Accessor { get; internal set; }

    public DimensionsRegister DimensionsRegister { get; private set; } = DimensionsRegister.Instance;
}
