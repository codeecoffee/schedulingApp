using System;
using System.IO;

namespace schedulingApp
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            StartApplication();
            ApplicationConfiguration.Initialize();
            Application.Run(new LoginForm());
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
                
            }
            else
            {
                Console.WriteLine("Database connection failed!");
            }
        }


    }
}