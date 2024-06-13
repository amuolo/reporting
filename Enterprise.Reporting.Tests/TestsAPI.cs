
namespace Enterprise.Reporting.Tests;

[TestClass]
public class TestsAPI
{
    [TestMethod]
    public void SettingUpReport()
    {
        var data = TemplateData.GetTransactionalData();
        var report = Reporting.Create(data);

        Assert.IsFalse(report.DataNetwork.Initialized);
        
        report.InitiateReport(() => { });

        Assert.IsTrue(report.DataNetwork.Initialized);
    }

    [TestMethod]
    public void DimensionNames()
    {
        var data = TemplateData.GetTransactionalData();
        var report = Reporting.Create(data);

        Assert.IsTrue(report.DimensionNames.Order().SequenceEqual([
            nameof(TransactionalData.AmountType), 
            nameof(TransactionalData.AocType),
            nameof(TransactionalData.Currency),
            nameof(TransactionalData.LineOfBusiness),
            nameof(TransactionalData.Scenario) ]));
    }

    [TestMethod]
    public void BaseTestColsRowsSlices()
    {
        var data = TemplateData.GetTransactionalData();
        var report = Reporting.Create(data);

        Assert.IsTrue(report.ColumnsSlices.SequenceEqual([]));
        Assert.IsTrue(report.RowsSlices.SequenceEqual([]));

        Assert.IsFalse(report.SliceColumnsBy("B", "A"));

        Assert.IsTrue(report.ColumnsSlices.SequenceEqual([]));
        Assert.IsTrue(report.RowsSlices.SequenceEqual([]));

        Assert.IsFalse(report.SliceRowsBy("Z", "J"));

        Assert.IsTrue(report.ColumnsSlices.SequenceEqual([]));
        Assert.IsTrue(report.RowsSlices.SequenceEqual([]));

        Assert.IsTrue(report.SliceColumnsBy(nameof(TransactionalData.LineOfBusiness)));

        Assert.IsTrue(report.ColumnsSlices.SequenceEqual([nameof(TransactionalData.LineOfBusiness)]));
        Assert.IsTrue(report.RowsSlices.SequenceEqual([]));

        Assert.IsTrue(report.SliceRowsBy("aca", "cac", nameof(TransactionalData.Currency)));

        Assert.IsTrue(report.ColumnsSlices.SequenceEqual([nameof(TransactionalData.LineOfBusiness)]));
        Assert.IsTrue(report.RowsSlices.SequenceEqual([nameof(TransactionalData.Currency)]));

        Assert.IsFalse(report.SliceColumnsBy(nameof(TransactionalData.LineOfBusiness)));

        Assert.IsTrue(report.ColumnsSlices.SequenceEqual([nameof(TransactionalData.LineOfBusiness)]));
        Assert.IsTrue(report.RowsSlices.SequenceEqual([nameof(TransactionalData.Currency)]));

        Assert.IsTrue(report.SliceRowsBy("aca", "cac", nameof(TransactionalData.LineOfBusiness), nameof(TransactionalData.Scenario)));

        Assert.IsTrue(report.ColumnsSlices.SequenceEqual([nameof(TransactionalData.LineOfBusiness)]));
        Assert.IsTrue(report.RowsSlices.SequenceEqual([nameof(TransactionalData.Currency), nameof(TransactionalData.Scenario)]));
    }
}
