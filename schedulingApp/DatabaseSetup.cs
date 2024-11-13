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
            connectionString = "server=localhost;user=sqlUser;database=client_schedule;port=3306;password=Passw0rd!";
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

                        // Setup
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