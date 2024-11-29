using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace schedulingApp
{
    public static class SimpleFileLogger
    {
        private static readonly string LogFile = @"C:\Users\LabUser\source\repos\codeecoffee\schedulingApp\schedulingApp\Login_History.txt";

        public static void LogLogin(string username)
        {
            try
            {
                
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string logMessage = $"{timestamp} - User logged in: {username}\r\n";

                
                File.AppendAllText(LogFile, logMessage);

                
                if (File.Exists(LogFile))
                {
                    string content = File.ReadAllText(LogFile);
                    Console.WriteLine($"Log file content: {content}");
                    //MessageBox.Show($"Log written successfully.\nPath: {LogFile}\nContent: {content}");
                }
            }
            catch (UnauthorizedAccessException uaEx)
            {
                MessageBox.Show($"Access denied to log file. Try running Visual Studio as administrator.\nError: {uaEx.Message}");
            }
            catch (DirectoryNotFoundException dnfEx)
            {
                MessageBox.Show($"Directory not found. Please verify the path exists.\nError: {dnfEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Logging error: {ex.Message}\nFile: {LogFile}\nError type: {ex.GetType().Name}");
            }
        }

        // Method to verify if we can write to the file
        public static bool VerifyFileAccess()
        {
            try
            {
                // Try to create/append to the file
                //File.AppendAllText(LogFile, $"File access verified at {DateTime.Now}\r\n");
                //MessageBox.Show($"Successfully verified file access at: {LogFile}");
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"File access verification failed:\n{ex.Message}");
                return false;
            }
        }
    }
}
