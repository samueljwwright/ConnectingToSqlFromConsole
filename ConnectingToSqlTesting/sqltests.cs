using ConnectingToSql;

namespace Tests.Tests;

[TestClass()]
public class sqltests
{
    [TestMethod()]
    public void queryCompilationTest()
    {
        DataSource d = new DataSource();
        Assert.IsNotNull(d);
    }

}