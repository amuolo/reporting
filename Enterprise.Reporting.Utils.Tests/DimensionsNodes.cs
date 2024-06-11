namespace Enterprise.Reporting.Utils.Tests;

[TestClass]
public class TestingDimensionNodes
{
    [TestMethod]
    public void DimensionNodes()
    {
        var currencies = new Currency[]
        {
            new() { SystemName = "CHF", DisplayName = "Swiss Francs" },
            new() { SystemName = "EUR", DisplayName = "Euro" }
        };

        var (status, items) = currencies.ExtractNodes();

        Assert.IsTrue(status);

        Assert.AreEqual("Swiss Francs", items.FirstOrDefault(x => x.Item.SystemName == "CHF")?.Item?.DisplayName);
        Assert.AreEqual(null, items.FirstOrDefault(x => x.Item.SystemName == "CHF")?.Children);
        Assert.AreEqual(null, items.FirstOrDefault(x => x.Item.SystemName == "CHF")?.Parent);
        Assert.AreEqual(null, items.FirstOrDefault(x => x.Item.SystemName == "CHF")?.Root);
        Assert.AreEqual(null, items.FirstOrDefault(x => x.Item.SystemName == "CHF")?.Leaves);
        Assert.AreEqual(0, items.FirstOrDefault(x => x.Item.SystemName == "CHF")?.Level);
    }

    [TestMethod]
    public void HierarchicalDimensionNodes()
    {
        var lineOfBusinesses = TemplateModel.GetLineOfBusinesses();

        var (status, items) = lineOfBusinesses.ExtractNodes();

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
    public void AdvancedHierarchicalNodes()
    {
        var lineOfBusinesses = TemplateModel.GetLineOfBusinesses();

        var (status, items) = lineOfBusinesses.ExtractNodes();

        Assert.IsTrue(status);

        var nodeA = items.FirstOrDefault(x => x.Item.SystemName == "A");

        Assert.AreEqual(0, nodeA?.Level, "A Level fails");
        Assert.AreEqual(null, nodeA?.Parent, "A Parent fails");
        Assert.IsTrue(nodeA?.Children?.Select(x => x.Item.SystemName).SequenceEqual(["A1", "A2"]), "A Children fail");
        Assert.IsTrue(nodeA?.AllChildren?.Select(x => x.Item.SystemName).OrderBy(x => x).SequenceEqual(["A1", "A11", "A12", "A2"]), "A AllChildren fails");
        Assert.IsTrue(nodeA?.AllAncestors?.Count() == 0, "A AllAncestors fails");
        Assert.IsTrue(nodeA?.Root?.Item.SystemName == "A", "A Root fails");
        Assert.IsTrue(nodeA?.Leaves?.Select(x => x.Item.SystemName).OrderBy(x => x).SequenceEqual(["A11", "A12", "A2"]), "A Leaves fails");

        var nodeA1 = items.FirstOrDefault(x => x.Item.SystemName == "A1");

        Assert.AreEqual(1, nodeA1?.Level, "A1 Level fails");
        Assert.AreEqual("A", nodeA1?.Parent?.Item.SystemName, "A1 Parent fails");
        Assert.IsTrue(nodeA1?.Children?.Select(x => x.Item.SystemName).SequenceEqual(["A11", "A12"]), "A1 Children fail");
        Assert.IsTrue(nodeA1?.AllChildren?.Select(x => x.Item.SystemName).OrderBy(x => x).SequenceEqual(["A11", "A12"]), "A1 AllChildren fails");
        Assert.IsTrue(nodeA1?.AllAncestors?.Count() == 1, "A1 AllAncestors fails");
        Assert.IsTrue(nodeA1?.AllAncestors.First().Item.SystemName == "A", "A1 AllAncestors fails");
        Assert.IsTrue(nodeA1?.Root?.Item.SystemName == "A", "A1 Root fails");
        Assert.IsTrue(nodeA1?.Leaves?.Select(x => x.Item.SystemName).OrderBy(x => x).SequenceEqual(["A11", "A12"]), "A1 Leaves fails");
    }

    [TestMethod]
    public void HierarchicalDimensionInfoWithIssues()
    {
        var lineOfBusinesses = TemplateModel.GetLineOfBusinesses();
        lineOfBusinesses[0].Parent = "X";

        var (status, items) = lineOfBusinesses.ExtractNodes();

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

        var (status, items) = nasties.ExtractNodes();

        Assert.IsTrue(status);

        Assert.AreEqual(nasties[0].Guid, items.FirstOrDefault(x => x.Item.SystemName == "A")?.Item?.Guid);
    }

    [TestMethod]
    public void MemoryFriendlyWithHierarchicalDimensions()
    {
        var nasties = new HierarchicalNasty[]
        {
            new() { SystemName = "A", DisplayName = "a", Parent = null },
            new() { SystemName = "A1", DisplayName = "a1", Parent = "A" },
        };

        var (status, items) = nasties.ExtractNodes();

        Assert.IsTrue(status);

        Assert.AreEqual(nasties[0].Guid, items.FirstOrDefault(x => x.Item.SystemName == "A")?.Item?.Guid);
    }
}
