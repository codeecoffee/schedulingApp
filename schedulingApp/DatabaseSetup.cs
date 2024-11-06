using MySql.Data.MySqlClient;
using System;
using System.IO;

namespace schedulingApp
{
    public class DatabaseSetup
    {
        private readonly string connectionString;

        public DatabaseSetup()
        {
            // Hardcoded credentials for AWS RDS
            connectionString = "Server=schedulingapp.cnw800uucbut.us-east-1.rds.amazonaws.com;" +
                             "Database=schedulingApp;" +
                             "Uid=root;" +
                             "Pwd=TestDB2024!;" +
                             "Convert Zero Datetime=True;";
        }

        public bool InitializeDatabase()
        {
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    using (var command = new MySqlCommand())
                    {
                        command.Connection = connection;

                        // Read and execute the setup script
                        string sqlScript = File.ReadAllText("database-setup.sql");
                        string[] commands = sqlScript.Split(new[] { ";" },
                            StringSplitOptions.RemoveEmptyEntries);

                        foreach (string commandText in commands)
                        {
                            if (!string.IsNullOrWhiteSpace(commandText))
                            {
                                command.CommandText = commandText;
                                try
                                {
                                    command.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Command error: {ex.Message}");
                                    // Continue with next command even if one fails
                                }
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization error: {ex.Message}");
                return false;
            }
        }
    }
}