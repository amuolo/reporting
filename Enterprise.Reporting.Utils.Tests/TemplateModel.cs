namespace Enterprise.Reporting.Utils.Tests;

public record MyRecord(string A, string B, double V);

public record LineOfBusiness : HierarchicalDimension;

public record Currency : Dimension;

public record Nasty : Dimension { public Guid Guid { get; set; } = Guid.NewGuid(); };

public record HierarchicalNasty : HierarchicalDimension { public Guid Guid { get; set; } = Guid.NewGuid(); };

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
}



