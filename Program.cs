using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace ReadSQLite3
{
    class Program
    {
        private static string connectionString = @"Data Source=.\Customer.db3; FailIfMissing=True";
        private static string queryTables = "SELECT name " +
            "FROM sqlite_master " +
            "WHERE type = 'table' " + 
            "ORDER BY name";
        private static string query = "SELECT c.Id, FirstName, LastName, Address1, Address2, Notes, HomePhone, WorkPhone, CellPhone, EMail, cty.Name AS City, s.Name AS State, c.Active, c.ModifiedDt, c.CreateDt " + 
          "FROM Contact AS c " +  
          "JOIN City AS cty ON c.CityId = cty.Id " + 
          "JOIN State as s ON cty.StateId = s.Id";

        static void Main(string[] args)
        {
            Console.WriteLine("List of tables:");
            GetTables();
            Console.WriteLine("\nContacts using DataAdapter:");
            UseAdapter();
            Console.WriteLine("\nContacts using DataReader:");
            UseReader();

            Console.WriteLine("\nPress <Enter> to continue....");
            Console.ReadLine();
        }

        private static void GetTables()
        {
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandText = queryTables;

                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        Console.WriteLine($"\tName: {reader.GetString(0)}");
                                    }
                                }
                                else
                                    Console.WriteLine("No tables in database!");
                            }
                        }
                    }
                }
            }
            catch (SQLiteException se)
            {
                Console.WriteLine($"Exception: {se.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

        }

        private static void UseAdapter()
        {
            SQLiteDataAdapter sqlAdapter = null;
            DataSet ds = null;
            object []items = null;
            int records = 0;

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandText = query;
                            sqlAdapter = new SQLiteDataAdapter();
                            sqlAdapter.SelectCommand = command;
                            ds = new System.Data.DataSet();

                            records = sqlAdapter.Fill(ds);

                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                items = row.ItemArray;
                                Console.WriteLine($"\tId: {items[0]}, First Name: {items[1]}, Last Name: {items[2]}");
                            }
                        }
                    }
                }
            }
            catch (SQLiteException se)
            {
                Console.WriteLine($"Exception: {se.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        private static void UseReader()
        {

            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        using (SQLiteCommand command = conn.CreateCommand())
                        {
                            command.CommandText = query;

                            using (SQLiteDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Console.WriteLine($"\tId: {reader.GetInt32(0)}, First Name: {reader[1]}, Last Name: {reader[2]},");
                                }

                            }
                        }
                    }
                }
            }
            catch (SQLiteException se)
            {
                Console.WriteLine($"Exception: {se.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

    }
}
