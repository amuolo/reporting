namespace Enterprise.Reporting.Utils;

public abstract record Dimension
{
    public string SystemName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}

public abstract record HierarchicalDimension : Dimension
{
    public string Parent { get; set; } = string.Empty;
}



