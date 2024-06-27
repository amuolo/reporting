namespace Enterprise.Reporting.Utils.Tests;

[TestClass]
public class TestingDimensionNodes
{
    [TestMethod]
    public void DimensionNodes()
    {
        var currencies = TemplateModel.GetCurrencies();

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
        Assert.IsTrue(items.FirstOrDefault(x => x.Item.SystemName == "B")?.Children?.Count() == 0); 
        Assert.IsTrue(items.FirstOrDefault(x => x.Item.SystemName == "A1")?.Children?.Select(x => x.Item.SystemName)?.SequenceEqual(["A11", "A12"])?? false); 
        Assert.IsTrue(items.FirstOrDefault(x => x.Item.SystemName == "A2")?.Children?.Count() == 0);
        Assert.IsTrue(items.FirstOrDefault(x => x.Item.SystemName == "A11")?.Children?.Count() == 0);
        Assert.IsTrue(items.FirstOrDefault(x => x.Item.SystemName == "A12")?.Children?.Count() == 0);
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
        Assert.IsTrue(nodeA?.Children?.Select(x => x.Item.SystemName).Order().SequenceEqual(["A1", "A2"]), "A Children fail");
        Assert.IsTrue(nodeA?.AllChildren?.Select(x => x.Item.SystemName).Order().SequenceEqual(["A1", "A11", "A12", "A2"]), "A AllChildren fails");
        Assert.IsTrue(nodeA?.AllAncestors?.Count() == 0, "A AllAncestors fails");
        Assert.IsTrue(nodeA?.Siblings?.Count() == 0, "A Siblings fails");
        Assert.IsTrue(nodeA?.Root?.Item.SystemName == "A", "A Root fails");
        Assert.IsTrue(nodeA?.Leaves?.Select(x => x.Item.SystemName).OrderBy(x => x).SequenceEqual(["A11", "A12", "A2"]), "A Leaves fails");

        var nodeA1 = items.FirstOrDefault(x => x.Item.SystemName == "A1");

        Assert.AreEqual(1, nodeA1?.Level, "A1 Level fails");
        Assert.AreEqual("A", nodeA1?.Parent?.Item.SystemName, "A1 Parent fails");
        Assert.IsTrue(nodeA1?.Children?.Select(x => x.Item.SystemName).SequenceEqual(["A11", "A12"]), "A1 Children fail");
        Assert.IsTrue(nodeA1?.AllChildren?.Select(x => x.Item.SystemName).Order().SequenceEqual(["A11", "A12"]), "A1 AllChildren fails");
        Assert.IsTrue(nodeA1?.AllAncestors?.Count() == 1, "A1 AllAncestors fails");
        Assert.IsTrue(nodeA1?.AllAncestors.First().Item.SystemName == "A", "A1 AllAncestors fails");
        Assert.IsTrue(nodeA1?.Siblings?.Select(x => x.Item.SystemName).Order().SequenceEqual(["A2"]), "A1 Siblings fails");
        Assert.IsTrue(nodeA1?.Root?.Item.SystemName == "A", "A1 Root fails");
        Assert.IsTrue(nodeA1?.Leaves?.Select(x => x.Item.SystemName).Order().SequenceEqual(["A11", "A12"]), "A1 Leaves fails");

        var nodeA11 = items.FirstOrDefault(x => x.Item.SystemName == "A11");

        Assert.AreEqual(2, nodeA11?.Level, "A11 Level fails");
        Assert.AreEqual("A1", nodeA11?.Parent?.Item.SystemName, "A11 Parent fails");
        Assert.IsTrue(nodeA11?.Children?.Count() == 0, "A11 Children fail");
        Assert.IsTrue(nodeA11?.AllChildren?.Count() == 0, "A11 AllChildren fails");
        Assert.IsTrue(nodeA11?.AllAncestors?.Select(x => x.Item.SystemName).Order().SequenceEqual(["A", "A1"]), "A11 AllAncestors fails");
        Assert.IsTrue(nodeA11?.Siblings?.Select(x => x.Item.SystemName).Order().SequenceEqual(["A12"]), "A11 Siblings fails");
        Assert.IsTrue(nodeA11?.Root?.Item.SystemName == "A", "A11 Root fails");
        Assert.IsTrue(nodeA11?.Leaves?.Count() == 1, "A11 Leaves fails");
        Assert.IsTrue(nodeA11?.Leaves?.First().Item.SystemName == "A11", "A11 Leaves fails");

        var nodeA2 = items.FirstOrDefault(x => x.Item.SystemName == "A2");

        Assert.AreEqual(1, nodeA2?.Level, "A2 Level fails");
        Assert.AreEqual("A", nodeA2?.Parent?.Item.SystemName, "A2 Parent fails");
        Assert.IsTrue(nodeA2?.Children?.Count() == 0, "A2 Children fail");
        Assert.IsTrue(nodeA2?.AllChildren?.Count() == 0, "A2 AllChildren fails");
        Assert.IsTrue(nodeA2?.AllAncestors?.Select(x => x.Item.SystemName).Order().SequenceEqual(["A"]), "A2 AllAncestors fails");
        Assert.IsTrue(nodeA2?.Siblings?.Select(x => x.Item.SystemName).Order().SequenceEqual(["A1"]), "A2 Siblings fails");
        Assert.IsTrue(nodeA2?.Root?.Item.SystemName == "A", "A2 Root fails");
        Assert.IsTrue(nodeA2?.Leaves?.Count() == 1, "A2 Leaves fails");
        Assert.IsTrue(nodeA2?.Leaves?.First().Item.SystemName == "A2", "A2 Leaves fails");
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
        var nasties = TemplateModel.GetCurrencies();

        var (status, items) = nasties.ExtractNodes();

        Assert.IsTrue(status);

        Assert.AreEqual(nasties[0].GetHashCode(), items.FirstOrDefault(x => x.Item.SystemName == "CHF")?.Item?.GetHashCode());
    }

    [TestMethod]
    public void MemoryFriendlyWithHierarchicalDimensions()
    {
        var nasties = TemplateModel.GetLineOfBusinesses();

        var (status, items) = nasties.ExtractNodes();

        Assert.IsTrue(status);

        Assert.AreEqual(nasties[0].GetHashCode(), items.FirstOrDefault(x => x.Item.SystemName == "A")?.Item?.GetHashCode());
    }

    [TestMethod]
    public void SimpleDimensionsToNodes()
    {
        var dims = new string[] { "LI", "NL" };

        var (status, items) = dims.ExtractNodes();

        Assert.IsTrue(status);

        Assert.AreEqual("LI", items.FirstOrDefault(x => x.Item.SystemName == "LI")?.Item?.DisplayName);

        Assert.AreEqual("NL", items.FirstOrDefault(x => x.Item.SystemName == "NL")?.Item?.DisplayName);
    }
}
