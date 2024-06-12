﻿using Enterprise.Reporting.Utils;

namespace Enterprise.Reporting;

public static class Reporting
{
    public static Report<TData> Create<TData>(IEnumerable<TData> data) where TData : class
    {
        return new Report<TData>() { Data = data.ToList() };
    }

    public static Report<TData> InitiateReport<TData>(
        this Report<TData> report,
        Action update) 
        where TData : class
    {
        report.GetFragment = report.Generate(update);
        return report;
    }

    public static Report<TData> IncludeDimension<TData, TDimension>(
        this Report<TData> report,
        IEnumerable<TDimension> dimensions)
        where TData : class
        where TDimension : IDimension
    {
        report.DimensionsRegister.Insert(dimensions);
        return report;
    }
}
