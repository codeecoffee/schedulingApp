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

            // Set up the DataGridView
            customersDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            customersDataGridView.MultiSelect = false;
            customersDataGridView.ReadOnly = true;
        }
        private void LoadCustomers()
        {
            try
            {
                DataTable customers = dbHelper.GetAllCustomers();
                customersDataGridView.DataSource = customers;

                // Optionally hide certain columns
                customersDataGridView.Columns["customer_id"].Visible = false;
                customersDataGridView.Columns["created_date"].Visible = false;

                // Rename displayed columns
                customersDataGridView.Columns["customer_name"].HeaderText = "Name";
                customersDataGridView.Columns["address"].HeaderText = "Address";
                customersDataGridView.Columns["phone_number"].HeaderText = "Phone";

                // Auto-size columns
                customersDataGridView.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customers: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void customersDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (customersDataGridView.SelectedRows.Count > 0)
            {
                DataGridViewRow row = customersDataGridView.SelectedRows[0];
                currentCustomerId = Convert.ToInt32(row.Cells["customer_id"].Value);

                // Populate the text boxes
                CustomerNameInput.Text = row.Cells["customer_name"].Value.ToString();
                CustomerAddressInput.Text = row.Cells["address"].Value.ToString();
                CustomerPhoneInput.Text = row.Cells["phone_number"].Value.ToString();

                // Enable modification controls
                bttnModify.Enabled = true;
                bttnDeleteCustomer.Enabled = true;
            }
            else
            {
                // Clear and disable controls when no selection
                //ClearFields();
                bttnModify.Enabled = false;
                bttnDeleteCustomer.Enabled = false;

            }
        }
        private void bttnModify_Click(object sender, EventArgs e)
        {
            if (currentCustomerId == -1)
            {
                MessageBox.Show("Please select a customer to modify.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate input
            var (isValid, validationMessage) = ValidationHelper.ValidateCustomerInput(
                CustomerNameInput.Text.Trim(),
                CustomerAddressInput.Text.Trim(),
                CustomerPhoneInput.Text.Trim());

            if (!isValid)
            {
                MessageBox.Show(validationMessage, "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Update customer
            var (success, message) = dbHelper.UpdateCustomer(
                currentCustomerId,
                CustomerNameInput.Text.Trim(),
                CustomerAddressInput.Text.Trim(),
                CustomerPhoneInput.Text.Trim());

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

        private void ClearFields()
        {
            currentCustomerId = -1;
            CustomerNameInput.Text = string.Empty;
            CustomerAddressInput.Text = string.Empty;
            CustomerPhoneInput.Text = string.Empty;
        }

        private void bttnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
