﻿using MySqlConnector;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace schedulingApp
{
    public partial class MainForm : Form
    {
        private readonly DatabaseHelper dbHelper;
        //private DateTime currentMonth = DateTime.Now;
        private DateTime currentMonth = new DateTime(2019, 1, 1);
        private DateTime? selectedDate = null;

        private Dictionary<DateTime, List<string>> appointments; //old


        private Button bttnReports;

        private void InitializeReportsButton()
        {
            bttnReports = new Button
            {
                BackColor = Color.RoyalBlue,
                Cursor = Cursors.Hand,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Noto Sans", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0),
                ForeColor = Color.White,
                Location = new Point(bttnLogout.Left - 160, bttnLogout.Top), // Position it next to Logout
                Name = "bttnReports",
                Size = new Size(140, 30),
                Text = "Reports",
                UseVisualStyleBackColor = false
            };

            // Add it to the form
            this.Controls.Add(bttnReports);

            // Add the click event handler
            bttnReports.Click += new EventHandler(bttnReports_Click);
        }
        public MainForm()
        {

            InitializeComponent();
            InitializeReportsButton();
            dbHelper = new DatabaseHelper();
            StartComp();
            LoadAppointments();
            DisplayCalendar();
            ConfigureAppointmentList();
        }

        private void bttnReports_Click(object sender, EventArgs e)
        {

            ContextMenuStrip reportsMenu = new ContextMenuStrip();
            reportsMenu.Items.Add("Appointment Types by Month", null, (s, args) =>
            {
                var appointments = GetAppointmentsForReports();
                if (appointments != null)
                    GenerateAppointmentTypesByMonth(appointments);
            });

            reportsMenu.Items.Add("User Schedules", null, (s, args) =>
            {
                var appointments = GetAppointmentsForReports();
                if (appointments != null)
                    GenerateUserSchedules(appointments);
            });

            reportsMenu.Items.Add("Location Analysis", null, (s, args) =>
            {
                var appointments = GetAppointmentsForReports();
                if (appointments != null)
                    GenerateCustomerLocationReport(appointments);
            });

            reportsMenu.Items.Add(new ToolStripSeparator());

            reportsMenu.Items.Add("Generate All Reports", null, (s, args) =>
            {
                GenerateReports();
            });

            // Show menu
            reportsMenu.Show(bttnReports, new Point(0, bttnReports.Height));
        }

        private IEnumerable<dynamic> GetAppointmentsForReports()
        {
            var appointments = dbHelper.GetAllAppointments();
            if (appointments == null || appointments.Rows.Count == 0)
            {
                MessageBox.Show("No appointments found to generate reports.",
                              "No Data",
                              MessageBoxButtons.OK,
                              MessageBoxIcon.Information);
                return null;
            }

            // !!Check: Convert DataTable to list of appointments 
            return appointments.AsEnumerable()
                .Select(row => new
                {
                    Type = row.Field<string>("type"),
                    Start = row.Field<DateTime>("start"),
                    CustomerName = row.Field<string>("customerName"),
                    UserId = row.Field<int>("userId"),
                    Title = row.Field<string>("title"),
                    Description = row.Field<string>("description"),
                    Location = row.Field<string>("location")
                })
                .ToList();
        }


        private void ExportReport(string reportContent, string suggestedFileName)
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 1,
                FileName = suggestedFileName
            };

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    File.WriteAllText(saveDialog.FileName, reportContent);
                    MessageBox.Show("Report exported successfully!",
                                  "Export Success",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting report: {ex.Message}",
                                  "Export Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
        }

        // ShowReportDialog method include export option
        private void ShowReportDialog(string title, string report)
        {
            Form reportForm = new Form
            {
                Text = title,
                Size = new Size(600, 500),
                StartPosition = FormStartPosition.CenterParent
            };

            TextBox reportText = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Font = new Font("Consolas", 10),
                Text = report
            };

            Button exportButton = new Button
            {
                Text = "Export",
                Dock = DockStyle.Bottom,
                Height = 30
            };

            exportButton.Click += (s, e) => ExportReport(report, $"{title.Replace(" ", "_")}.txt");

            reportForm.Controls.Add(reportText);
            reportForm.Controls.Add(exportButton);
            reportForm.ShowDialog();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.DoubleBuffered = true;  // Enable double buffering for the entire form
        }

        private void LoadAppointments()
        {
            UpdateAppointmentList();
        }

        private void UpdateAppointmentList()
        {
            appointmentList.Items.Clear();

            DataTable appointments;
            if (selectedDate.HasValue)
            {
                appointments = dbHelper.GetAppointmentsByDate(selectedDate.Value);
            }
            else
            {
                appointments = dbHelper.GetAllAppointments();
            }

            if (appointments == null || appointments.Rows.Count == 0)
            {
                appointmentList.Items.Add("No appointments registered yet. Consider creating one");
                return;
            }

            foreach (DataRow row in appointments.Rows)
            {
                    string appointmentInfo =
                $"Customer: {row["customerName"]}\n" +
                $"Start: {Convert.ToDateTime(row["start"]).ToString("MM/dd/yyyy h:mm tt")}\n" +
                $"End: {Convert.ToDateTime(row["end"]).ToString("MM/dd/yyyy h:mm tt")}\n" +
                $"Type: {row["type"]}\n" +
                $"Description: {row["description"]}";

                appointmentList.Items.Add(appointmentInfo);
            }
        }

        //[STAThread]
        private void StartComp()
        {
            bttnPreviosMonth.Click += bttnPreviosMonth_Click;
            bttnNextMonth.Click += bttnNextMonth_Click;
            this.Text = "Scheduling App";
        }
        private void ConfigureAppointmentList()
        {
            // Keep your existing configuration
            appointmentList.DrawMode = DrawMode.OwnerDrawFixed;
            appointmentList.ItemHeight = 100;
            appointmentList.DrawItem += AppointmentList_DrawItem;

            // Add double-click handler
            appointmentList.DoubleClick += AppointmentList_DoubleClick;
        }

        private void AppointmentList_DoubleClick(object sender, EventArgs e)
        {
            if (appointmentList.SelectedIndex == -1) return;

            DataTable appointments;
            if (selectedDate.HasValue)
            {
                appointments = dbHelper.GetAppointmentsByDate(selectedDate.Value);
            }
            else
            {
                appointments = dbHelper.GetAllAppointments();
            }

            if (appointments == null || appointments.Rows.Count == 0 ||
                appointmentList.SelectedIndex >= appointments.Rows.Count)
            {
                MessageBox.Show("Error retrieving appointment details.",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
                return;
            }

            // Get the appointment ID from the selected row
            int appointmentId = Convert.ToInt32(appointments.Rows[appointmentList.SelectedIndex]["appointmentId"]);

            // Open the edit form
            EditAppointments editForm = new EditAppointments(appointmentId);
            editForm.FormClosed += (s, args) =>
            {
                this.Show();
                // Refresh the calendar and appointment list
                DisplayCalendar();
                UpdateAppointmentList();
            };
            editForm.Show();
            this.Hide();
        }


        // Custom rendering for appointment items
        private void AppointmentList_DrawItem(object sender, DrawItemEventArgs e)
        {
            // Ensure there are items to draw
            if (e.Index < 0) return;

            ListBox lb = (ListBox)sender;
            string appointmentText = lb.Items[e.Index].ToString();

            // Set background color for selected item
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.Yellow), e.Bounds);
            }
            else
            {
                e.Graphics.FillRectangle(new SolidBrush(lb.BackColor), e.Bounds);
            }

            // Create font for bold text when selected
            Font appointmentFont = (e.State & DrawItemState.Selected) == DrawItemState.Selected ?
                                   new Font(e.Font, FontStyle.Bold) :
                                   e.Font;

            // Set margins (left: 20px, top: 10px)
            int marginLeft = 20;
            int marginTop = 10;
            
            //New
            using (StringFormat sf = new StringFormat())
            {
                sf.LineAlignment = StringAlignment.Near;
                e.Graphics.DrawString(appointmentText, appointmentFont, new SolidBrush(lb.ForeColor),
                                    new RectangleF(e.Bounds.X + marginLeft, e.Bounds.Y + marginTop,
                                                 e.Bounds.Width - marginLeft * 2, e.Bounds.Height - marginTop * 2),
                                    sf);
            }

            // Draw borders
            using (Pen borderPen = new Pen(Color.Gray, 1))
            {
                e.Graphics.DrawLine(borderPen, e.Bounds.Left, e.Bounds.Top, e.Bounds.Right, e.Bounds.Top);
                e.Graphics.DrawLine(borderPen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);
            }

            e.DrawFocusRectangle();

            //// Draw the appointment text with margin and bold font if selected
            //e.Graphics.DrawString(appointmentText, appointmentFont, new SolidBrush(lb.ForeColor),
            //                      new PointF(e.Bounds.X + marginLeft, e.Bounds.Y + marginTop));

            //// Draw top and bottom border for the item
            //Pen borderPen = new Pen(Color.Gray, 1);
            //e.Graphics.DrawLine(borderPen, e.Bounds.Left, e.Bounds.Top, e.Bounds.Right, e.Bounds.Top);
            //e.Graphics.DrawLine(borderPen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);

            //// Ensure item is correctly drawn
            //e.DrawFocusRectangle();
        }

        private void appointmentList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        // Display the calendar for the current month
        private void DisplayCalendar()
        {
            calendarPanel.SuspendLayout();
            calendarPanel.Controls.Clear();

            // Initialize calendar panel rows and columns if not already done
            if (calendarPanel.ColumnCount != 7 || calendarPanel.RowCount != 7)
            {
                calendarPanel.ColumnCount = 7;
                calendarPanel.RowCount = 7;

                // Set column styles
                calendarPanel.ColumnStyles.Clear();
                for (int i = 0; i < 7; i++)
                {
                    calendarPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F / 7F));
                }

                // Set row styles
                calendarPanel.RowStyles.Clear();
                for (int i = 0; i < 7; i++)
                {
                    calendarPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F / 7F));
                }
            }

            // Update month label
            labelMonth.Text = currentMonth.ToString("MMMM yyyy");

            // Add day headers
            string[] dayNames = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            for (int i = 0; i < 7; i++)
            {
                Label dayLabel = new Label
                {
                    Text = dayNames[i],
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new Font("Arial", 9, FontStyle.Bold),
                    ForeColor = Color.Black
                };
                calendarPanel.Controls.Add(dayLabel, i, 0);
            }

            // Get first day and number of days in month
            DateTime firstDay = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
            int dayOfWeek = (int)firstDay.DayOfWeek;

            // Get appointment counts
            var appointmentCounts = dbHelper.GetMonthlyAppointmentCounts(currentMonth);

            // Fill in the calendar
            int currentDay = 1;
            for (int row = 1; row < 7; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (row == 1 && col < dayOfWeek)
                    {
                        // Empty cells before first day of month
                        continue;
                    }

                    if (currentDay > daysInMonth)
                    {
                        // Empty cells after last day of month
                        continue;
                    }

                    DateTime currentDate = new DateTime(currentMonth.Year, currentMonth.Month, currentDay);
                    Button dayButton = new Button
                    {
                        Text = currentDay.ToString(),
                        Tag = currentDate,
                        Dock = DockStyle.Fill,
                        FlatStyle = FlatStyle.Flat,
                        Margin = new Padding(2),
                        Font = new Font("Arial", 9F)
                    };

                    // Style based on appointments
                    if (appointmentCounts.ContainsKey(currentDate))
                    {
                        dayButton.BackColor = Color.LightGreen;
                        dayButton.Text = $"{currentDay}\n({appointmentCounts[currentDate]})";
                    }
                    else
                    {
                        dayButton.BackColor = Color.White;
                    }

                    // Highlight selected date
                    if (selectedDate.HasValue && currentDate.Date == selectedDate.Value.Date)
                    {
                        dayButton.BackColor = Color.Yellow;
                    }

                    // Add click handler
                    dayButton.Click += (sender, e) => OnDaySelected(currentDate);

                    // Add button to calendar
                    calendarPanel.Controls.Add(dayButton, col, row);
                    currentDay++;
                }
            }

            calendarPanel.ResumeLayout();
        }
        // Handle when a day is clicked
        private void OnDaySelected(DateTime date)
        {
            if (selectedDate.HasValue && selectedDate.Value.Date == date.Date)
            {
                // Deselect the date if it's already selected
                selectedDate = null;
            }
            else
            {
                selectedDate = date;
            }

            DisplayCalendar(); // Refresh calendar to show selection
            UpdateAppointmentList(); // Update appointment list for selected date
        }

        private void bttnPreviosMonth_Click(object sender, EventArgs e)
        {
            currentMonth = currentMonth.AddMonths(-1);
            selectedDate = null; // Reset selected date when changing months
            DisplayCalendar();
            UpdateAppointmentList();
        }
        private void bttnNextMonth_Click(object sender, EventArgs e)
        {
            currentMonth = currentMonth.AddMonths(1);
            selectedDate = null; // Reset selected date when changing months
            DisplayCalendar();
            UpdateAppointmentList();
        }
        public class DoubleBufferedTableLayoutPanel : TableLayoutPanel
        {
            public DoubleBufferedTableLayoutPanel()
            {
                this.DoubleBuffered = true;
            }
        }



        private void GenerateReports()
        {
            var appointments = dbHelper.GetAllAppointments();
            if (appointments == null || appointments.Rows.Count == 0)
            {
                MessageBox.Show("No appointments found to generate reports.");
                return;
            }

            // Convert DataTable to list of appointments for easier manipulation
            var appointmentList = appointments.AsEnumerable()
                .Select(row => new
                {
                    Type = row.Field<string>("type"),
                    Start = row.Field<DateTime>("start"),
                    CustomerName = row.Field<string>("customerName"),
                    UserId = row.Field<int>("userId"),
                    Title = row.Field<string>("title"),
                    Description = row.Field<string>("description"),
                    Location = row.Field<string>("location")
                }).ToList();

            // Generate reports
            GenerateAppointmentTypesByMonth(appointmentList);
            GenerateUserSchedules(appointmentList);
            GenerateCustomerLocationReport(appointmentList); // Additional report
        }

        private void GenerateAppointmentTypesByMonth(IEnumerable<dynamic> appointments)
        {
            try
            {
                // Group appointments by month and type using lambda
                var report = appointments
                    .GroupBy(a => new { Month = a.Start.ToString("MMMM yyyy"), Type = a.Type })
                    .Select(g => new
                    {
                        Month = g.Key.Month,
                        Type = g.Key.Type,
                        Count = g.Count()
                    })
                    .OrderBy(x => DateTime.ParseExact(x.Month, "MMMM yyyy", null))
                    .ThenBy(x => x.Type)
                    .ToList();

                // Create report string
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Appointment Types by Month Report\n");

                string currentMonth = "";
                foreach (var item in report)
                {
                    if (currentMonth != item.Month)
                    {
                        currentMonth = item.Month;
                        sb.AppendLine($"\n{currentMonth}:");
                    }
                    sb.AppendLine($"  {item.Type}: {item.Count} appointments");
                }

                // Show report
                ShowReportDialog("Appointment Types by Month", sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating appointment types report: {ex.Message}");
            }
        }

        private void GenerateUserSchedules(IEnumerable<dynamic> appointments)
        {
            try
            {
                // Group appointments by user and order by date using lambda
                var userSchedules = appointments
                    .GroupBy(a => a.UserId)
                    .Select(g => new
                    {
                        UserId = g.Key,
                        Appointments = g.OrderBy(a => a.Start)
                            .Select(a => new
                            {
                                Date = a.Start.ToString("MM/dd/yyyy"),
                                Time = a.Start.ToString("HH:mm"),
                                Customer = a.CustomerName,
                                Title = a.Title,
                                Type = a.Type
                            })
                            .ToList()
                    })
                    .ToList();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("User Schedules Report\n");

                foreach (var userSchedule in userSchedules)
                {
                    sb.AppendLine($"User ID: {userSchedule.UserId}");
                    sb.AppendLine("----------------------------------------");

                    string currentDate = "";
                    foreach (var appt in userSchedule.Appointments)
                    {
                        if (currentDate != appt.Date)
                        {
                            currentDate = appt.Date;
                            sb.AppendLine($"\n{currentDate}:");
                        }
                        sb.AppendLine($"  {appt.Time} - {appt.Customer} ({appt.Type})");
                    }
                    sb.AppendLine("\n");
                }

                ShowReportDialog("User Schedules", sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating user schedules report: {ex.Message}");
            }
        }

        // Additional report: Customer appointments by location with statistics
        private void GenerateCustomerLocationReport(IEnumerable<dynamic> appointments)
        {
            try
            {
                // Group appointments by location using lambda
                var locationStats = appointments
                    .GroupBy(a => a.Location)
                    .Select(g => new
                    {
                        Location = string.IsNullOrEmpty(g.Key) ? "No Location" : g.Key,
                        TotalAppointments = g.Count(),
                        UniqueCustomers = g.Select(a => a.CustomerName).Distinct().Count(),
                        AverageAppointmentsPerMonth = g.GroupBy(a => new { a.Start.Year, a.Start.Month })
                            .Average(m => m.Count()),
                        MostCommonType = g.GroupBy(a => a.Type)
                            .OrderByDescending(t => t.Count())
                            .First().Key
                    })
                    .OrderByDescending(l => l.TotalAppointments)
                    .ToList();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Location Analysis Report\n");
                sb.AppendLine("Location Statistics Summary");
                sb.AppendLine("----------------------------------------");

                foreach (var location in locationStats)
                {
                    sb.AppendLine($"\nLocation: {location.Location}");
                    sb.AppendLine($"Total Appointments: {location.TotalAppointments}");
                    sb.AppendLine($"Unique Customers: {location.UniqueCustomers}");
                    sb.AppendLine($"Average Appointments/Month: {location.AverageAppointmentsPerMonth:F1}");
                    sb.AppendLine($"Most Common Appointment Type: {location.MostCommonType}");
                }

                ShowReportDialog("Location Analysis Report", sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating location report: {ex.Message}");
            }
        }

        private void bttnNewCustomer_Click(object sender, EventArgs e)
        {
            NewCustomerForm newCustomerForm = new NewCustomerForm();
            newCustomerForm.Show();
            this.Hide();
        }

        private void bttnEditCustomer_Click(object sender, EventArgs e)
        {
            EditCustomerForm editCustomerForm = new EditCustomerForm();
            editCustomerForm.FormClosed += (s, args) =>
            {
                this.Show();
                // Refresh the calendar and appointment list in case customer details affect appointments
                DisplayCalendar();
                UpdateAppointmentList();
            };
            editCustomerForm.Show();
            this.Hide();
        }

        private void bttnNewAppt_Click(object sender, EventArgs e)
        {
            NewAppointment newAppointmentForm = new NewAppointment();
            newAppointmentForm.FormClosed += (s, args) =>
            {
                this.Show();
                // Refresh the calendar and appointment list
                DisplayCalendar();
                UpdateAppointmentList();
            };
            newAppointmentForm.Show();
            this.Hide();
        }

        private void bttnCancelAppt_Click(object sender, EventArgs e)
        {
            // Check if an appointment is selected
            if (appointmentList.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an appointment to cancel.",
                               "No Selection",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);
                return;
            }

            // Get the selected appointment's details
            DataTable appointments;
            if (selectedDate.HasValue)
            {
                appointments = dbHelper.GetAppointmentsByDate(selectedDate.Value);
            }
            else
            {
                appointments = dbHelper.GetAllAppointments();
            }

            if (appointments == null || appointments.Rows.Count == 0 ||
                appointmentList.SelectedIndex >= appointments.Rows.Count)
            {
                MessageBox.Show("Error retrieving appointment details.",
                               "Error",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
                return;
            }

            // Get the appointment ID from the selected row
            int appointmentId = Convert.ToInt32(appointments.Rows[appointmentList.SelectedIndex]["appointmentId"]);

            // Confirm deletion with user
            var result = MessageBox.Show(
                "Are you sure you want to cancel this appointment?",
                "Confirm Cancellation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Call the database helper method to delete the appointment
                var (success, message) = dbHelper.DeleteAppointment(appointmentId);

                if (success)
                {
                    MessageBox.Show("Appointment cancelled successfully.",
                                  "Success",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);

                    // Refresh the calendar and appointment list
                    DisplayCalendar();
                    UpdateAppointmentList();
                }
                else
                {
                    MessageBox.Show(message,
                                  "Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
        }

        private void bttnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to log out?",
                "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                LoginForm loginForm = new LoginForm();
                loginForm.Show();

                this.Close();
            }
        }
    }
}
