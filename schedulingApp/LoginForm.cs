using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace schedulingApp
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void bttnLogin_Click(object sender, EventArgs e)
        {
            string username = usernameInput.Text;
            string password = passwordInput.Text;

            // Validate input
            var (isValid, message) = ValidationHelper.ValidateLoginInput(username, password);
            if (!isValid)
            {
                MessageBox.Show(message, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DatabaseHelper dbHelper = new DatabaseHelper();

            if (dbHelper.ValidateUser(username, password))
            {
                // Log successful login
                dbHelper.LogLoginAttempt(username, true);

                // Get user details if needed
                DataRow userDetails = dbHelper.GetUserDetails(username);

                // Open the MainForm          
                MainForm mainForm = new MainForm();
                mainForm.Show();

                this.Hide();
            }
            else
            {
                // Log failed login attempt
                dbHelper.LogLoginAttempt(username, false);
                MessageBox.Show("Invalid username or password.", "Login Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            this.Close();
        }
        private void LogLogin(string username)
        {
            string filePath = "Login_History.txt";
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Append the username and timestamp to the text file
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine($"{timestamp}: {username} logged in.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to log the login: {ex.Message}");
            }
        }

        private void labelUsername_Click(object sender, EventArgs e)
        {

        }
    }
}
