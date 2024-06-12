namespace Enterprise.Reporting.Utils.Tests;

[TestClass]
public class DimensionsRegisterTests
{
    [TestMethod]
    public void RegisterBasicUsage()
    {
        var register = DimensionsRegister.Instance;
        var lineOfBusinesses = TemplateModel.GetLineOfBusinesses();
        register.Insert(lineOfBusinesses);

        var status = register.TryGet<LineOfBusiness>("A", out var lobA);

        Assert.IsTrue(status, "Status is false");
        Assert.IsTrue(lobA?.Item.SystemName == "A");
    }

    [TestMethod]
    public void RegisterTryGetWithStringDimension()
    {
        var register = DimensionsRegister.Instance;
        var lineOfBusinesses = TemplateModel.GetLineOfBusinesses();
        register.Insert(lineOfBusinesses);

        var status = register.TryGet(nameof(LineOfBusiness), "A", out var lobA);

        Assert.IsTrue(status, "Status is false");
        Assert.IsTrue(lobA?.Item.SystemName == "A");
    }

    [TestMethod]
    public void HierarchyCorrectnessViaRegister()
    {
        var register = DimensionsRegister.Instance;
        var lineOfBusinesses = TemplateModel.GetLineOfBusinesses();
        register.Insert(lineOfBusinesses);

        var status = register.TryGet<LineOfBusiness>("A1", out var nodeA1);

        Assert.AreEqual(1, nodeA1?.Level, "A1 Level fails");
        Assert.AreEqual("A", nodeA1?.Parent?.Item.SystemName, "A1 Parent fails");
        Assert.IsTrue(nodeA1?.Children?.Select(x => x.Item.SystemName).SequenceEqual(["A11", "A12"]), "A1 Children fail");
        Assert.IsTrue(nodeA1?.AllChildren?.Select(x => x.Item.SystemName).Order().SequenceEqual(["A11", "A12"]), "A1 AllChildren fails");
        Assert.IsTrue(nodeA1?.AllAncestors?.Count() == 1, "A1 AllAncestors fails");
        Assert.IsTrue(nodeA1?.AllAncestors.First().Item.SystemName == "A", "A1 AllAncestors fails");
        Assert.IsTrue(nodeA1?.Siblings?.Select(x => x.Item.SystemName).Order().SequenceEqual(["A2"]), "A1 Siblings fails");
        Assert.IsTrue(nodeA1?.Root?.Item.SystemName == "A", "A1 Root fails");
        Assert.IsTrue(nodeA1?.Leaves?.Select(x => x.Item.SystemName).Order().SequenceEqual(["A11", "A12"]), "A1 Leaves fails");
    }
}