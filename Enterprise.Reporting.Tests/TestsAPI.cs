
namespace Enterprise.Reporting.Tests;

[TestClass]
public class TestsAPI
{
    [TestMethod]
    public void SettingUpReport()
    {
        var data = TemplateData.GetTransactionalData();
        var report = Reporting.Create(data).InitiateReport(() => { });
    }
}
