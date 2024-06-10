
namespace Enterprise.Reporting.Utils.Tests;

[TestClass]
public class ExtractingDimensions
{
    public record MyRecord(string A, string B, double V);

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
}
