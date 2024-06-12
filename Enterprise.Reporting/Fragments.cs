using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Enterprise.Reporting;

internal static class ReportFragments
{
    internal static void Update<TData>(
        this Report<TData> report,
        Action update,
        int i = -1,
        int j = -1
        ) where TData : class
    {
        report.GetFragment = report.Generate(update, i, j);
        update();
    }

    internal static Func<IComponent, RenderFragment> Generate<TData>(
        this Report<TData> report,
        Action update,
        int i = -1,
        int j = -1
        ) where TData : class
    {
        Func<IComponent, RenderFragment> lambda = (IComponent owner) => (builder) =>
        {
            builder.OpenElement(0, "style");
            builder.AddMarkupContent(0, ".dimZoom { transition: transform .2s; width: 1px; height: 1px; margin: 0 auto; }");
            builder.AddMarkupContent(0, ".dimZoom:hover { -ms-transform: scale(1.1); -webkit-transform: scale(1.1); transform: scale(1.1); }");
            builder.CloseElement();

            builder.OpenElement(10, "table");
            builder.AddAttribute(10, "class", "table");
            builder.OpenElement(11, "thead");
            builder.OpenElement(12, "tr");

            builder.OpenElement(13, "th");
            builder.AddAttribute(10, "class", "dimZoom");
            builder.AddAttribute(14, "onclick", EventCallback.Factory.Create(owner, () => report.Update(update, i < 0 ? 4 : ++i, ++i)));
            builder.AddEventPreventDefaultAttribute(4, "onclick", true);
            builder.AddMarkupContent(16, "A");
            builder.CloseElement();

            builder.OpenElement(17, "th");
            builder.AddMarkupContent(17, "B");
            builder.CloseElement();

            builder.OpenElement(18, "th");
            builder.AddMarkupContent(18, "C");
            builder.CloseElement();

            builder.CloseElement();
            builder.CloseElement();

            builder.OpenElement(60, "tbody");

            foreach (var x in Enumerable.Range(0, i < 0 ? 4 : i))
            {
                builder.OpenElement(61, "tr");
                builder.AddMarkupContent(71, $"<td>{x+j}</td>");
                builder.AddMarkupContent(81, $"<td>{x+j}</td>");
                builder.AddMarkupContent(91, $"<td>{x+j}</td>");
                builder.CloseElement();
            }

            builder.CloseElement();
            builder.CloseElement();
        };

        return lambda;
    }

}


