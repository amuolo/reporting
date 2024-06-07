using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;

namespace Enterprise.Reporting;

public static class Reporting
{
    public static Report<TData> Create<TData>(IEnumerable<TData> data) where TData : class
    {
        return new Report<TData>() { Data = data.ToList() };
    }

    public static Func<IComponent, RenderFragment> GenerateFragment<TData>(
        this Report<TData> report,
        Action update,
        int i = -1,
        int j = -1
        ) where TData : class
    {
        Func<IComponent, RenderFragment> lambda = (IComponent owner) => (builder) =>
        {
            builder.OpenElement(0, "table");
            builder.AddAttribute(0, "class", "table");
            builder.OpenElement(1, "thead");
            builder.OpenElement(2, "tr");

            builder.OpenElement(3, "th");
            builder.AddAttribute(3, "onclick", EventCallback.Factory.Create(owner, () => report.UpdateReport(update, i < 0 ? 4 : ++i, ++i)));
            builder.AddEventPreventDefaultAttribute(3, "onclick", true);
            builder.AddMarkupContent(3, "A");
            builder.CloseElement();

            builder.OpenElement(4, "th");
            builder.AddMarkupContent(4, "B");
            builder.CloseElement();

            builder.OpenElement(5, "th");
            builder.AddMarkupContent(5, "C");
            builder.CloseElement();

            builder.CloseElement();
            builder.CloseElement();

            builder.OpenElement(6, "tbody");

            foreach (var x in Enumerable.Range(0, i < 0 ? 4 : i))
            {
                builder.OpenElement(6, "tr");
                builder.AddMarkupContent(7, $"<td>{x+j}</td>");
                builder.AddMarkupContent(8, $"<td>{x+j}</td>");
                builder.AddMarkupContent(9, $"<td>{x+j}</td>");
                builder.CloseElement();
            }

            builder.CloseElement();
            builder.CloseElement();
        };

        return lambda;
    }

    public static void UpdateReport<TData>(
        this Report<TData> report, 
        Action update,
        int i = -1, 
        int j = -1
        ) where TData : class
    {
        report.GetFragment = report.GenerateFragment(update, i, j);
        update();
    }
    
    public static Report<TData> InitiateReport<TData>(
        this Report<TData> report,
        Action update
        ) where TData : class
    {
        report.GetFragment = report.GenerateFragment(update);

        return report;
    }
}


