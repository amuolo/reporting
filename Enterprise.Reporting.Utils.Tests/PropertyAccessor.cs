﻿namespace Enterprise.Reporting.Utils.Tests;

[TestClass]
public class PropertyAccessor
{
    [TestMethod]
    public void ExtractingFromRecord()
    {
        var obj1 = new MyRecord("xx", "yy", 10);
        var obj2 = new MyRecord("mm", "nn", 100);

        var extractor = PropertyAccessor<MyRecord>.Instance;

        Assert.AreEqual("xx", extractor.Get(obj1, "A"));
        Assert.AreEqual("yy", extractor.Get(obj1, "B"));
        Assert.AreEqual("mm", extractor.Get(obj2, "A"));
        Assert.AreEqual("nn", extractor.Get(obj2, "B"));

        Assert.AreEqual((double)10, extractor.Get(obj1, "V"));

        Assert.AreEqual(null, extractor.Get(obj1, "X"));
    }

    [TestMethod]
    public void ExtractStringProperties()
    {
        var obj1 = new MyRecord("xx", "yy", 10);
        var obj2 = new MyRecord("mm", "nn", 100);

        var extractor = PropertyAccessor<MyRecord>.Instance;

        Assert.AreEqual("xx_yy", extractor.GetStringProperties(obj1));
        Assert.AreEqual("mm_nn", extractor.GetStringProperties(obj2));
    }

    [TestMethod]
    public void GetStringPropertyNames()
    {
        var obj1 = new MyRecord("xx", "yy", 10);
        var obj2 = new MyRecord("mm", "nn", 100);

        var extractor = PropertyAccessor<MyRecord>.Instance;

        string[] propertyNames = ["A", "B"];

        Assert.IsTrue(PropertyAccessor<MyRecord>.StringPropertyNames.Order().SequenceEqual(propertyNames));
        Assert.IsTrue(PropertyAccessor<MyRecord>.StringPropertyNames.Order().SequenceEqual(propertyNames));
    }
}
