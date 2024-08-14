using System.Runtime.ExceptionServices;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.Data.SqlClient;

[MemoryDiagnoser]
public class Program
{
    private static void Main(string[] args)
    {
        var b = BenchmarkRunner.Run<Program>();
    }

    [Benchmark]
    public void StringAppending()
    {
        string server = "127.0.0.1,1433";
        string db = "MoviesAndActors";
        string username = "SA";
        string password = "P455word123!";
        var sql = "SELECT * FROM Movies";

        SqlConnection connection = new SqlConnection($"Server={server};" +
            $"TrustServerCertificate = True; Database = {db};" +
            $"User Id = {username}; Password = {password};");

        connection.Open();

    }

    [Benchmark]
    public void StringBuilder()
    {


        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        builder.DataSource = "127.0.0.1,1433";
        builder.InitialCatalog = "MoviesAndActors";
        builder.TrustServerCertificate = true;
        builder.UserID = "SA";
        builder.Password = "P455word123!";

        string connectionString = builder.ConnectionString;

        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
    }

    //| Method          | Mean     | Error    | StdDev   | Allocated |
    //|---------------- |---------:|---------:|---------:|----------:|
    //| StringAppending | 25.26 us | 2.374 us | 6.811 us |   2.32 KB |
    //| StringBuilder   | 45.45 us | 1.953 us | 5.634 us |   2.67 KB |

    //String appending using regular string connecting is typically slower and less effiecient in terms of mem usage
    //However given the lack of opersation the time taken and mem required is low 
    //SQL connection string builder performed worse in termsof time and space but the difference is minimal 
}