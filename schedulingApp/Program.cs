namespace schedulingApp
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                var dbSetup = new DatabaseSetup();
                dbSetup.InitializeDatabase();


                ApplicationConfiguration.Initialize();
                Application.Run(new LoginForm());

            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Application initialization error: {ex.Message}",
                                 "Error",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
            }
        }
    }
}