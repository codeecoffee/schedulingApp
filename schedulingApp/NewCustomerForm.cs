using System;
using System.Windows.Forms;

namespace schedulingApp
{
    public partial class NewCustomerForm : Form
    {
        private readonly DatabaseHelper dbHelper;

        public NewCustomerForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
        }

        private void bttnSave_Click(object sender, EventArgs e)
        {
            try
            {
                // Get values from form controls and trim them
                string customerName = CustomerNameInput.Text.Trim();
                string address = CustomerAddressInput.Text.Trim();
                string phoneNumber = CustomerPhoneInput.Text.Trim();

                // Validate all customer input
                var (isValid, validationMessage) = ValidationHelper.ValidateCustomerInput(
                    customerName, address, phoneNumber);

                if (!isValid)
                {
                    MessageBox.Show(validationMessage, "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Try to add the customer
                var (success, dbMessage) = dbHelper.AddCustomer(customerName, address, phoneNumber);

                if (success)
                {
                    MessageBox.Show(dbMessage, "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Return to main form
                    MainForm mainForm = new MainForm();
                    mainForm.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show(dbMessage, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bttnExit_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Close();
        }

        
        private void customerNameTextBox_Leave(object sender, EventArgs e)
        {
            var (isValid, message) = ValidationHelper.ValidateCustomerName(CustomerNameInput.Text.Trim());
            if (!isValid)
            {
                MessageBox.Show(message, "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CustomerNameInput.Focus();
            }
        }

        private void addressTextBox_Leave(object sender, EventArgs e)
        {
            var (isValid, message) = ValidationHelper.ValidateAddress(CustomerAddressInput.Text.Trim());
            if (!isValid)
            {
                MessageBox.Show(message, "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CustomerAddressInput.Focus();
            }
        }

        private void phoneNumberTextBox_Leave(object sender, EventArgs e)
        {
            var (isValid, message) = ValidationHelper.ValidatePhoneNumber(CustomerPhoneInput.Text.Trim());
            if (!isValid)
            {
                MessageBox.Show(message, "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CustomerPhoneInput.Focus();
            }
        }
    }
}