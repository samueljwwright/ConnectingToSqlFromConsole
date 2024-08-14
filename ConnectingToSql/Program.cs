using ConnectingToSql;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Channels;

public class program
{
    public static void Main(string[] args)
    {
        DataSource Conn = new DataSource();
        SqlConnection connection = Conn.CreateConnection();
        //Conn.View(connection, "Movies", 3);
        //Console.WriteLine();
        //Conn.View(connection, "Movies");
        //Console.WriteLine();
        //string[] ats = {"Title", "Release_Year" };
        //Conn.View(connection, "Movies", ats );

        //Conn.GetColumns(connection, "Movies");

        //Conn.CloseConnection(connection);

        loop(Conn, connection);
    }


    private static void loop(DataSource Conn, SqlConnection connection)
    {
        bool exit = false;
        do
        {
            Console.WriteLine("Welcome to the movies and actors app!");
            Console.WriteLine("Please select an option by inputting a number");
            Console.WriteLine("1. View data");
            Console.WriteLine("2. Create new entry");
            Console.WriteLine("3. Update entry");
            Console.WriteLine("4. Remove entry");
            Console.WriteLine("0. Exit");
            string? input = Console.ReadLine();
            switch (input)
            {
                case ("1"):
                    View(Conn, connection);
                    break;
                case ("2"):
                    Create(Conn, connection);
                    break;
                case ("3"):
                    break;
                case ("4"):
                    break;
                case ("0"):
                    exit = true;
                    break;
                default:
                    exit = false;
                    break;

            }

        } while (!exit);
    }

    private static void View(DataSource Conn, SqlConnection connection)
    {

        //Table name (Required field)

        //string CompositeQuery = ""; query handled in datasource class
        Console.Clear(); 
        Console.WriteLine("Please select table by number:");
        List<string> tables = Conn.getTables(connection);
        
        for (int i = 0; i < tables.Count; i++)
        {
            Console.WriteLine($"{i+1}. {tables[i]}");
        }

        string Table_Name = "";
        bool exit = false;
        do
        {
            string input = Console.ReadLine();

            
            if (Int32.TryParse(input, out int InputAsInt) && InputAsInt < tables.Count + 1 && InputAsInt > 0)
            {
                Table_Name = tables[InputAsInt - 1];
                exit = true;
            }
            
        } while (!exit);


        //ROW COUNT (not required)

        Console.Clear();

        Console.WriteLine("Please input number of desired rows (0 for all rows available): ");

        int Row_Count = -1;
        exit = false;
        do
        {
            string input = Console.ReadLine();

            if (Int32.TryParse(input, out int InputAsInt) && InputAsInt > -1) //maybe make func getting reused agin
            {
                if (InputAsInt != 0)
                {
                    Row_Count = InputAsInt;
                    exit = true;
                }
                else
                {
                    exit = true;
                }            
            }
        } while (!exit);

        //ATTRIBUTES (not reqiured)

        Console.Clear();

        Console.WriteLine("Please enter the attributes you would like to view:");

        List<string> tableAttributes = Conn.GetColumns(connection, Table_Name);

        for (int i = 0; i < tableAttributes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {tableAttributes[i]}");
        }
        Console.WriteLine("Enter 0 to stop adding fields to search or to skip");


        List<string> Attributes = new List<string>();
        exit = false;
        do
        {
            string input = Console.ReadLine();
            if (Int32.TryParse(input, out int InputAsInt) && InputAsInt < tables.Count + 1 && InputAsInt > -1)
            {
                if (InputAsInt == 0)
                    exit = true;
                else
                    Attributes.Add(tableAttributes[InputAsInt - 1]);
            }
        } while (!exit);

        Console.Clear();

        //should be a better way of doing this > 

        if (Row_Count <= 0)
        {
            if (Attributes.Count == 0)
            {
                Conn.View(connection, Table_Name);
            }
            else
            {
                Conn.View(connection, Table_Name, Attributes.ToArray());
            }
        }
        else if (Row_Count > 0)
        {
            if (Attributes.Count == 0)
            {
                Conn.View(connection, Table_Name, (uint)Row_Count);
            }
            else
            {
                Conn.View(connection, Table_Name, (uint)Row_Count, Attributes.ToArray());
            }
        }

        Console.WriteLine();
        Console.WriteLine("Make a new query:");
    }

    //struct Movie {
    //    int id;
    //    string title;
    //    int year;
    //    int rating;
    //    int genre;

    //}


    private static void Create(DataSource Conn, SqlConnection connection)
    {
        Console.WriteLine("Please select table by number:");
        List<string> tables = Conn.getTables(connection);

        for (int i = 0; i < tables.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {tables[i]}");
        }

        string Table_Name = "";
        bool exit = false;
        do
        {
            string input = Console.ReadLine();


            if (Int32.TryParse(input, out int InputAsInt) && InputAsInt < tables.Count + 1 && InputAsInt > 0)
            {
                Table_Name = tables[InputAsInt - 1];
                exit = true;
            }

        } while (!exit);

        Console.Clear();

        Console.WriteLine($"The following fields are required to INSERT new data into {Table_Name}");

        List<string> tableAttributes = Conn.GetColumns(connection, Table_Name);
        List<string> AttributeDataTypes = Conn.GetColumnData(connection, Table_Name);

        for (int i = 0; i < tableAttributes.Count; i++)
        {
            Console.Write($" |  {tableAttributes[i]} as [{AttributeDataTypes[i]}]");
        }

        Dictionary<string, string> inputToAtribute= new Dictionary<string, string>();

        for (int i =0; i < tableAttributes.Count; i++)
        {

            exit = false;
            do
            {
                string input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    if (AttributeDataTypes[i] == "int")
                    {
                        if (Int32.TryParse(input, out int InputAsInt))
                        {
                            inputToAtribute.Add(tableAttributes[i], input);

                            exit = true;
                            break;
                        }
                    }
                    else if (AttributeDataTypes[i] == "varchar")
                    {
                        
                        inputToAtribute.Add(tableAttributes[i], $"'{input}'");

                        exit = true;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Unsuported data type required.");
                    }
                }
                
                


            } while (exit);

            //ensure data is correct and matches the att
            //Console.WriteLine(tableAttributes[i] + " " + inputToAtribute[tableAttributes[i]]);

        }


        Conn.Insert(connection, Table_Name, inputToAtribute);


    }

}