using MySql.Data.MySqlClient;
using System;
//using System.IO;

namespace schedulingApp
{
    public class DatabaseSetup
    {
        private readonly string connectionString;
        private readonly string rootCoonectionString;

        public DatabaseSetup()
        {
            rootCoonectionString = "server=127.0.0.1;" + "port=3306;" + "Uid=sqlUser;" + "Pwd=Passw0rd!";
            connectionString = rootCoonectionString +"Database=client_schedule";
           // connectionString = "server=localhost;user=sqlUser;database=client_schedule;port=3306;password=Passw0rd!"
        }

        public bool InitializeDatabase()
        {
            try
            {
                if (!ExecuteScriptFile("database-setup.sql",rootCoonectionString)) { return false; }
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