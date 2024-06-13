using Enterprise.Reporting.Utils;
using Microsoft.AspNetCore.Components;

namespace Enterprise.Reporting;

public class Report<TData> where TData : class
{
    internal Report(IEnumerable<TData> data)
    {
        Data = data.ToList();
        Accessor = PropertyAccessor<TData>.Instance;
    }

    internal List<TData> Data { get; set; }

    public PropertyAccessor<TData> Accessor { get; internal set; }

    public Func<IComponent, RenderFragment> GetFragment { get; internal set; }

    public DimensionsRegister DimensionsRegister { get; private set; } = DimensionsRegister.Instance;

    public List<string> ColumnsSlices { get; internal set; } = [];

    public List<string> RowsSlices { get; internal set; } = [];
}
