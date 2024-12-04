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
    public partial class EditCustomerForm : Form
    {
        private readonly DatabaseHelper dbHelper;
        private int currentCustomerId = -1;

        public EditCustomerForm()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            LoadCustomers();

           
            customersDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            customersDataGridView.MultiSelect = false;
            customersDataGridView.ReadOnly = true;

            customersDataGridView.CellClick += new DataGridViewCellEventHandler(customersDataGridView_CellClick);
            customersDataGridView.CellDoubleClick += new DataGridViewCellEventHandler(customersDataGridView_CellDoubleClick);

        }

        private void LoadCustomers()
        {
            try
            {
                DataTable customers = dbHelper.GetAllCustomers();
                customersDataGridView.DataSource = customers;

                // Hide system columns
                customersDataGridView.Columns["customerId"].Visible = false;
                customersDataGridView.Columns["active"].Visible = false;

                // Rename displayed columns
                customersDataGridView.Columns["customerName"].HeaderText = "Name";
                customersDataGridView.Columns["address"].HeaderText = "Address";
                customersDataGridView.Columns["address2"].HeaderText = "Address 2";
                customersDataGridView.Columns["city"].HeaderText = "City";
                customersDataGridView.Columns["postalCode"].HeaderText = "Postal Code";
                customersDataGridView.Columns["phone"].HeaderText = "Phone";
                customersDataGridView.Columns["country"].HeaderText = "Country";

                // Auto-size columns
                customersDataGridView.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //OK
        private void customersDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = customersDataGridView.Rows[e.RowIndex];
                currentCustomerId = Convert.ToInt32(row.Cells["customerId"].Value);
            }
        }
        //OK
        private void customersDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = customersDataGridView.Rows[e.RowIndex];
                currentCustomerId = Convert.ToInt32(row.Cells["customerId"].Value);

                CustomerNameInput.Text = row.Cells["customerName"].Value.ToString();
                CustomerAddressInput.Text = row.Cells["address"].Value.ToString();
                CustomerAddress2Input.Text = row.Cells["address2"].Value.ToString();
                CustomerCityInput.Text = row.Cells["city"].Value.ToString();
                CustomerCountryInput.Text = row.Cells["country"].Value.ToString();
                CustomerPostalCodeInput.Text = row.Cells["postalCode"].Value.ToString();
                CustomerPhoneInput.Text = row.Cells["phone"].Value.ToString();
            }
        }

        private void ClearFields()
        {
            currentCustomerId = -1;
            CustomerNameInput.Text = string.Empty;
            CustomerAddressInput.Text = string.Empty;
            CustomerAddress2Input.Text = string.Empty;
            CustomerCityInput.Text = string.Empty;
            CustomerCountryInput.Text = string.Empty;
            CustomerPostalCodeInput.Text = string.Empty;
            CustomerPhoneInput.Text = string.Empty;
        }

        private void bttnModify_Click(object sender, EventArgs e)
        {
            if (currentCustomerId == -1)
            { MessageBox.Show("Please select a customer to modify.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
           
            string customerName = CustomerNameInput.Text.Trim();
            string address = CustomerAddressInput.Text.Trim();
            string address2 = CustomerAddress2Input.Text.Trim();
            string city = CustomerCityInput.Text.Trim();
            string country = CustomerCountryInput.Text.Trim();
            string postalCode = CustomerPostalCodeInput.Text.Trim();
            string phoneNumber = CustomerPhoneInput.Text.Trim();

            
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

            // Update customer with all fields
            var (success, message) = dbHelper.UpdateCustomer(
                currentCustomerId,
                customerName,
                address,
                address2,
                city,
                country,
                postalCode,
                phoneNumber);

            MessageBox.Show(message, success ? "Success" : "Error",
                MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

            if (success)
            {
                LoadCustomers(); // Reload the grid to show changes
            }

        }

        private void bttnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (currentCustomerId == -1)
            {
                MessageBox.Show("Please select a customer to delete.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Confirm deletion
            var result = MessageBox.Show(
                "Are you sure you want to delete this customer? This action cannot be undone.",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                var (success, message) = dbHelper.DeleteCustomer(currentCustomerId);

                MessageBox.Show(message, success ? "Success" : "Error",
                    MessageBoxButtons.OK, success ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                if (success)
                {
                    LoadCustomers(); // Reload the grid
                    ClearFields(); // Clear the input fields
                }
            }
        }

        private void bttnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
