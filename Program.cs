using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using System.Data;

namespace ReadSQLite3
{
    class Program
    {
        private static string connectionString = @"Data Source=.\Customer.db3;Cache=Shared";
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
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        GetTables(conn);
                        
                        Console.WriteLine("\nContacts using DataAdapter:");
                        UseAdapter(conn);

                        Console.WriteLine("\nContacts using DataReader:");                        
                        UseReader(conn);
                    }
                    else
                    {
                        Console.WriteLine($"Unable to open database: {connectionString} ");
                    }

                }
                catch (SqliteException se)
                {
                    Console.WriteLine($"Exception: {se.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }
            Console.WriteLine("\nPress <Enter> to continue....");
            Console.ReadLine();
        }

        private static void GetTables(SqliteConnection conn)
        {
            using (SqliteCommand command = conn.CreateCommand())
            {
                command.CommandText = queryTables;

                using (SqliteDataReader reader = command.ExecuteReader())
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

        private static void UseAdapter(SqliteConnection conn)
        {
            //    //SqliteDataAdapter sqlAdapter = null;
            //    DataSet ds = null;
            //    object []items = null;
            //    int records = 0;

            //using (SqliteCommand command = conn.CreateCommand())
            //{
            //    command.CommandText = query;
            //    sqlAdapter = new SqliteDataAdapter();
            //    sqlAdapter.SelectCommand = command;
            //    ds = new System.Data.DataSet();

            //    records = sqlAdapter.Fill(ds);

            //    foreach (DataRow row in ds.Tables[0].Rows)
            //    {
            //        items = row.ItemArray;
            //        Console.WriteLine($"\tId: {items[0]}, First Name: {items[1]}, Last Name: {items[2]}");
            //    }
            //}
            Console.WriteLine($"\tNot implmented yet (https://github.com/dotnet/efcore/issues/13838)");
        }

        private static void UseReader(SqliteConnection conn)
        {

            using (SqliteCommand command = conn.CreateCommand())
            {
                command.CommandText = query;

                using (SqliteDataReader reader = command.ExecuteReader())
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
