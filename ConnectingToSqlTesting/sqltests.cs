using ConnectingToSql;
using Microsoft.Data.SqlClient;
namespace Tests.Tests;

[TestClass()]
public class sqltests
{
    [TestMethod()]
    public void queryCompilationTest()
    {
        List<string> cols = new List<string>();
        cols.Add("Actor_ID");
        cols.Add("First_Name");
        cols.Add("Last_Name");

        string server = "127.0.0.1,1433";
        string db = "MoviesAndActors";
        string username = "SA";
        string password = "P455word123!";
        var sql = "SELECT * FROM Movies";


        SqlConnection connection = new SqlConnection($"Server={server};" +
            $"TrustServerCertificate = True; Database = {db};" +
            $"User Id = {username}; Password = {password};");
        connection.Open();

        DataSource d = new DataSource();
        Assert.AreEqual(d.GetColumns(connection, "Actors")[0] , cols[0]);
    }

}