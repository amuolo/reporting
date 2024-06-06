using Microsoft.AspNetCore.Components;

namespace Enterprise.Reporting;

public static class Reporting
{
    public static Report<TData> Create<TData>(IEnumerable<TData> data) where TData : class
    {
        return new Report<TData>() { Data = data.ToList() };
    }

    public static RenderFragment GetReport<TData>(this Report<TData> report, int i = -1, int j = -1) where TData : class
    {
        RenderFragment fragment = (builder) =>
        {
            builder.AddMarkupContent(0, "<table class='table'>");
            builder.OpenElement(1, "thead");
            builder.OpenElement(2, "tr");
            builder.AddMarkupContent(3, "<th @onclick = 'UpdateReport'>A</th>");
            builder.AddMarkupContent(4, "<th>B</th>");
            builder.AddMarkupContent(5, "<th>C</th>");
            builder.CloseElement();
            builder.CloseElement();
            builder.OpenElement(1, "tbody");
            
            foreach (var i in Enumerable.Range(0, 4))
            {
                builder.OpenElement(6, "tr");
                builder.AddMarkupContent(7, $"{i}");
                builder.AddMarkupContent(8, $"{i}");
                builder.AddMarkupContent(9, $"{i}");
                builder.CloseElement();
            }
            
            builder.CloseElement();
            builder.AddMarkupContent(10, "</table>");
        };


        return fragment;
    }
}


