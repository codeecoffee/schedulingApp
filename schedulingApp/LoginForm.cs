using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace schedulingApp
{
    public partial class LoginForm : Form
    {
        private readonly Helper helper;
        private readonly DatabaseHelper dbHelper;

        public LoginForm()
        {
            InitializeComponent();
            helper = new Helper();
            dbHelper = new DatabaseHelper();



               

            Label locationLabel = new Label
            {
                AutoSize = true,
                Location = new Point(12, this.Height - 100),
                Text = helper.GetUserLocation()
            };
            this.Controls.Add(locationLabel);
            try
            {
               
                if (dbHelper.TestConnection())
                {
                    Console.WriteLine("Database connection successful!");
                }
                else
                {
                    MessageBox.Show("Could not connect to database. Please check your connection settings.",
                        "Connection Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}",
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void CheckUpcomingAppointments(string username)
        {
            DataTable upcomingAppointments = dbHelper.GetUpcomingAppointments(username, 15);

            if (upcomingAppointments != null && upcomingAppointments.Rows.Count > 0)
            {
                foreach (DataRow appointment in upcomingAppointments.Rows)
                {
                    DateTime appointmentTime = TimeZoneInfo.ConvertTimeFromUtc(
                        ((DateTime)appointment["start"]).ToUniversalTime(),
                        TimeZoneInfo.Local);

                    int minutesUntil = (int)(appointmentTime - DateTime.Now).TotalMinutes;

                    string message = string.Format(
                        helper.TranslateMessage("UpcomingAppointment"),
                        minutesUntil) + $"\n\n" +
                        $"Customer: {appointment["customerName"]}\n" +
                        $"Title: {appointment["title"]}\n" +
                        $"Time: {appointmentTime:g}";

                    MessageBox.Show(
                        message,
                        helper.TranslateMessage("UpcomingAppointment"),
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
        }

        private void bttnLogin_Click(object sender, EventArgs e)
        {
            string username = usernameInput.Text;
            string password = passwordInput.Text;

            // Validate input
            var (isValid, message) = ValidationHelper.ValidateLoginInput(username, password);
            if (!isValid)
            {
                MessageBox.Show(
                    helper.TranslateMessage(message),
                    helper.TranslateMessage("ValidationError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (dbHelper.ValidateUser(username, password))
            {
                dbHelper.LogLoginAttempt(username, true);

                // Check for upcoming appointments
                CheckUpcomingAppointments(username);

                // Show success message
                MessageBox.Show(
                    helper.TranslateMessage("LoginSuccessful"),
                    "",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                
                //Log the login attempt to the file 
                LogLogin(username);

                // Open main form
                MainForm mainForm = new MainForm();
                mainForm.Show();
                this.Hide();
            }
            else
            {
                dbHelper.LogLoginAttempt(username, false);
                MessageBox.Show(
                    helper.TranslateMessage("LoginFailed"),
                    helper.TranslateMessage("ValidationError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
        private void bttnRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            NewUserForm newUserForm = new NewUserForm();
            newUserForm.Show();
        }

        private void bttnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
        "Are you sure you want to exit the application?",
        "Confirm Exit",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                
                Application.Exit();
            }
        }
        private void LogLogin(string username)
        {
            string filePath = Path.Combine(Application.StartupPath, "Login_History.txt");
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //MessageBox.Show("[Func Loglogin] You hit the function to log shit");

            // Append the username and timestamp to the text file
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                // Write to file
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"{timestamp}: {username} logged in.");
                    writer.Flush(); // Ensure content is written immediately
                    //MessageBox.Show("[Func Loglogin] Function executed");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to log the login: {ex.Message}\nFile path: {filePath}");
            }
        }

        private void labelUsername_Click(object sender, EventArgs e)
        {

        }
    }
}
