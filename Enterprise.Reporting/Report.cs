using Enterprise.Reporting.Utils;
using Microsoft.AspNetCore.Components;

namespace Enterprise.Reporting;

public class Report<TData> where TData : class
{
    internal Report(IEnumerable<TData> data)
    {
        DataNetwork = DataNetwork<TData>.Instance(data);
    }

    internal DataNetwork<TData> DataNetwork { get; set; }

    public Func<IComponent, RenderFragment> GetFragment { get; internal set; }

    public DimensionsRegister DimensionsRegister { get; private set; } = DimensionsRegister.Instance;

    public List<string> ColumnsSlices { get; internal set; } = [];

    public List<string> RowsSlices { get; internal set; } = [];
}
