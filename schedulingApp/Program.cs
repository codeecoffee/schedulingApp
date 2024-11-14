//namespace schedulingApp
//{
//    internal static class Program
//    {
//        /// <summary>
//        ///  The main entry point for the application.
//        /// </summary>
//        [STAThread]
//        static void Main()
//        {
//            try
//            {
//                var dbSetup = new DatabaseSetup();
//                dbSetup.InitializeDatabase();


//                ApplicationConfiguration.Initialize();
//                Application.Run(new LoginForm());

//            }
//            catch (Exception ex) 
//            {
//                MessageBox.Show($"Application initialization error: {ex.Message}",
//                                 "Error",
//                                 MessageBoxButtons.OK,
//                                 MessageBoxIcon.Error);
//            }
//        }
//    }
//}


using System;
using System.IO;

namespace schedulingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Check if this is first run by looking for a marker file
            string markerFile = "database_initialized.txt";

            if (!File.Exists(markerFile))
            {
                Console.WriteLine("First run detected. Initializing database...");

                var dbSetup = new DatabaseSetup();
                if (dbSetup.InitializeDatabase())
                {
                    Console.WriteLine("Database initialized successfully!");
                    // Create marker file to indicate database has been initialized
                    File.WriteAllText(markerFile, DateTime.Now.ToString());
                }
                else
                {
                    Console.WriteLine("Failed to initialize database!");
                    return;
                }
            }

            // Continue with regular application startup
            StartApplication();
        }

        private static void StartApplication()
        {
            // Your regular application startup code here
            Console.WriteLine("Application starting...");

            // Example: Test database connection
            var dbHelper = new DatabaseHelper();
            if (dbHelper.TestConnection())
            {
                Console.WriteLine("Database connection successful!");

                // Test if data was loaded
                var customers = dbHelper.GetAllCustomers();
                if (customers != null && customers.Rows.Count > 0)
                {
                    Console.WriteLine($"Found {customers.Rows.Count} customers in database.");
                }
            }
            else
            {
                Console.WriteLine("Database connection failed!");
            }
        }
    }
}