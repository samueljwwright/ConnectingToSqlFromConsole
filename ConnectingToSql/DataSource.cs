using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectingToSql
{
    public class DataSource
    {
        public SqlConnection CreateConnection()
        {

            string server = "127.0.0.1,1433";
            string db = "MoviesAndActors";
            string username = "SA";
            string password = "P455word123!";
            var sql = "SELECT * FROM Movies";


            SqlConnection connection = new SqlConnection($"Server={server};" +
                $"TrustServerCertificate = True; Database = {db};" +
                $"User Id = {username}; Password = {password};");

            //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            //builder.DataSource = "127.0.0.1,1433";
            //builder.InitialCatalog = "MoviesAndActors";
            //builder.TrustServerCertificate = true;
            //builder.UserID = "SA";
            //builder.Password = "P455word123!";

            //string connectionString = builder.ConnectionString;

            //SqlConnection conn = new SqlConnection(connectionString);





            connection.Open();

            return connection;

            


            
        }


        public void CloseConnection(SqlConnection connection)
        {
            connection.Close();
        }

        //View overloads

        //All entries and all attrbutes
        public void View(SqlConnection connection, string table)
        {
            string query = $"SELECT * FROM {table}";
            SqlCommand cmd = new SqlCommand(query, connection);
            var reader = cmd.ExecuteReader();          
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    Console.Write($"{reader[i]} |");
                }
                Console.WriteLine();
            }
            reader.Close();
        }

        //Specified entry count with all attributes
        public void View(SqlConnection connection, string table, uint count)
        {
            string query = $"SELECT * FROM {table}";
            SqlCommand cmd = new SqlCommand(query, connection);
            var reader = cmd.ExecuteReader();

            int iter = 0;
            while (reader.Read() && iter < count)
            {
                for (int i =0; i < reader.FieldCount; i++)
                {
                    Console.Write($"{reader[i]} |");
                }
                Console.WriteLine();
                iter++;
            }
            reader.Close();
        }

        //all entries to specific attributes
        public void View(SqlConnection connection, string table, string[] attributes)
        {
            string query = $"SELECT * FROM {table}";
            SqlCommand cmd = new SqlCommand(query, connection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                for (int i = 0; i < attributes.Length; i++)
                { 
                    Console.Write($"{reader[attributes[i]]} |");
                }
                Console.WriteLine();
            }
            reader.Close();
        }

        //Specified entries and specifiec attributes
        public void View(SqlConnection connection, string table, uint count ,string[] attributes)
        {
            string query = $"SELECT * FROM {table}";
            SqlCommand cmd = new SqlCommand(query, connection);
            var reader = cmd.ExecuteReader();
            int iter = 0;
            while (reader.Read() && iter < count)
            {
                for (int i = 0; i < attributes.Length; i++)
                {
                    Console.Write($"{reader[attributes[i]]} |");
                }
                Console.WriteLine();
                iter++;
            }
            reader.Close();
        }


        //Gets all col names for given table
        public List<string> GetColumns(SqlConnection connection, string table)
        {
            List<string> cols = new List<string>();
            string query = $"SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table}'";
            SqlCommand cmd = new SqlCommand(query, connection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //Console.WriteLine(reader["COLUMN_NAME"]);
                cols.Add(reader["COLUMN_NAME"].ToString());
                
            }
            reader.Close();
            return cols;
        }

        public List<string> getTables(SqlConnection connection)
        {
            DataTable dt = connection.GetSchema("Tables");
            List<string> tables = new List<string>();
            for (int i =0; i < dt.Rows.Count; i++)
            {
                string tablename = dt.Rows[i][2].ToString(); // second index of each row is name 
                tables.Add(tablename);
                //Console.WriteLine(tablename);
            }
            
            return tables; 
        }

        //Change to single mthod with get column
        public List<string> GetColumnData(SqlConnection connection, string table)
        {
            List<string> cols = new List<string>();
            string query = $"SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{table}'";
            SqlCommand cmd = new SqlCommand(query, connection);
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                //Console.WriteLine(reader["COLUMN_NAME"]);
                cols.Add(reader["DATA_TYPE"].ToString());

            }
            reader.Close();
            return cols;
        }


        public void Insert(SqlConnection connection, string table,
                            Dictionary<string, string> valuesToFields)
        {
            string fields = string.Join(", ", valuesToFields.Keys);
            string values = string.Join(", ", valuesToFields.Values);

            //foreach (var l in valuesToFields)
            //{
            //    Console.WriteLine(l.Key + " " + l.Value);
            //}

            string query = $"INSERT INTO {table} ({fields}) VALUES ({values})";
            SqlCommand cmd = new SqlCommand(query, connection);
            cmd.ExecuteNonQuery();
            Console.WriteLine("Entry Entered");
        }

    }
        
}

