namespace DataModel;

public abstract record Dimension
{
    public string SystemName { get; } = string.Empty;
    public string DisplayName { get; } = string.Empty;
}

public record LineOfBusiness : Dimension;

public record AmountType : Dimension;

public class TransactionalData
{
    public string LineOfBusiness { get; set; } = string.Empty;

    public string AmountType { get; set; } = string.Empty;

    public string Scenario { get; set; } = string.Empty;

    public double Value { get; set; }

}
