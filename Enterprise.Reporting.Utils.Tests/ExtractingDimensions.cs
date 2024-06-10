using System.Linq;

namespace Enterprise.Reporting.Utils.Tests;

[TestClass]
public class ExtractingDimensions
{
    public record MyRecord(string A, string B, double V);

    public record LineOfBusiness : HierarchicalDimension;

    public record Currency : Dimension;

    public record Nasty : Dimension { public Guid Guid { get; set; } = Guid.NewGuid(); };

    public record HierarchicalNasty : HierarchicalDimension { public Guid Guid { get; set; } = Guid.NewGuid(); };

    [TestMethod]
    public void ExtractingFromRecord()
    {
        var obj1 = new MyRecord("xx", "yy", 10);
        var obj2 = new MyRecord("mm", "nn", 100);

        var extractor = DimensionExtractor<MyRecord>.Instance;

        Assert.AreEqual("xx", extractor.Get(obj1, "A"));
        Assert.AreEqual("yy", extractor.Get(obj1, "B"));
        Assert.AreEqual("mm", extractor.Get(obj2, "A"));
        Assert.AreEqual("nn", extractor.Get(obj2, "B"));

        Assert.AreEqual((double)10, extractor.Get(obj1, "V"));

        Assert.AreEqual(null, extractor.Get(obj1, "X"));
    }

    [TestMethod]
    public void GetDimensionInfo()
    {
        var currencies = new Currency[]
        {
            new() { SystemName = "CHF", DisplayName = "Swiss Francs" },
            new() { SystemName = "EUR", DisplayName = "Euro" }
        };

        var (status, items) = currencies.ExtractInfo();

        Assert.IsTrue(status);

        Assert.AreEqual("Swiss Francs", items.FirstOrDefault(x => x.Item.SystemName == "CHF")?.Item?.DisplayName);
        Assert.AreEqual(null, items.FirstOrDefault(x => x.Item.SystemName == "CHF")?.Children);
        Assert.AreEqual(null, items.FirstOrDefault(x => x.Item.SystemName == "CHF")?.Parent);
        Assert.AreEqual(null, items.FirstOrDefault(x => x.Item.SystemName == "CHF")?.Root);
        Assert.AreEqual(null, items.FirstOrDefault(x => x.Item.SystemName == "CHF")?.Leaves);
        Assert.AreEqual(0, items.FirstOrDefault(x => x.Item.SystemName == "CHF")?.Level);
    }

    [TestMethod]
    public void GetHierarchicalDimensionInfo()
    {
        var lineOfBusinesses = new LineOfBusiness[]
        {
            new() { SystemName = "A",   DisplayName = "a",   Parent = "" },
            new() { SystemName = "B",   DisplayName = "b",   Parent = "" },
            new() { SystemName = "A1",  DisplayName = "a1",  Parent = "A" },
            new() { SystemName = "A2",  DisplayName = "a2",  Parent = "A" },
            new() { SystemName = "A11", DisplayName = "a11", Parent = "A1" },
            new() { SystemName = "A12", DisplayName = "a12", Parent = "A1" },
        };

        var (status, items) = lineOfBusinesses.ExtractInfo();

        Assert.IsTrue(status);

        Assert.AreEqual(lineOfBusinesses.FirstOrDefault(x => x.SystemName == "B"), items.FirstOrDefault(x => x.Item.SystemName == "B")?.Item);
        Assert.AreEqual(lineOfBusinesses.FirstOrDefault(x => x.SystemName == "A"), items.FirstOrDefault(x => x.Item.SystemName == "A1")?.Parent?.Item);

        Assert.IsTrue(lineOfBusinesses.Where(x => x.Parent == "A1").SequenceEqual(
            items.FirstOrDefault(x => x.Item.SystemName == "A1")?.Children?.Select(x => x.Item)?? [])); 

        Assert.AreEqual(null, items.FirstOrDefault(x => x.Item.SystemName == "A")?.Parent?.Item.SystemName);
        Assert.AreEqual(null, items.FirstOrDefault(x => x.Item.SystemName == "B")?.Parent?.Item.SystemName);
        Assert.AreEqual("A", items.FirstOrDefault(x => x.Item.SystemName == "A1")?.Parent?.Item.SystemName);
        Assert.AreEqual("A", items.FirstOrDefault(x => x.Item.SystemName == "A2")?.Parent?.Item.SystemName);
        Assert.AreEqual("A1", items.FirstOrDefault(x => x.Item.SystemName == "A11")?.Parent?.Item.SystemName);
        Assert.AreEqual("A1", items.FirstOrDefault(x => x.Item.SystemName == "A12")?.Parent?.Item.SystemName);

        Assert.IsTrue(items.FirstOrDefault(x => x.Item.SystemName == "A")?.Children?.Select(x => x.Item.SystemName)?.SequenceEqual(["A1", "A2"])?? false);
        Assert.IsTrue(items.FirstOrDefault(x => x.Item.SystemName == "B")?.Children == null); 
        Assert.IsTrue(items.FirstOrDefault(x => x.Item.SystemName == "A1")?.Children?.Select(x => x.Item.SystemName)?.SequenceEqual(["A11", "A12"])?? false); 
        Assert.IsTrue(items.FirstOrDefault(x => x.Item.SystemName == "A2")?.Children == null);
        Assert.IsTrue(items.FirstOrDefault(x => x.Item.SystemName == "A11")?.Children == null);
        Assert.IsTrue(items.FirstOrDefault(x => x.Item.SystemName == "A12")?.Children == null);
    }

    [TestMethod]
    public void GetHierarchicalDimensionInfoWithIssues()
    {
        var lineOfBusinesses = new LineOfBusiness[]
        {
            new() { SystemName = "A",   DisplayName = "a",   Parent = "X" },
            new() { SystemName = "B",   DisplayName = "b",   Parent = "" },
            new() { SystemName = "A1",  DisplayName = "a1",  Parent = "A" },
            new() { SystemName = "A2",  DisplayName = "a2",  Parent = "A" },
            new() { SystemName = "A11", DisplayName = "a11", Parent = "A1" },
            new() { SystemName = "A12", DisplayName = "a12", Parent = "A1" },
        };

        var (status, items) = lineOfBusinesses.ExtractInfo();

        Assert.IsFalse(status);
    }

    [TestMethod]
    public void MemoryFriendlyWithDimensions()
    {
        var nasties = new Nasty[]
        {
            new() { SystemName = "A", DisplayName = "a" },
            new() { SystemName = "B", DisplayName = "b" },
        };

        var (status, items) = nasties.ExtractInfo();

        Assert.IsTrue(status);

        Assert.AreEqual(nasties[0].Guid, items.FirstOrDefault(x => x.Item.SystemName == "A")?.Item?.Guid);
    }

    [TestMethod]
    public void MemoryFriendlyWithHierarchicalDimensions()
    {
        var nasties = new HierarchicalNasty[]
        {
            new() { SystemName = "A", DisplayName = "a", Parent = "" },
            new() { SystemName = "A1", DisplayName = "a1", Parent = "A" },
        };

        var (status, items) = nasties.ExtractInfo();

        Assert.IsTrue(status);

        Assert.AreEqual(nasties[0].Guid, items.FirstOrDefault(x => x.Item.SystemName == "A")?.Item?.Guid);
    }
}
