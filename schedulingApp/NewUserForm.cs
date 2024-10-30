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
            string username = usernameTextBox.Text.Trim();
            string password = passwordTextBox.Text;
            string confirmPassword = confirmPasswordTextBox.Text;
            string email = emailTextBox.Text.Trim();
            string firstName = firstNameTextBox.Text.Trim();
            string lastName = lastNameTextBox.Text.Trim();

            // Validate all registration input
            var (isValid, message) = ValidationHelper.ValidateRegistrationInput(
                username, password, confirmPassword, email, firstName, lastName);

            if (!isValid)
            {
                MessageBox.Show(message, "Registration Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Attempt to register the user
            var (success, dbMessage) = dbHelper.RegisterUser(username, password, email, firstName, lastName);

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
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
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