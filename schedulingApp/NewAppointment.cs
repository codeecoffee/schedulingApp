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
    //[ok]
    public partial class NewAppointment : Form
    {
        private readonly DatabaseHelper dbHelper;
        private int? selectedCustomerId = null;
        private Label timezoneInfoLabel;
        private readonly Label businessHoursLabel;
        private readonly TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
        private ComboBox timezoneComboBox; // New timezone selector
        private TimeZoneInfo selectedTimeZone; // Store selected timezone
        private bool isUpdatingPickers = false; // Prevent recursive updates


        public NewAppointment()
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();

            // Add timezone selector
            Label lblTimeZone = new Label
            {
                AutoSize = true,
                Location = new Point(375, StartDatePicker.Bottom + 10),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Text = "Your Time Zone:"
            };
            panel1.Controls.Add(lblTimeZone);

            timezoneComboBox = new ComboBox
            {
                Location = new Point(375, lblTimeZone.Bottom + 5),
                Width = 300,
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            panel1.Controls.Add(timezoneComboBox);

            // Load timezones into combo box
            var timeZones = TimeZoneInfo.GetSystemTimeZones()
                .OrderBy(tz => tz.BaseUtcOffset)
                .ToList();
            timezoneComboBox.DataSource = timeZones;
            timezoneComboBox.DisplayMember = "DisplayName";
            timezoneComboBox.SelectedItem = TimeZoneInfo.Local;
            selectedTimeZone = TimeZoneInfo.Local;

            timezoneComboBox.SelectedIndexChanged += TimezoneComboBox_SelectedIndexChanged;

            // Adjust existing timezone info label position
            timezoneInfoLabel = new Label
            {
                AutoSize = true,
                Location = new Point(375, timezoneComboBox.Bottom + 10),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.Black,
                BackColor = Color.White,
                Text = GetTimezoneDifferenceMessage()
            };
            panel1.Controls.Add(timezoneInfoLabel);

            // Adjust business hours label position
            businessHoursLabel = new Label
            {
                AutoSize = true,
                Location = new Point(39, StartDatePicker.Bottom + 20),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                ForeColor = Color.Black,
                BackColor = Color.White,
                Text = "Business Hours (EST): 09:00 ~ 17:00"
            };
            panel1.Controls.Add(businessHoursLabel);

            // Add time conversion labels
            Label lblLocalTime = new Label
            {
                AutoSize = true,
                Location = new Point(39, businessHoursLabel.Bottom + 10),
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                Name = "lblLocalTime",
                Text = "Selected time in your timezone"
            };
            panel1.Controls.Add(lblLocalTime);

            InitializeForm();
            LoadCustomers();
            LoadAppointments();

            // Add value changed handlers for date pickers
            StartDatePicker.ValueChanged += DatePicker_ValueChanged;
            EndDatePicker.ValueChanged += DatePicker_ValueChanged;
        }

        private void TimezoneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isUpdatingPickers) return;

            selectedTimeZone = (TimeZoneInfo)timezoneComboBox.SelectedItem;

            // Convert the local times to UTC first
            var startUtc = TimeZoneInfo.ConvertTimeToUtc(StartDatePicker.Value, TimeZoneInfo.Local);
            var endUtc = TimeZoneInfo.ConvertTimeToUtc(EndDatePicker.Value, TimeZoneInfo.Local);

            // Then convert from UTC to the newly selected timezone
            isUpdatingPickers = true;
            StartDatePicker.Value = TimeZoneInfo.ConvertTimeFromUtc(startUtc, selectedTimeZone);
            EndDatePicker.Value = TimeZoneInfo.ConvertTimeFromUtc(endUtc, selectedTimeZone);
            isUpdatingPickers = false;

            UpdateTimezoneLabels();
        }

        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (!isUpdatingPickers)
            {
                UpdateTimezoneLabels();
            }
        }

        private void UpdateTimezoneLabels()
        {
            // Convert selected times to EST for display
            var startUtc = TimeZoneInfo.ConvertTimeToUtc(StartDatePicker.Value, TimeZoneInfo.Local);
            var endUtc = TimeZoneInfo.ConvertTimeToUtc(EndDatePicker.Value, TimeZoneInfo.Local);

            DateTime startEst = TimeZoneInfo.ConvertTimeFromUtc(startUtc, easternZone);
            DateTime endEst = TimeZoneInfo.ConvertTimeFromUtc(endUtc, easternZone);

            timezoneInfoLabel.Text = $"Selected time in EST - Start: {startEst:HH:mm}, End: {endEst:HH:mm}";
        }

        private string GetTimezoneDifferenceMessage()
        {
            DateTime now = DateTime.Now;
            DateTime easternTime = TimeZoneInfo.ConvertTime(now, TimeZoneInfo.Local, easternZone);
            TimeSpan difference = selectedTimeZone.BaseUtcOffset - easternZone.BaseUtcOffset;
            string sign = difference.Hours >= 0 ? "+" : "";

            // Add DST information for both timezones
            string localDst = selectedTimeZone.IsDaylightSavingTime(now) ? "DST Active" : "Standard Time";
            string estDst = easternZone.IsDaylightSavingTime(easternTime) ? "DST Active" : "Standard Time";

            return $"Time difference from EST: {sign}{difference.Hours} hours | Local: {localDst}, EST: {estDst}";
        }

        // Modify the existing IsWithinBusinessHours method to handle timezone conversion
        public bool IsWithinBusinessHours(DateTime startTime, DateTime endTime)
        {
            // Convert input times to EST for validation
            DateTime startEst = TimeZoneInfo.ConvertTime(startTime, selectedTimeZone, easternZone);
            DateTime endEst = TimeZoneInfo.ConvertTime(endTime, selectedTimeZone, easternZone);

            if (startEst.DayOfWeek == DayOfWeek.Saturday || startEst.DayOfWeek == DayOfWeek.Sunday) return false;
            if (startEst.Date != endEst.Date) return false;
            if (startEst.TimeOfDay < TimeSpan.FromHours(9) || endEst.TimeOfDay > TimeSpan.FromHours(17)) return false;

            return true;
        }

        // Modified the BttnCreate_Click to handle timezone conversion
        private void BttnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                //no customer selected
                if (!selectedCustomerId.HasValue)
                {
                    MessageBox.Show("Please select a customer.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //no title
                if (string.IsNullOrWhiteSpace(TitleInput.Text))
                {
                    MessageBox.Show("Title is required.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //no type
                if (string.IsNullOrWhiteSpace(TypeInput.Text))
                {
                    MessageBox.Show("Type is required.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //appt in the past 
                if (DateTime.Now > StartDatePicker.Value || DateTime.Now > EndDatePicker.Value)
                {
                    MessageBox.Show("Cannot create appointments in the past.", "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DateTime startTimeUtc = TimeZoneInfo.ConvertTimeToUtc(StartDatePicker.Value, TimeZoneInfo.Local);
                DateTime endTimeUtc = TimeZoneInfo.ConvertTimeToUtc(EndDatePicker.Value, TimeZoneInfo.Local);

                // Convert UTC to EST for business hours validation
                DateTime startEst = TimeZoneInfo.ConvertTimeFromUtc(startTimeUtc, easternZone);
                DateTime endEst = TimeZoneInfo.ConvertTimeFromUtc(endTimeUtc, easternZone);

                if (!IsWithinBusinessHours(startEst, endEst))
                {
                    MessageBox.Show(
                        "Appointments must be scheduled within business hours (9 AM - 5 PM Eastern Time) " +
                        "and cannot occur on weekends.",
                        "Invalid Appointment Time",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                if (dbHelper.HasOverlappingAppointments(startTimeUtc, endTimeUtc))
                {
                    MessageBox.Show("There is already another appointment scheduled for this time");
                    return;
                }
                int userId;
                try
                {
                    userId = dbHelper.GetUserIdByUsername(DatabaseHelper.CurrentUser);
                    Console.WriteLine($"Current user: {DatabaseHelper.CurrentUser}, User ID: {userId}");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error getting user ID: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var (success, message) = dbHelper.AddAppointment(
                    selectedCustomerId.Value,
                    userId, //curr user ID 
                    TitleInput.Text.Trim(),
                    DescriptionInput.Text.Trim(),
                    LocationInput.Text.Trim(),
                    ContactInput.Text.Trim(),
                    TypeInput.Text.Trim(),
                    URLInput.Text.Trim(),
                    startTimeUtc, endTimeUtc
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
        // Modify LoadAppointments to show times in user's selected timezone
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
                    DateTime startUtc = DateTime.SpecifyKind(Convert.ToDateTime(row["start"]), DateTimeKind.Utc);
                    DateTime endUtc = DateTime.SpecifyKind(Convert.ToDateTime(row["end"]), DateTimeKind.Utc);

                    // Now convert from UTC to the selected timezone
                    DateTime startLocal = TimeZoneInfo.ConvertTimeFromUtc(startUtc, selectedTimeZone);
                    DateTime endLocal = TimeZoneInfo.ConvertTimeFromUtc(endUtc, selectedTimeZone);

                    string startDisplay = $"{startLocal:MM/dd/yyyy HH:mm} (UTC{selectedTimeZone.GetUtcOffset(startLocal):hh\\:mm})";
                    string endDisplay = $"{endLocal:MM/dd/yyyy HH:mm} (UTC{selectedTimeZone.GetUtcOffset(endLocal):hh\\:mm})";


                    AppointmentsDataGridView.Rows.Add(
                        row["customerName"],
                        row["title"],
                        startDisplay,
                        endDisplay,
                        row["type"]
                    );
                }
            }
        }
        //[ok]
        //private string GetTimezoneDifferenceMessage() 
        //{
        //    DateTime now = DateTime.Now;
        //    DateTime easternTime = TimeZoneInfo.ConvertTime(now, TimeZoneInfo.Local, easternZone);

        //    return $"Current Time - Local: {now:HH:mm}, Eastern: {easternTime:HH:mm}";
        //}

       
        //public static bool IsWithinBusinessHours(DateTime startTime, DateTime endTime)
        //{
        //   //receives the time input by user 

        //    //check if it's a weekday
        //    if (startTime.DayOfWeek == DayOfWeek.Saturday || startTime.DayOfWeek == DayOfWeek.Sunday) return false;
        //    //check if appt streaches for more than 1 day
        //    if (startTime.Date != endTime.Date) return false;
        //    if (startTime.TimeOfDay < TimeSpan.FromHours(9) || endTime.TimeOfDay > TimeSpan.FromHours(17)) return false;

        //    return true;
                        
        //}

        private void InitializeForm()
        {
            // Set up date/time pickers
            StartDatePicker.Format = DateTimePickerFormat.Custom;
            StartDatePicker.CustomFormat = "MM/dd/yyyy HH:mm";
            EndDatePicker.Format = DateTimePickerFormat.Custom;
            EndDatePicker.CustomFormat = "MM/dd/yyyy HH:mm";

            //min/  max time
            StartDatePicker.MinDate = DateTime.Now;
            StartDatePicker.MaxDate = DateTime.Today.AddYears(1);
            EndDatePicker.MinDate = DateTime.Now;
            EndDatePicker.MaxDate = DateTime.Today.AddYears(1);


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
           
            bttnCreate.Click += BttnCreate_Click;
            bttnExit.Click += BttnExit_Click;
        }
        
        //[ok]
        private void CustomerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CustomerComboBox.SelectedValue == DBNull.Value || CustomerComboBox.SelectedValue == null)
            {
                selectedCustomerId = null;
                LoadAppointments(); 
            }
            else
            {
                DataRowView drv = (DataRowView)CustomerComboBox.SelectedItem;
                if (drv["customerId"] != DBNull.Value)
                {
                    selectedCustomerId = Convert.ToInt32(drv["customerId"]);
                    LoadAppointments(selectedCustomerId);
                }
                else
                {
                    selectedCustomerId = null;
                    LoadAppointments();
                }
            }
        }
        //[ok]
        private void LoadCustomers()
        {
            DataTable customers = dbHelper.GetAllCustomers();

            // Create a new DataTable with the same schema
            DataTable customersWithAll = new DataTable();
            customersWithAll.Columns.Add("customerId", typeof(int));
            customersWithAll.Columns.Add("customerName", typeof(string));

            // Add empty option for showing all appointments
            DataRow allCustomersRow = customersWithAll.NewRow();
            allCustomersRow["customerId"] = DBNull.Value;
            allCustomersRow["customerName"] = "-- All Customers --";
            customersWithAll.Rows.Add(allCustomersRow);

            // Add all existing customers
            foreach (DataRow row in customers.Rows)
            {
                DataRow newRow = customersWithAll.NewRow();
                newRow["customerId"] = row["customerId"];
                newRow["customerName"] = row["customerName"];
                customersWithAll.Rows.Add(newRow);
            }

            CustomerComboBox.DataSource = customersWithAll;
            CustomerComboBox.DisplayMember = "customerName";
            CustomerComboBox.ValueMember = "customerId";
            CustomerComboBox.SelectedIndex = 0; // Select "All Customers" by default
        }
        //[ok]
        //private void LoadAppointments(int? customerId = null)
        //{
        //    DataTable appointments = customerId.HasValue ?
        //        dbHelper.GetCustomerAppointments(customerId.Value) :
        //        dbHelper.GetAllAppointments();

        //    AppointmentsDataGridView.Rows.Clear();

        //    if (appointments != null && appointments.Rows.Count > 0)
        //    {
        //        var easternZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");

        //        foreach (DataRow row in appointments.Rows)
        //        {
        //            //Convert from UtC to EST 

        //            DateTime startConvertKind = DateTime.SpecifyKind(Convert.ToDateTime(row["start"]), DateTimeKind.Utc); 
        //            DateTime endConvertKind =  DateTime.SpecifyKind(Convert.ToDateTime(row["end"]), DateTimeKind.Utc);

        //            //Converting to EST for display (have to - the Est offset, which is 5)
        //            DateTime startAdjustedToEst = startConvertKind.AddHours(-5);
        //            DateTime endAdjustedToEst = endConvertKind.AddHours(-5);


        //            // Format times for display
        //            string startDisplay = $"{startAdjustedToEst:MM/dd/yyyy HH:mm tt} EST";
        //            string endDisplay = $"{endAdjustedToEst:MM/dd/yyyy HH:mm tt} EST";

        //            AppointmentsDataGridView.Rows.Add(
        //                row["customerName"],
        //                row["title"],
        //                startDisplay,
        //                endDisplay,
        //                row["type"]
        //            );
        //        }
        //    }
        //}

        //private void BttnCreate_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //no customer selected
        //        if (!selectedCustomerId.HasValue)
        //        {
        //            MessageBox.Show("Please select a customer.", "Validation Error",
        //                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }
        //        //no title
        //        if (string.IsNullOrWhiteSpace(TitleInput.Text))
        //        {
        //            MessageBox.Show("Title is required.", "Validation Error",
        //                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }
        //        //no type
        //        if (string.IsNullOrWhiteSpace(TypeInput.Text))
        //        {
        //            MessageBox.Show("Type is required.", "Validation Error",
        //                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        DateTime startTime = StartDatePicker.Value;
        //        DateTime endTime = EndDatePicker.Value;

        //        //converting to UTC to save onto the DB using the 5 hour offset
        //        DateTime startTimeUtc = startTime.AddHours(5);
        //        DateTime endTimeUtc = endTime.AddHours(5);
                
        //        // end time before start time 
        //        if (endTime <= startTime)
        //        {
        //            MessageBox.Show("End time must be after start time.", "Validation Error",
        //                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        if (!IsWithinBusinessHours(startTime, endTime))
        //        {
        //            MessageBox.Show(
        //                "Appointments must be scheduled within business hours (9 AM - 5 PM Eastern Time) " +
        //                "and cannot occur on weekends.",
        //                "Invalid Appointment Time",
        //                MessageBoxButtons.OK,
        //                MessageBoxIcon.Warning);
        //            return;
        //        }
        //        //DateTime startUtc = DateTime.SpecifyKind(Convert.ToDateTime(startTime), DateTimeKind.Utc);
        //        //DateTime endUtc = DateTime.SpecifyKind(Convert.ToDateTime(endTime), DateTimeKind.Utc);

        //        //DateTime startUtc = startTime.ToUniversalTime();
        //        //DateTime endUtc = endTime.ToUniversalTime();

        //        //MessageBox.Show($"[func BttnCreate_Click] Start time input: {startTime} endTime: {endTime} | Time in UTC, Start: {startTimeUtc} END: {endTimeUtc}");

        //        if (dbHelper.HasOverlappingAppointments(startTimeUtc, endTimeUtc))
        //        {
        //            MessageBox.Show("There is already another appointment for the time you want");
        //            return;
        //        }

        //        int userId;
        //        try
        //        {
        //            userId = dbHelper.GetUserIdByUsername(DatabaseHelper.CurrentUser);
        //            Console.WriteLine($"Current user: {DatabaseHelper.CurrentUser}, User ID: {userId}");
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Error getting user ID: {ex.Message}", "Error",
        //                MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            return;
        //        }

        //        var (success, message) = dbHelper.AddAppointment(
        //            selectedCustomerId.Value,
        //            userId, //curr user ID 
        //            TitleInput.Text.Trim(),
        //            DescriptionInput.Text.Trim(),
        //            LocationInput.Text.Trim(),
        //            ContactInput.Text.Trim(),
        //            TypeInput.Text.Trim(),
        //            URLInput.Text.Trim(),
        //            startTimeUtc, endTimeUtc
        //        );

        //        if (success)
        //        {
        //            //MessageBox.Show($"current user: {dbHelper.GetUserIdByUsername(DatabaseHelper.CurrentUser)}");
        //            MessageBox.Show("Appointment created successfully!", "Success",
        //                MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            LoadAppointments(selectedCustomerId);
        //            ClearForm();
        //        }
        //        else
        //        {
        //            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"An error occurred: {ex.Message}", "Error",
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        
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
            CustomerComboBox.SelectedIndex = 0;
        }
        private void BttnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
    }
}