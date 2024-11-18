using System;
using System.Windows.Forms;

namespace schedulingApp
{
    public partial class NewUserForm : Form
    {
        private readonly DatabaseHelper dbHelper;

        public NewUserForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void bttnRegister_Click(object sender, EventArgs e)
        {
            string username = userNameInput.Text.Trim();
            string password = PasswordInput.Text;
            string confirmPassword = ConfirmPassword.Text;

            //validation Part
            
            //1. check for empty fields/ exceeding 50 chars
            var (isValid, message) = ValidationHelper.ValidateLoginInput(username, password);
            if (!isValid)
            {
                MessageBox.Show(message, "Registration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            //MessageBox.Show($"This is what password is: {password} and this is what confirmpass is: {confirmPassword}");
           //2.check if pass and confirmation match
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match.", "Registration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var (success, dbMessage) = dbHelper.RegisterUser(username, password);
            if (success)
            {
                MessageBox.Show(dbMessage, "Registration Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Open login form and close registration form
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show(dbMessage, "Registration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void bttnExit_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }
    }
}