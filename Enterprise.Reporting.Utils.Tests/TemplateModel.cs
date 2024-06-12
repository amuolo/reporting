namespace Enterprise.Reporting.Utils.Tests;

public record MyRecord(string A, string B, double V);

public record LineOfBusiness : HierarchicalDimension;

public record Currency : Dimension;

public static class TemplateModel
{
    public static LineOfBusiness[] GetLineOfBusinesses() =>
        [
            new() { SystemName = "A",   DisplayName = "a",   Parent = null },
            new() { SystemName = "B",   DisplayName = "b",   Parent = null },
            new() { SystemName = "A1",  DisplayName = "a1",  Parent = "A" },
            new() { SystemName = "A2",  DisplayName = "a2",  Parent = "A" },
            new() { SystemName = "A11", DisplayName = "a11", Parent = "A1" },
            new() { SystemName = "A12", DisplayName = "a12", Parent = "A1" },
        ];

    public static Currency[] GetCurrencies() =>
        [
            new() { SystemName = "CHF", DisplayName = "Swiss Francs" },
            new() { SystemName = "EUR", DisplayName = "Euro" }
        ];
}



