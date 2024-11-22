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
        private Label TimezoneInfoLabel;

        public EditAppointments(int appointmentId)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            currentAppointmentId = appointmentId;

            // Set minimum date for date pickers
            StartDatePicker.Format = DateTimePickerFormat.Custom;
            StartDatePicker.CustomFormat = "MM/dd/yyyy hh:mm tt";
            EndDatePicker.Format = DateTimePickerFormat.Custom;
            EndDatePicker.CustomFormat = "MM/dd/yyyy hh:mm tt";

            StartDatePicker.MinDate = new DateTime(2018, 1, 1, 6, 0, 0);
            EndDatePicker.MinDate = new DateTime(2018, 1, 1, 6, 0, 0);

            InitializeForm();
            InitializeTimezoneControls();

            // Wire up event handlers
            bttnSaveChanges.Click += BttnSaveChanges_Click;
            bttnExit.Click += BttnExit_Click;
            this.Load += EditAppointments_Load;
            TimeZoneComboBox.SelectedIndexChanged += TimeZoneComboBox_SelectedIndexChanged;
            StartDatePicker.ValueChanged += DatePicker_ValueChanged;
            EndDatePicker.ValueChanged += DatePicker_ValueChanged;
        }

        private void InitializeTimezoneControls()
        {
            TimeZoneComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(StartDatePicker.Left, StartDatePicker.Bottom + 20),
                Width = 300,
                Font = new Font("Segoe UI", 9F)
            };

            TimezoneInfoLabel = new Label
            {
                AutoSize = true,
                Location = new Point(TimeZoneComboBox.Left, TimeZoneComboBox.Bottom + 10),
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Black
            };

            this.Controls.Add(TimeZoneComboBox);
            this.Controls.Add(TimezoneInfoLabel);

            var timeZones = AppointmentHelper.GetAvailableTimeZones();
            TimeZoneComboBox.DataSource = timeZones;
            TimeZoneComboBox.DisplayMember = "DisplayName";
            TimeZoneComboBox.ValueMember = "Id";
            TimeZoneComboBox.SelectedValue = TimeZoneInfo.Local.Id;
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

                string selectedTimeZoneId = TimeZoneComboBox.SelectedValue?.ToString() ?? TimeZoneInfo.Local.Id;

                foreach (DataRow row in appointments.Rows)
                {
                    DateTime startUtc = ((DateTime)row["start"]).ToUniversalTime();
                    DateTime endUtc = ((DateTime)row["end"]).ToUniversalTime();

                    var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(selectedTimeZoneId);
                    DateTime start = TimeZoneInfo.ConvertTimeFromUtc(startUtc, userTimeZone);
                    DateTime end = TimeZoneInfo.ConvertTimeFromUtc(endUtc, userTimeZone);

                    displayTable.Rows.Add(
                        row["customerName"],
                        row["title"],
                        AppointmentHelper.FormatAppointmentTimeZones(start, selectedTimeZoneId),
                        AppointmentHelper.FormatAppointmentTimeZones(end, selectedTimeZoneId),
                        row["type"]
                    );
                }

                AppointmentsDataGridView.DataSource = displayTable;
            }
        }

        private void TimeZoneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TimeZoneComboBox.SelectedValue == null) return;

            try
            {
                string selectedTimeZoneId = TimeZoneComboBox.SelectedValue.ToString();
                TimezoneInfoLabel.Text = AppointmentHelper.GetTimeZoneInfoMessage(selectedTimeZoneId);

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
        private void LoadAppointmentData()
        {
            try
            {
                var appointments = dbHelper.GetAllAppointments();
                if (appointments != null)
                {
                    DataRow appointmentRow = appointments.AsEnumerable()
                        .FirstOrDefault(row => row.Field<int>("appointmentId") == currentAppointmentId);

                    if (appointmentRow != null)
                    {
                        // Store IDs
                        currentCustomerId = appointmentRow.Field<int>("customerId");
                        currentUserId = appointmentRow.Field<int>("userId");

                        // Populate fields
                        TitleInput.Text = appointmentRow.Field<string>("title");
                        DescriptionInput.Text = appointmentRow.Field<string>("description");
                        LocationInput.Text = appointmentRow.Field<string>("location");
                        ContactInput.Text = appointmentRow.Field<string>("contact");
                        TypeInput.Text = appointmentRow.Field<string>("type");
                        URLInput.Text = appointmentRow.Field<string>("url");

                        // Convert UTC times to local
                        DateTime startUtc = appointmentRow.Field<DateTime>("start");
                        DateTime endUtc = appointmentRow.Field<DateTime>("end");
                        DateTime localStart = TimeZoneInfo.ConvertTimeFromUtc(startUtc, TimeZoneInfo.Local);
                        DateTime localEnd = TimeZoneInfo.ConvertTimeFromUtc(endUtc, TimeZoneInfo.Local);
                        //StartDatePicker.Value = TimeZoneInfo.ConvertTimeFromUtc(startUtc, TimeZoneInfo.Local);
                        //EndDatePicker.Value = TimeZoneInfo.ConvertTimeFromUtc(endUtc, TimeZoneInfo.Local);


                        StartDatePicker.Value = localStart < StartDatePicker.MinDate ? StartDatePicker.MinDate : localStart;
                        EndDatePicker.Value = localEnd < EndDatePicker.MinDate ? EndDatePicker.MinDate : localEnd;


                        // Load current appointments for reference
                        RefreshAppointmentsGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading appointment data: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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

            // Check for overlapping appointments
            DateTime startUtc = TimeZoneInfo.ConvertTimeToUtc(StartDatePicker.Value);
            DateTime endUtc = TimeZoneInfo.ConvertTimeToUtc(EndDatePicker.Value);


            return (true, string.Empty);
        }

        private void BttnSaveChanges_Click(object sender, EventArgs e)
        {
            try
            {
                var (isValid, message) = ValidateAppointmentInput();
                if (!isValid)
                {
                    MessageBox.Show(message, "Validation Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Update appointment
                
                var (success, updateMessage) = dbHelper.UpdateAppointment(
                    currentAppointmentId,
                    Convert.ToInt32(CustomerComboBox.SelectedValue),
                    currentUserId,
                    TitleInput.Text,
                    DescriptionInput.Text,
                    LocationInput.Text,
                    ContactInput.Text,
                    TypeInput.Text,
                    URLInput.Text,
                    StartDatePicker.Value,
                    EndDatePicker.Value
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