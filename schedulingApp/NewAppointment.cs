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
        private Label timezoneInfoLabel;
        private readonly Label businessHoursLabel;
        private ComboBox TimeZoneComboBox;

        public NewAppointment()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();

            InitializeTimezoneControls();

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
            };
            panel1.Controls.Add(businessHoursLabel);

            InitializeForm();
            LoadCustomers();
            LoadAppointments();
            UpdateTimezoneInfo();
        }

        private void InitializeTimezoneControls()
        {
            // Create timezone combo box
            TimeZoneComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(92, EndDatePicker.Bottom + 20),
                Width = 300,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Black,
                BackColor = Color.White
            };

            // Add timezone info label
            timezoneInfoLabel = new Label
            {
                AutoSize = true,
                Location = new Point(92, TimeZoneComboBox.Bottom + 10),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.White,
            };

            // Add controls to panel
            panel1.Controls.Add(TimeZoneComboBox);
            panel1.Controls.Add(timezoneInfoLabel);

            // Populate timezone combo box
            var timeZones = AppointmentHelper.GetAvailableTimeZones();
            TimeZoneComboBox.DataSource = timeZones;
            TimeZoneComboBox.DisplayMember = "DisplayName";
            TimeZoneComboBox.ValueMember = "Id";

            // Set default to local timezone
            TimeZoneComboBox.SelectedValue = TimeZoneInfo.Local.Id;

            // Add event handler
            TimeZoneComboBox.SelectedIndexChanged += TimeZoneComboBox_SelectedIndexChanged;
        }

        private void TimeZoneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TimeZoneComboBox.SelectedValue == null) return;

            try
            {
                UpdateTimezoneInfo();

                // Adjust appointment times for selected timezone
                string selectedTimeZoneId = TimeZoneComboBox.SelectedValue.ToString();
                var (adjustedStart, adjustedEnd) = AppointmentHelper.AdjustAppointmentTimesForTimeZone(
                    StartDatePicker.Value,
                    EndDatePicker.Value,
                    TimeZoneInfo.Local.Id,
                    selectedTimeZoneId);

                // Update date pickers (temporarily remove event handlers)
                StartDatePicker.ValueChanged -= DatePicker_ValueChanged;
                EndDatePicker.ValueChanged -= DatePicker_ValueChanged;

                if (adjustedStart >= StartDatePicker.MinDate)
                    StartDatePicker.Value = adjustedStart;
                if (adjustedEnd >= EndDatePicker.MinDate)
                    EndDatePicker.Value = adjustedEnd;

                // Reattach event handlers
                StartDatePicker.ValueChanged += DatePicker_ValueChanged;
                EndDatePicker.ValueChanged += DatePicker_ValueChanged;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adjusting timezone: {ex.Message}",
                    "Timezone Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateTimezoneInfo()
        {
            if (TimeZoneComboBox.SelectedValue == null) return;

            string selectedTimeZoneId = TimeZoneComboBox.SelectedValue.ToString();
            timezoneInfoLabel.Text = AppointmentHelper.GetTimeZoneInfoMessage(selectedTimeZoneId);

            var (start, end) = AppointmentHelper.GetAdjustedBusinessHours(DateTime.Now, selectedTimeZoneId);
            businessHoursLabel.Text = $"Business Hours in Your Timezone: {start:HH:mm} - {end:HH:mm}";
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            UpdateTimezoneInfo();
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

            AppointmentsDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            AppointmentsDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Set up ComboBox
            CustomerComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            CustomerComboBox.SelectedIndexChanged += CustomerComboBox_SelectedIndexChanged;

            // Set up event handlers
            StartDatePicker.ValueChanged += DatePicker_ValueChanged;
            bttnCreate.Click += BttnCreate_Click;
            bttnExit.Click += BttnExit_Click;
        }

        //old
        //private void DatePicker_ValueChanged(object sender, EventArgs e)
        //{
        //    businessHoursLabel.Text = AppointmentHelper.BusinessHours.GetBusinessHoursMessage(StartDatePicker.Value);
        //}

        private void LoadCustomers()
        {
            DataTable customers = dbHelper.GetAllCustomers();
            CustomerComboBox.DataSource = customers;
            CustomerComboBox.DisplayMember = "customerName";
            CustomerComboBox.ValueMember = "customerId";
            CustomerComboBox.SelectedIndex = -1;
        }
        //private void LoadAppointments(int? customerId = null)
        //{
        //    DataTable appointments = customerId.HasValue ?
        //        dbHelper.GetCustomerAppointments(customerId.Value) :
        //        dbHelper.GetAllAppointments();

        //    AppointmentsDataGridView.Rows.Clear();
        //    if (appointments != null && appointments.Rows.Count > 0)
        //    {
        //        string selectedTimeZoneId = TimeZoneComboBox.SelectedValue?.ToString() ?? TimeZoneInfo.Local.Id;

        //        foreach (DataRow row in appointments.Rows)
        //        {
        //            DateTime startUtc = ((DateTime)row["start"]).ToUniversalTime();
        //            DateTime endUtc = ((DateTime)row["end"]).ToUniversalTime();

        //            var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(selectedTimeZoneId);
        //            DateTime start = TimeZoneInfo.ConvertTimeFromUtc(startUtc, userTimeZone);
        //            DateTime end = TimeZoneInfo.ConvertTimeFromUtc(endUtc, userTimeZone);

        //            AppointmentsDataGridView.Rows.Add(
        //                row["customerName"],
        //                row["title"],
        //                AppointmentHelper.FormatAppointmentTimeZones(start, selectedTimeZoneId),
        //                AppointmentHelper.FormatAppointmentTimeZones(end, selectedTimeZoneId),
        //                row["type"]
        //            );
        //        }
        //    }
        //}

        private void LoadAppointments(int? customerId = null)
        {
            DataTable appointments = customerId.HasValue ?
                dbHelper.GetCustomerAppointments(customerId.Value) :
                dbHelper.GetAllAppointments();

            AppointmentsDataGridView.Rows.Clear();
            if (appointments != null && appointments.Rows.Count > 0)
            {
                string selectedTimeZoneId = TimeZoneComboBox.SelectedValue?.ToString() ?? TimeZoneInfo.Local.Id;

                foreach (DataRow row in appointments.Rows)
                {
                    DateTime startUtc = ((DateTime)row["start"]).ToUniversalTime();
                    DateTime endUtc = ((DateTime)row["end"]).ToUniversalTime();

                    var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(selectedTimeZoneId);
                    DateTime start = TimeZoneInfo.ConvertTimeFromUtc(startUtc, userTimeZone);
                    DateTime end = TimeZoneInfo.ConvertTimeFromUtc(endUtc, userTimeZone);

                    AppointmentsDataGridView.Rows.Add(
                        row["customerName"],
                        row["title"],
                        AppointmentHelper.FormatAppointmentTimeZones(start, selectedTimeZoneId),
                        AppointmentHelper.FormatAppointmentTimeZones(end, selectedTimeZoneId),
                        row["type"]
                    );
                }

                // Adjust column widths
                AppointmentsDataGridView.Columns["start"].Width = 200;
                AppointmentsDataGridView.Columns["end"].Width = 200;

                // Set default row height to accommodate two lines
                AppointmentsDataGridView.RowTemplate.Height = 40;
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
                string selectedTimeZoneId = TimeZoneComboBox.SelectedValue.ToString();
                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(selectedTimeZoneId);
                DateTime startTime = DateTime.SpecifyKind(StartDatePicker.Value, DateTimeKind.Local);
                DateTime endTime = DateTime.SpecifyKind(EndDatePicker.Value, DateTimeKind.Local);
                DateTime startInUserTz = TimeZoneInfo.ConvertTime(startTime, TimeZoneInfo.Local, userTimeZone);
                DateTime endInUserTz = TimeZoneInfo.ConvertTime(endTime, TimeZoneInfo.Local, userTimeZone);
                DateTime startUtc = TimeZoneInfo.ConvertTimeToUtc(startInUserTz, userTimeZone);
                DateTime endUtc = TimeZoneInfo.ConvertTimeToUtc(endInUserTz, userTimeZone);
                DateTime startLocal = TimeZoneInfo.ConvertTimeFromUtc(startUtc, TimeZoneInfo.Local);
                DateTime endLocal = TimeZoneInfo.ConvertTimeFromUtc(endUtc, TimeZoneInfo.Local);
                DataTable existingAppointments = dbHelper.GetAllAppointments();

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
                if (string.IsNullOrWhiteSpace(TypeInput.Text))
                {
                    MessageBox.Show("Type is required.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (endTime <= startTime)
                {
                    MessageBox.Show("End time must be after start time.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (!AppointmentHelper.IsWithinBusinessHours(startLocal, endLocal))
                {
                    var (localStart, localEnd) = AppointmentHelper.GetAdjustedBusinessHours(startTime.Date, selectedTimeZoneId);
                    MessageBox.Show(
                        $"Appointments must be scheduled within business hours:\n\n" +
                        $"Your timezone: {localStart:HH:mm} - {localEnd:HH:mm}\n" +
                        $"Eastern Time: 09:00 - 17:00\n\n" +
                        $"Please adjust your appointment time.",
                        "Validation Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                if (AppointmentHelper.HasOverlappingAppointments(startLocal, endLocal, existingAppointments))
                {
                    MessageBox.Show(
                       "This appointment overlaps with an existing appointment.\n" +
                       "Please choose a different time.",
                       "Validation Error",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Warning);
                    return;
                }

                // Add appointment using UTC times
                var (success, message) = dbHelper.AddAppointment(
                    selectedCustomerId.Value,
                    1, // Current user ID
                    TitleInput.Text.Trim(),
                    DescriptionInput.Text.Trim(),
                    LocationInput.Text.Trim(),
                    ContactInput.Text.Trim(),
                    TypeInput.Text.Trim(),
                    URLInput.Text.Trim(),
                    startUtc,
                    endUtc
                );

                if (success)
                {
                    MessageBox.Show("Appointment created successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAppointments(selectedCustomerId);
                    ClearForm();
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