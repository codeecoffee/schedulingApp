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
                // Get values and trim them
                string customerName = CustomerNameInput.Text.Trim();
                string address = CustomerAddressInput.Text.Trim();
                string address2 = CustomerAddress2Input.Text.Trim();  
                string city = CustomerCityInput.Text.Trim();         
                string country = CustomerCountryInput.Text.Trim();    
                string postalCode = CustomerPostalCodeInput.Text.Trim(); 
                string phoneNumber = CustomerPhoneInput.Text.Trim();

                // Validate all customer input
                var (isValid, validationMessage) = ValidationHelper.ValidateCustomerInput(
                    customerName,
                    address,
                    address2,
                    postalCode,
                    phoneNumber,
                    city,
                    country);

                if (!isValid)
                {
                    MessageBox.Show(validationMessage, "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Try to add the customer
                var (success, dbMessage) = dbHelper.AddCustomer(
                    customerName,
                    address,
                    address2,
                    city,
                    country,
                    postalCode,
                    phoneNumber);

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


        private void CustomerNameInput_Leave(object sender, EventArgs e)
        {
            var (isValid, message) = ValidationHelper.ValidateCustomerName(CustomerNameInput.Text.Trim());
            if (!isValid)
            {
                MessageBox.Show(message, "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CustomerNameInput.Focus();
            }
        }
        private void CustomerAddressInput_Leave(object sender, EventArgs e)
        {
            var (isValid, message) = ValidationHelper.ValidateAddress(CustomerAddressInput.Text.Trim());
            if (!isValid)
            {
                MessageBox.Show(message, "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CustomerAddressInput.Focus();
            }
        }
        private void CustomerAddress2Input_Leave(object sender, EventArgs e)
        {
            var (isValid, message) = ValidationHelper.ValidateAddress2(CustomerAddress2Input.Text.Trim());
            if (!isValid)
            {
                MessageBox.Show(message, "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CustomerAddress2Input.Focus();
            }
        }
        private void CustomerCityInput_Leave(object sender, EventArgs e)
        {
            var (isValid, message) = ValidationHelper.ValidateCity(CustomerCityInput.Text.Trim());
            if (!isValid)
            {
                MessageBox.Show(message, "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CustomerCityInput.Focus();
            }
        }
        private void CustomerCountryInput_Leave(object sender, EventArgs e)
        {
            var (isValid, message) = ValidationHelper.ValidateCountry(CustomerCountryInput.Text.Trim());
            if (!isValid)
            {
                MessageBox.Show(message, "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CustomerCountryInput.Focus();
            }
        }
        private void CustomerPostalCodeInput_Leave(object sender, EventArgs e)
        {
            var (isValid, message) = ValidationHelper.ValidatePostalCode(CustomerPostalCodeInput.Text.Trim());
            if (!isValid)
            {
                MessageBox.Show(message, "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CustomerPostalCodeInput.Focus();
            }
        }
        private void CustomerPhoneInput_Leave(object sender, EventArgs e)
        {
            var (isValid, message) = ValidationHelper.ValidatePhone(CustomerPhoneInput.Text.Trim());
            if (!isValid)
            {
                MessageBox.Show(message, "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                CustomerPhoneInput.Focus();
            }
        }
    }
}