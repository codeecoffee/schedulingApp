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

        public EditAppointments(int appointmentId)
        {
            InitializeComponent();
            dbHelper = new DatabaseHelper();
            currentAppointmentId = appointmentId;

            // Wire up events
            bttnSaveChanges.Click += BttnSaveChanges_Click;
            bttnExit.Click += BttnExit_Click;
            this.Load += EditAppointments_Load;

            // Set minimum date for date pickers
            StartDatePicker.MinDate = DateTime.Now.Date;
            EndDatePicker.MinDate = DateTime.Now.Date;

            // Handle date validation
            StartDatePicker.ValueChanged += DatePicker_ValueChanged;
            EndDatePicker.ValueChanged += DatePicker_ValueChanged;
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
                        StartDatePicker.Value = TimeZoneInfo.ConvertTimeFromUtc(startUtc, TimeZoneInfo.Local);
                        EndDatePicker.Value = TimeZoneInfo.ConvertTimeFromUtc(endUtc, TimeZoneInfo.Local);

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

        private void RefreshAppointmentsGrid()
        {
            var appointments = dbHelper.GetAllAppointments();
            if (appointments != null)
            {
                AppointmentsDataGridView.DataSource = appointments;
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

            //TODO!! CHECK THIS 
            //old
            //if (dbHelper.HasOverlappingAppointments(
            //    Convert.ToInt32(CustomerComboBox.SelectedValue),
            //    startUtc,
            //    endUtc,
            //    currentAppointmentId))
            //    return (false, "This time slot overlaps with another appointment.");

            //new
            DataTable existingAppointments = dbHelper.GetAllAppointments();
            if (!AppointmentHelper.HasOverlappingAppointments(StartDatePicker.Value, EndDatePicker.Value, existingAppointments))
            {
                MessageBox.Show(
                   "This appointment overlaps with an existing appointment.\n" +
                   "Please choose a different time.",
                   "Validation Error",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                return(false, "Check if condition for hasOverlappingAppt, inside of FUNC: ValidateAppointmentInput file: EditAppointment ");

            }

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