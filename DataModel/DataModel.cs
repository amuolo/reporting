using Enterprise.Reporting.Utils;

namespace DataModel;

public record LineOfBusiness : HierarchicalDimension;

public record AmountType : HierarchicalDimension;

public record Scenario : Dimension;

public record AocType : Dimension;

public class TransactionalData
{
    public string LineOfBusiness { get; set; } = string.Empty;

    public string AmountType { get; set; } = string.Empty;

    public string Scenario { get; set; } = string.Empty;

    public string AocType { get; set; } = string.Empty;

    public string Currency { get; set; } = string.Empty;

    public double Value { get; set; }

}
