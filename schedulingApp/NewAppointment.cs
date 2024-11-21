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
    public partial class NewAppointment : Form
    {
        private readonly DatabaseHelper dbHelper;
        private int? selectedCustomerId = null;
        private readonly Label timezoneInfoLabel;
        private readonly Label businessHoursLabel;

        public NewAppointment()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();

            // Add timezone info label
            timezoneInfoLabel = new Label
            {
                AutoSize = true,
                Location = new Point(92, EndDatePicker.Bottom + 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.White,
                Text = AppointmentHelper.GetTimezoneDifferenceMessage()
            };
            panel1.Controls.Add(timezoneInfoLabel);

            // Add business hours label
            businessHoursLabel = new Label
            {
                AutoSize = true,
                Location = new Point(92, timezoneInfoLabel.Bottom + 10),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.White,
                Text = AppointmentHelper.BusinessHours.GetBusinessHoursMessage(DateTime.Now)
            };
            panel1.Controls.Add(businessHoursLabel);

            InitializeForm();
            LoadCustomers();
            LoadAppointments();
        }

        private void InitializeForm()
        {
            // Set up date/time pickers
            StartDatePicker.Format = DateTimePickerFormat.Custom;
            StartDatePicker.CustomFormat = "MM/dd/yyyy hh:mm tt";
            EndDatePicker.Format = DateTimePickerFormat.Custom;
            EndDatePicker.CustomFormat = "MM/dd/yyyy hh:mm tt";

            // Set up DataGridView
            AppointmentsDataGridView.AutoGenerateColumns = false;
            AppointmentsDataGridView.Columns.Add("customerName", "Customer");
            AppointmentsDataGridView.Columns.Add("title", "Title");
            AppointmentsDataGridView.Columns.Add("start", "Start Time");
            AppointmentsDataGridView.Columns.Add("end", "End Time");
            AppointmentsDataGridView.Columns.Add("type", "Type");

            // Set up ComboBox
            CustomerComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            CustomerComboBox.SelectedIndexChanged += CustomerComboBox_SelectedIndexChanged;

            // Set up event handlers
            StartDatePicker.ValueChanged += DatePicker_ValueChanged;
            bttnCreate.Click += BttnCreate_Click;
            bttnExit.Click += BttnExit_Click;
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            businessHoursLabel.Text = AppointmentHelper.BusinessHours.GetBusinessHoursMessage(StartDatePicker.Value);
        }

        private void LoadCustomers()
        {
            DataTable customers = dbHelper.GetAllCustomers();
            CustomerComboBox.DataSource = customers;
            CustomerComboBox.DisplayMember = "customerName";
            CustomerComboBox.ValueMember = "customerId";
            CustomerComboBox.SelectedIndex = -1;
        }

        private void LoadAppointments(int? customerId = null)
        {
            DataTable appointments = customerId.HasValue ?
                dbHelper.GetCustomerAppointments(customerId.Value) :
                dbHelper.GetAllAppointments();

            AppointmentsDataGridView.Rows.Clear();
            if (appointments != null && appointments.Rows.Count > 0)
            {
                foreach (DataRow row in appointments.Rows)
                {
                    DateTime start = TimeZoneInfo.ConvertTimeFromUtc(
                        ((DateTime)row["start"]).ToUniversalTime(),
                        TimeZoneInfo.Local);
                    DateTime end = TimeZoneInfo.ConvertTimeFromUtc(
                        ((DateTime)row["end"]).ToUniversalTime(),
                        TimeZoneInfo.Local);

                    AppointmentsDataGridView.Rows.Add(
                        row["customerName"],
                        row["title"],
                        AppointmentHelper.FormatTimeWithZones(start),
                        AppointmentHelper.FormatTimeWithZones(end),
                        row["type"]
                    );
                }
            }
        }

        private void CustomerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CustomerComboBox.SelectedValue != null)
            {
                DataRowView drv = (DataRowView)CustomerComboBox.SelectedItem;
                selectedCustomerId = Convert.ToInt32(drv["customerId"]); 
                LoadAppointments(selectedCustomerId);

                //selectedCustomerId = Convert.ToInt32(CustomerComboBox.SelectedValue);
                //LoadAppointments(selectedCustomerId);
            }
            else
            {
                selectedCustomerId = null;
                LoadAppointments();
            }
        }

        private void BttnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!selectedCustomerId.HasValue)
                {
                    MessageBox.Show("Please select a customer.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(TitleInput.Text))
                {
                    MessageBox.Show("Title is required.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime startTime = StartDatePicker.Value;
                DateTime endTime = EndDatePicker.Value;

                if (endTime <= startTime)
                {
                    MessageBox.Show("End time must be after start time.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate business hours
                if (!AppointmentHelper.IsWithinBusinessHours(startTime, endTime))
                {
                    var (localStart, localEnd) = AppointmentHelper.BusinessHours.GetBusinessHoursInLocalTime(startTime.Date);
                    MessageBox.Show(
                        $"Appointments must be scheduled within business hours:\n\n" +
                        $"Your local time: {localStart:HH:mm} - {localEnd:HH:mm}\n" +
                        $"Eastern Time: 09:00 - 17:00\n\n" +
                        $"Please adjust your appointment time.",
                        "Validation Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }


                DataTable existingAppointments = dbHelper.GetAllAppointments();
                if (AppointmentHelper.HasOverlappingAppointments(startTime, endTime,existingAppointments)) 
                {
                    MessageBox.Show(
                       "This appointment overlaps with an existing appointment.\n" +
                       "Please choose a different time.",
                       "Validation Error",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Warning);
                    return;

                }

                // Add appointment
                var (success, message) = dbHelper.AddAppointment(
                    selectedCustomerId.Value,
                    1, // Current user ID
                    TitleInput.Text.Trim(),
                    DescriptionInput.Text.Trim(),
                    LocationInput.Text.Trim(),
                    ContactInput.Text.Trim(),
                    TypeInput.Text.Trim(),
                    URLInput.Text.Trim(),
                    startTime,
                    endTime
                );

                if (success)
                {
                    MessageBox.Show("Appointment created successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAppointments(selectedCustomerId);
                    ClearForm();

                    //Should keep? 
                    // Return to main form
                    //MainForm mainForm = new MainForm();
                    //mainForm.Show();
                    //this.Close();
                }
                else
                {
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            TitleInput.Text = "";
            DescriptionInput.Text = "";
            LocationInput.Text = "";
            ContactInput.Text = "";
            TypeInput.Text = "";
            URLInput.Text = "";
            StartDatePicker.Value = DateTime.Now;
            EndDatePicker.Value = DateTime.Now.AddHours(1);
        }

        private void BttnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}