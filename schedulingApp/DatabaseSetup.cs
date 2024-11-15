using MySqlConnector;
using System;
using System.IO;

namespace schedulingApp
{
    public class DatabaseSetup
    {

        public bool InitializeDatabase(string connectionString)
        {
            try
            {
                if (!ExecuteScriptFile("database-setup.sql",connectionString)) { return false; }
                if (!ExecuteScriptFile("LoadData.sql",connectionString)) { return false; }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization error: {ex.Message}");
                return false;
            }
        }

        public bool ExecuteScriptFile(string filename, string connString)
        {
            try
            {
                using(var connection = new MySqlConnection(connString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand())
                    {
                        command.Connection = connection;
                        string sqlScript = File.ReadAllText(filename);
                        string[] commands = sqlScript.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

                        foreach (string commandText in commands)
                        {
                            if(!string.IsNullOrWhiteSpace(commandText))
                            {
                                command.CommandText = commandText;
                                try
                                {
                                    command.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Command error in {filename}: {ex.Message}");
                                    Console.WriteLine($"Failed command: {commandText}");
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error exec {filename}:{ex.Message}");
                return false;
            }
        }

    }
}