using Enterprise.Reporting.Utils;
using Microsoft.AspNetCore.Components;

namespace Enterprise.Reporting;

public class Report<TData> where TData : class
{
    internal Report(IEnumerable<TData> data)
    {
        DataTensor = DataTensor<TData>.Instance(data);
        DimensionNames = DataTensor.DimensionNames;
        ColumnsSlices.EnsureCapacity(DimensionNames.Length);
        RowsSlices.EnsureCapacity(DimensionNames.Length);
    }

    public DataTensor<TData> DataTensor { get; internal set; }

    public string[] DimensionNames { get; internal set; }

    public Func<IComponent, RenderFragment> GetFragment { get; internal set; }

    public List<string> ColumnsSlices { get; internal set; } = [];

    public List<string> RowsSlices { get; internal set; } = [];
}
