namespace Enterprise.Reporting;

public static class Reporting
{

    public static Report<TData> GetReport<TData>(IEnumerable<TData> data) where TData : class
    {
        return new Report<TData>() { Data = data.ToList() };
    }

    public static Report<TData> GetReport<TData>(this Report<TData> report) where TData : class
    {
        return report;
    }
}


