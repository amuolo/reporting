using Microsoft.AspNetCore.Components;

namespace Enterprise.Reporting;

public class Report<TData> where TData : class
{
    internal List<TData> Data { get; set; }

    public Func<IComponent, RenderFragment> GetFragment { get; set; }
}
