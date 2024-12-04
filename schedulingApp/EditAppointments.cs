using System;
using System.Data;
using System.Windows.Forms;

namespace schedulingApp
{
    public partial class EditAppointments : Form
    {
        private readonly DatabaseHelper dbHelper;
        private int currentAppointmentId;
        private int currentCustomerId;
        private int currentUserId;
        private ComboBox TimeZoneComboBox;
        //private Label TimezoneInfoLabel;

        public EditAppointments(int appointmentId)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            currentAppointmentId = appointmentId;

            // Set minimum date for date pickers
            StartDatePicker.Format = DateTimePickerFormat.Custom;
            StartDatePicker.CustomFormat = "MM/dd/yyyy HH:mm tt";
            EndDatePicker.Format = DateTimePickerFormat.Custom;
            EndDatePicker.CustomFormat = "MM/dd/yyyy HH:mm tt";

            StartDatePicker.MinDate = new DateTime(2024, 1, 1, 9,0,0);
            EndDatePicker.MinDate = new DateTime(2018, 1, 1, 17, 0, 0);

            InitializeForm();
           

            // Wire up event handlers
            bttnSaveChanges.Click += BttnSaveChanges_Click;
            bttnExit.Click += BttnExit_Click;
            this.Load += EditAppointments_Load;
            //TimeZoneComboBox.SelectedIndexChanged += TimeZoneComboBox_SelectedIndexChanged;
            StartDatePicker.ValueChanged += DatePicker_ValueChanged;
            EndDatePicker.ValueChanged += DatePicker_ValueChanged;
        }
        private void InitializeForm()
        {
            // Set up DataGridView
            AppointmentsDataGridView.AutoGenerateColumns = false;
            AppointmentsDataGridView.Columns.Clear();

            // Add only the columns we want to display
            AppointmentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "customerName",
                DataPropertyName = "customerName",
                HeaderText = "Customer"
            });

            AppointmentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "title",
                DataPropertyName = "title",
                HeaderText = "Title"
            });

            AppointmentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "start",
                DataPropertyName = "start",
                HeaderText = "Start Time",
                Width = 200
            });

            AppointmentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "end",
                DataPropertyName = "end",
                HeaderText = "End Time",
                Width = 200
            });

            AppointmentsDataGridView.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "type",
                DataPropertyName = "type",
                HeaderText = "Type"
            });

            // Configure DataGridView for multiline cells
            AppointmentsDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            AppointmentsDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            AppointmentsDataGridView.RowTemplate.Height = 40;
        }


        private void RefreshAppointmentsGrid()
        {
            var appointments = dbHelper.GetAllAppointments();
            if (appointments != null)
            {
                DataTable displayTable = new DataTable();
                displayTable.Columns.Add("customerName", typeof(string));
                displayTable.Columns.Add("title", typeof(string));
                displayTable.Columns.Add("start", typeof(string));
                displayTable.Columns.Add("end", typeof(string));
                displayTable.Columns.Add("type", typeof(string));

                foreach (DataRow row in appointments.Rows)
                {
                    //Convert from UtC to EST 
                    DateTime startConvertKind = DateTime.SpecifyKind(Convert.ToDateTime(row["start"]), DateTimeKind.Utc);
                    DateTime endConvertKind = DateTime.SpecifyKind(Convert.ToDateTime(row["end"]), DateTimeKind.Utc);

                    //Converting to EST for display (have to - the Est offset, which is 5)
                    DateTime startAdjustedToEst = startConvertKind.AddHours(-5);
                    DateTime endAdjustedToEst = endConvertKind.AddHours(-5);

                    // Format times for display
                    string startDisplay = $"{startAdjustedToEst:MM/dd/yyyy HH:mm tt} EST";
                    string endDisplay = $"{endAdjustedToEst:MM/dd/yyyy HH:mm tt} EST";

                    displayTable.Rows.Add(
                        row["customerName"],
                        row["title"],
                        startDisplay,
                        endDisplay,
                        row["type"]
                    );
                }

                AppointmentsDataGridView.DataSource = displayTable;
            }
        }

        //private void LoadAppointments()
        //{
        //    var appointments = dbHelper.GetAllAppointments();

        //    AppointmentsDataGridView.Rows.Clear();

        //    if (appointments != null && appointments.Rows.Count > 0)
        //    {
        //        var easternZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");

        //        foreach (DataRow row in appointments.Rows)
        //        {
        //            //Convert from UtC to EST 
        //            DateTime startConvertKind = DateTime.SpecifyKind(Convert.ToDateTime(row["start"]), DateTimeKind.Utc);
        //            DateTime endConvertKind = DateTime.SpecifyKind(Convert.ToDateTime(row["end"]), DateTimeKind.Utc);

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

        private void LoadAppointmentData()
        {
            try
            {
                var appointments = dbHelper.GetAllAppointments();
                if (appointments == null || appointments.Rows.Count == 0) return;

                DataRow appointmentRow = null;
                DateTime startEstToSet = DateTime.Now;  // Default value
                DateTime endEstToSet = DateTime.Now;    // Default value
                AppointmentsDataGridView.Rows.Clear();

                foreach (DataRow row in appointments.Rows)
                {
                    //Convert from UtC to EST 
                    DateTime startConvertKind = DateTime.SpecifyKind(Convert.ToDateTime(row["start"]), DateTimeKind.Utc);
                    DateTime endConvertKind = DateTime.SpecifyKind(Convert.ToDateTime(row["end"]), DateTimeKind.Utc);

                    //Converting to EST for display (have to - the Est offset, which is 5)
                    DateTime startAdjustedToEst = startConvertKind.AddHours(-5);
                    DateTime endAdjustedToEst = endConvertKind.AddHours(-5);

                    // If this is our target appointment, store the EST times
                    if (row.Field<int>("appointmentId") == currentAppointmentId)
                    {
                        appointmentRow = row;
                        startEstToSet = startAdjustedToEst;
                        endEstToSet = endAdjustedToEst;
                    }

                    // Format times for display
                    string startDisplay = $"{startAdjustedToEst:MM/dd/yyyy HH:mm tt} EST";
                    string endDisplay = $"{endAdjustedToEst:MM/dd/yyyy HH:mm tt} EST";

                    AppointmentsDataGridView.Rows.Add(
                        row["customerName"],
                        row["title"],
                        startDisplay,
                        endDisplay,
                        row["type"]
                    );
                }

                // Populate form if appointment was found
                if (appointmentRow != null)
                {
                    currentCustomerId = appointmentRow.Field<int>("customerId");
                    currentUserId = appointmentRow.Field<int>("userId");

                    TitleInput.Text = appointmentRow.Field<string>("title");
                    DescriptionInput.Text = appointmentRow.Field<string>("description");
                    LocationInput.Text = appointmentRow.Field<string>("location");
                    ContactInput.Text = appointmentRow.Field<string>("contact");
                    TypeInput.Text = appointmentRow.Field<string>("type");
                    URLInput.Text = appointmentRow.Field<string>("url");

                    // Use the EST times we already calculated
                    StartDatePicker.Value = startEstToSet < StartDatePicker.MinDate ? StartDatePicker.MinDate : startEstToSet;
                    EndDatePicker.Value = endEstToSet < EndDatePicker.MinDate ? EndDatePicker.MinDate : endEstToSet;

                    RefreshAppointmentsGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointment data: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //private void RefreshAppointmentsGrid()
        //{
        //    var appointments = dbHelper.GetAllAppointments();
        //    if (appointments != null)
        //    {
        //        DataTable displayTable = new DataTable();
        //        displayTable.Columns.Add("customerName", typeof(string));
        //        displayTable.Columns.Add("title", typeof(string));
        //        displayTable.Columns.Add("start", typeof(string));
        //        displayTable.Columns.Add("end", typeof(string));
        //        displayTable.Columns.Add("type", typeof(string));

        //        //string selectedTimeZoneId = TimeZoneComboBox.SelectedValue?.ToString() ?? TimeZoneInfo.Local.Id;

        //        foreach (DataRow row in appointments.Rows)
        //        {

        //            DateTime startTime = DateTime.SpecifyKind(Convert.ToDateTime(row["start"]), DateTimeKind.Utc);
        //            DateTime endTime = DateTime.SpecifyKind(Convert.ToDateTime(row["end"]), DateTimeKind.Utc);


        //            //Convert from UtC to EST 

        //            DateTime startConvertKind = DateTime.SpecifyKind(Convert.ToDateTime(row["start"]), DateTimeKind.Utc);
        //            DateTime endConvertKind = DateTime.SpecifyKind(Convert.ToDateTime(row["end"]), DateTimeKind.Utc);

        //            //Converting to EST for display (have to - the Est offset, which is 5)
        //            DateTime startAdjustedToEst = startConvertKind.AddHours(-5);
        //            DateTime endAdjustedToEst = endConvertKind.AddHours(-5);


        //            // Format times for display
        //            string startDisplay = $"{startAdjustedToEst:MM/dd/yyyy HH:mm tt} EST";
        //            string endDisplay = $"{endAdjustedToEst:MM/dd/yyyy HH:mm tt} EST";




        //            DateTime startEst = startTime.AddHours(-5);
        //            DateTime endEst = endTime.AddHours(-5);
        //            //DateTime startUtc = ((DateTime)row["start"]).ToUniversalTime();
        //            //DateTime endUtc = ((DateTime)row["end"]).ToUniversalTime();

        //            //var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(selectedTimeZoneId);
        //            //DateTime start = TimeZoneInfo.ConvertTimeFromUtc(startUtc, userTimeZone);
        //            //DateTime end = TimeZoneInfo.ConvertTimeFromUtc(endUtc, userTimeZone);

        //            displayTable.Rows.Add(
        //                row["customerName"],
        //                row["title"],
        //                row["start"],
        //                row["end"],
        //                //AppointmentHelper.FormatAppointmentTimeZones(start, selectedTimeZoneId),
        //                //AppointmentHelper.FormatAppointmentTimeZones(end, selectedTimeZoneId),
        //                row["type"]
        //            );
        //        }

        //        AppointmentsDataGridView.DataSource = displayTable;
        //    }
        //}

        private void TimeZoneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TimeZoneComboBox.SelectedValue == null) return;

            try
            {
                string selectedTimeZoneId = TimeZoneComboBox.SelectedValue.ToString();
                //TimezoneInfoLabel.Text = AppointmentHelper.GetTimeZoneInfoMessage(selectedTimeZoneId);

                var (adjustedStart, adjustedEnd) = AppointmentHelper.AdjustAppointmentTimesForTimeZone(
                    StartDatePicker.Value,
                    EndDatePicker.Value,
                    TimeZoneInfo.Local.Id,
                    selectedTimeZoneId);

                StartDatePicker.ValueChanged -= DatePicker_ValueChanged;
                EndDatePicker.ValueChanged -= DatePicker_ValueChanged;

                if (adjustedStart >= StartDatePicker.MinDate)
                    StartDatePicker.Value = adjustedStart;
                if (adjustedEnd >= EndDatePicker.MinDate)
                    EndDatePicker.Value = adjustedEnd;

                StartDatePicker.ValueChanged += DatePicker_ValueChanged;
                EndDatePicker.ValueChanged += DatePicker_ValueChanged;

                // Refresh the grid to show times in the selected timezone
                RefreshAppointmentsGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adjusting timezone: {ex.Message}",
                    "Timezone Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EditAppointments_Load(object sender, EventArgs e)
        {
            LoadAppointmentData();
            LoadCustomers();
        }

        private void LoadCustomers()
        {
            var customers = dbHelper.GetAllCustomers();
            if (customers != null)
            {
                CustomerComboBox.DataSource = customers;
                CustomerComboBox.DisplayMember = "customerName";
                CustomerComboBox.ValueMember = "customerId";
                CustomerComboBox.SelectedValue = currentCustomerId;
            }
        }
        //private void LoadAppointmentData()
        //{
        //    try
        //    {
        //        var appointments = dbHelper.GetAllAppointments();
        //        if (appointments == null || appointments.Rows.Count == 0) return;

        //        DataRow appointmentRow = null;
        //        DateTime startEstToSet = DateTime.Now;  // Default value
        //        DateTime endEstToSet = DateTime.Now;    // Default value
        //        AppointmentsDataGridView.Rows.Clear();

        //        for (int i = 0; i < appointments.Rows.Count; i++)
        //        {
        //            DataRow row = appointments.Rows[i];

        //            // Convert times from UTC to EST
        //            DateTime startTime = DateTime.SpecifyKind(Convert.ToDateTime(row["start"]), DateTimeKind.Utc);
        //            DateTime endTime = DateTime.SpecifyKind(Convert.ToDateTime(row["end"]), DateTimeKind.Utc);

        //            DateTime startEst = startTime.AddHours(-5);
        //            DateTime endEst = endTime.AddHours(-5);

        //            // If this is our target appointment, store the EST times
        //            if (row.Field<int>("appointmentId") == currentAppointmentId)
        //            {
        //                appointmentRow = row;
        //                startEstToSet = startEst;
        //                endEstToSet = endEst;
        //            }

        //            // Format and add to grid
        //            AppointmentsDataGridView.Rows.Add(
        //                row["customerName"],
        //                row["title"],
        //                $"{startEst:MM/dd/yyyy HH:mm tt} EST",
        //                $"{endEst:MM/dd/yyyy HH:mm tt} EST",
        //                row["type"]
        //            );
        //        }

        //        // Populate form if appointment was found
        //        if (appointmentRow != null)
        //        {
        //            currentCustomerId = appointmentRow.Field<int>("customerId");
        //            currentUserId = appointmentRow.Field<int>("userId");

        //            TitleInput.Text = appointmentRow.Field<string>("title");
        //            DescriptionInput.Text = appointmentRow.Field<string>("description");
        //            LocationInput.Text = appointmentRow.Field<string>("location");
        //            ContactInput.Text = appointmentRow.Field<string>("contact");
        //            TypeInput.Text = appointmentRow.Field<string>("type");
        //            URLInput.Text = appointmentRow.Field<string>("url");

        //            // Use the EST times we already calculated
        //            StartDatePicker.Value = startEstToSet < StartDatePicker.MinDate ? StartDatePicker.MinDate : startEstToSet;
        //            EndDatePicker.Value = endEstToSet < EndDatePicker.MinDate ? EndDatePicker.MinDate : endEstToSet;

        //            RefreshAppointmentsGrid();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error loading appointment data: {ex.Message}",
        //            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }





        //}
        private void DatePicker_ValueChanged(object sender, EventArgs e)
        {
            // Ensure end date is not before start date
            if (EndDatePicker.Value < StartDatePicker.Value)
            {
                EndDatePicker.Value = StartDatePicker.Value.AddHours(1);
            }
        }

        private (bool isValid, string message) ValidateAppointmentInput()
        {
            if (string.IsNullOrWhiteSpace(TitleInput.Text))
                return (false, "Title is required.");

            if (string.IsNullOrWhiteSpace(TypeInput.Text))
                return (false, "Type is required.");

            if (CustomerComboBox.SelectedValue == null)
                return (false, "Please select a customer.");

            if (StartDatePicker.Value >= EndDatePicker.Value)
                return (false, "End time must be after start time.");

            // Check business hours (8:00 AM to 5:00 PM)
            if (StartDatePicker.Value.Hour < 8 || StartDatePicker.Value.Hour >= 17 ||
                EndDatePicker.Value.Hour < 8 || EndDatePicker.Value.Hour >= 17)
                return (false, "Appointments must be scheduled between 8:00 AM and 5:00 PM.");


            return (true, string.Empty);
        }
        public static bool IsWithinBusinessHours(DateTime startTime, DateTime endTime)
        {
            //receives the time input by user 

            //check if it's a weekday
            if (startTime.DayOfWeek == DayOfWeek.Saturday || startTime.DayOfWeek == DayOfWeek.Sunday) return false;
            //check if appt streaches for more than 1 day
            if (startTime.Date != endTime.Date) return false;
            if (startTime.TimeOfDay < TimeSpan.FromHours(9) || endTime.TimeOfDay > TimeSpan.FromHours(17)) return false;

            return true;

        }
        //private void BttnSaveChanges_Click(object sender, EventArgs e)
        //{
        //    DateTime startTimeUtc = StartDatePicker.Value.AddHours(5);
        //    DateTime endTimeUtc = EndDatePicker.Value.AddHours(5);

        //    try
        //    {
        //        var (isValid, message) = ValidateAppointmentInput();
        //        if (!isValid)
        //        {
        //            MessageBox.Show(message, "Validation Error",
        //                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }
        //        if (!IsWithinBusinessHours(startTimeUtc, endTimeUtc))
        //        {
        //            MessageBox.Show("Time chosen is not within business hours. FIX IT!");
        //            return;
        //        }
        //        if (!dbHelper.HasOverlappingAppointments(startTimeUtc, endTimeUtc))

        //        // Update appointment

        //        var (success, updateMessage) = dbHelper.UpdateAppointment(
        //            currentAppointmentId,
        //            Convert.ToInt32(CustomerComboBox.SelectedValue),
        //            currentUserId,
        //            TitleInput.Text,
        //            DescriptionInput.Text,
        //            LocationInput.Text,
        //            ContactInput.Text,
        //            TypeInput.Text,
        //            URLInput.Text,
        //            StartDatePicker.Value,
        //            EndDatePicker.Value
        //        );

        //        if (success)
        //        {
        //            MessageBox.Show("Appointment updated successfully!",
        //                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            this.Close();
        //        }
        //        else
        //        {
        //            MessageBox.Show($"Error updating appointment: {updateMessage}",
        //                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error saving changes: {ex.Message}",
        //            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        private void BttnSaveChanges_Click(object sender, EventArgs e)
        {
            DateTime startTimeUtc = StartDatePicker.Value.AddHours(5);
            DateTime endTimeUtc = EndDatePicker.Value.AddHours(5);

            try
            {
                var (isValid, message) = ValidateAppointmentInput();
                if (!isValid)
                {
                    MessageBox.Show(message, "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!IsWithinBusinessHours(StartDatePicker.Value, EndDatePicker.Value))
                {
                    MessageBox.Show("Time chosen is not within business hours. FIX IT!");
                    return;
                }

                // Check for overlapping appointments
                if (dbHelper.HasOverlappingAppointments(startTimeUtc, endTimeUtc, currentAppointmentId))
                {
                    MessageBox.Show("This appointment overlaps with another appointment.",
                        "Overlap Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Update appointment
                var (success, updateMessage) = dbHelper.UpdateAppointment(
                    currentAppointmentId,
                    Convert.ToInt32(CustomerComboBox.SelectedValue),
                    dbHelper.GetUserIdByUsername(DatabaseHelper.CurrentUser),
                    TitleInput.Text,
                    DescriptionInput.Text,
                    LocationInput.Text,
                    ContactInput.Text,
                    TypeInput.Text,
                    URLInput.Text,
                    startTimeUtc,
                    endTimeUtc
                );

                if (success)
                {
                    MessageBox.Show("Appointment updated successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    MessageBox.Show($"Error updating appointment: {updateMessage}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BttnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}