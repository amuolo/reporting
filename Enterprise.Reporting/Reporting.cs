using Enterprise.Reporting.Utils;

namespace Enterprise.Reporting;

public static class Reporting
{
    public static Report<TData> Create<TData>(IEnumerable<TData> data) where TData : class => new Report<TData>(data);

    public static Report<TData> InitiateReport<TData>(
        this Report<TData> report,
        Action update) 
        where TData : class
    {
        report.DataTensor.Initialize();
        report.GetFragment = report.Generate(update);
        return report;
    }

    public static bool IncludeDimension<TData, TDimension>(
        this Report<TData> report,
        IEnumerable<TDimension> dimensions)
        where TData : class
        where TDimension : IDimension
    {
        return report.DataTensor.IncludeDimension(dimensions);
    }

    public static bool SliceColumnsBy<TData>(
        this Report<TData> report,
        params string[] slices)
        where TData : class
    {
        slices = slices.Where(s => report.DimensionNames.Contains(s) && !report.ColumnsSlices.Contains(s) && !report.RowsSlices.Contains(s)).ToArray();
        if (!slices.Any())
            return false;
        foreach (var slice in slices) 
            report.ColumnsSlices.Add(slice);
        report.ColumnsSlices.Distinct();
        return true;
    }

    public static bool SliceRowsBy<TData>(
        this Report<TData> report,
        params string[] slices)
        where TData : class
    {
        slices = slices.Where(s => report.DimensionNames.Contains(s) && !report.ColumnsSlices.Contains(s) && !report.RowsSlices.Contains(s)).ToArray();
        if (!slices.Any())
            return false;
        foreach (var slice in slices)
            report.RowsSlices.Add(slice);
        report.RowsSlices.Distinct();
        return true;
    }
}
