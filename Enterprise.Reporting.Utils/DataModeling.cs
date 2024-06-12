using System.ComponentModel.DataAnnotations;

namespace Enterprise.Reporting.Utils;

public interface IDimension
{
    [Key]
    public string SystemName { get; set; }
    public string DisplayName { get; set; }
}

public interface IHierarchicalDimension : IDimension
{
    public string? Parent { get; set; }
}

public interface IOrderedDimension : IDimension
{
    public int Order { get; set; }
}

public interface IOrderedHierarchicalDimension : IHierarchicalDimension, IDimension
{
    public int Order { get; set; }
}

public abstract record Dimension : IDimension
{
    [Key]
    public string SystemName { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}

public abstract record HierarchicalDimension : Dimension, IHierarchicalDimension, IDimension
{
    public string? Parent { get; set; }
}

public abstract record OrderedDimension : Dimension, IOrderedDimension
{
    public int Order { get; set; }
}

public abstract record OrderedHierarchicalDimension : HierarchicalDimension, IOrderedHierarchicalDimension
{
    public int Order { get; set; }
}