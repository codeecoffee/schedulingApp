using MySqlConnector;
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
        private DateTime currentMonth;  // Remove the initialization here
        private DateTime? selectedDate = null;
        private Button bttnReports;
        private NewCustomerForm currentNewCustomerForm;

        private readonly DataGridView calendarGrid;
        private readonly Button prevMonthBtn;
        private readonly Button nextMonthBtn;


        public MainForm(string username)
        {
            InitializeComponent();
            InitializeReportsButton();
            dbHelper = new DatabaseHelper();

            // Remove existing calendar panel
            this.Controls.Remove(calendarPanel);

            // Create and configure DataGridView
            calendarGrid = new DataGridView
            {
                Location = new Point(636, 151),
                Size = new Size(583, 475),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = false,
                RowHeadersVisible = false,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 40,
                RowTemplate = new DataGridViewRow { Height = 80 }, // Increased row height
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Color.RoyalBlue,
                    ForeColor = Color.White,
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Arial", 9, FontStyle.Regular),
                    Padding = new Padding(3),
                    WrapMode = DataGridViewTriState.True
                },
                SelectionMode = DataGridViewSelectionMode.CellSelect,
                MultiSelect = false,
                EnableHeadersVisualStyles = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None
            };

            // Configure grid columns
            string[] days = { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            foreach (string day in days)
            {
                DataGridViewColumn column = new DataGridViewTextBoxColumn
                {
                    Name = day,
                    HeaderText = day,
                    Width = calendarGrid.Width / 7,
                    DefaultCellStyle = new DataGridViewCellStyle
                    {
                        WrapMode = DataGridViewTriState.True
                    }
                };
                calendarGrid.Columns.Add(column);
            }

            // Add event handlers
            calendarGrid.CellClick += CalendarGrid_CellClick;
            calendarGrid.CellFormatting += CalendarGrid_CellFormatting;

            // Update the button click handlers
            bttnPreviosMonth.Click -= bttnPreviosMonth_Click;  // Remove old handler if exists
            bttnNextMonth.Click -= bttnNextMonth_Click;        // Remove old handler if exists

            bttnPreviosMonth.Click += (s, e) =>
            {
                currentMonth = currentMonth.AddMonths(-1);
                DisplayCalendarGrid();
            };

            bttnNextMonth.Click += (s, e) =>
            {
                currentMonth = currentMonth.AddMonths(1);
                DisplayCalendarGrid();
            };

            // Add to form
            this.Controls.Add(calendarGrid);

            // Initialize current month
            currentMonth = DateTime.Today;

            //StartComp();
            LoadAppointments();
            DisplayCalendarGrid();
            ConfigureAppointmentList();
        }


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

        private void SetupCalendarGrid()
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

        private void AddDayHeaders()
        {
            string[] dayNames = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            for (int i = 0; i < 7; i++)
            {
                Label dayLabel = new Label
                {
                    Text = dayNames[i],
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new Font("Arial", 9, FontStyle.Bold),
                    ForeColor = Color.Black,
                    BackColor = Color.LightGray
                };
                calendarPanel.Controls.Add(dayLabel, i, 0);
            }
        }

        //private void OnDaySelected(DateTime date)
        //{
        //    if (selectedDate.HasValue && selectedDate.Value.Date == date.Date)
        //    {
        //        selectedDate = null;
        //    }
        //    else
        //    {
        //        selectedDate = date;
        //    }

        //    DisplayCalendar(); // Refresh calendar to show selection
        //    UpdateAppointmentList(); // Update appointment list for selected date
        //}

        //new calendar 
        private void DisplayCalendarGrid()
        {
            try
            {
                calendarGrid.Rows.Clear();
                labelMonth.Text = currentMonth.ToString("MMMM yyyy");

                DateTime firstDay = new DateTime(currentMonth.Year, currentMonth.Month, 1);
                int daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);
                int startingDay = (int)firstDay.DayOfWeek;

                // Get appointment data
                var appointmentCounts = dbHelper.GetMonthlyAppointmentCounts(currentMonth);

                // Calculate needed rows
                int totalDays = startingDay + daysInMonth;
                int numberOfRows = (int)Math.Ceiling(totalDays / 7.0);

                // Add rows to grid
                calendarGrid.Rows.Add(numberOfRows);

                // Set the height for all rows
                foreach (DataGridViewRow row in calendarGrid.Rows)
                {
                    row.Height = 80; // Adjust this value as needed
                }

                int currentDay = 1;
                bool monthStarted = false;

                // Fill the calendar
                for (int row = 0; row < numberOfRows; row++)
                {
                    for (int col = 0; col < 7; col++)
                    {
                        if (!monthStarted && col == startingDay)
                        {
                            monthStarted = true;
                        }

                        var cell = calendarGrid.Rows[row].Cells[col];
                        cell.Style.BackColor = Color.White;
                        cell.Style.SelectionBackColor = Color.LightBlue;

                        if (monthStarted && currentDay <= daysInMonth)
                        {
                            DateTime currentDate = new DateTime(currentMonth.Year, currentMonth.Month, currentDay);
                            string cellText = currentDay.ToString();

                            if (appointmentCounts.ContainsKey(currentDate))
                            {
                                cellText += $"\n({appointmentCounts[currentDate]} appt)";
                                cell.Style.BackColor = Color.LightGreen;
                            }

                            cell.Value = cellText;
                            cell.Tag = currentDate;

                            if (selectedDate.HasValue && currentDate.Date == selectedDate.Value.Date)
                            {
                                cell.Style.BackColor = Color.Yellow;
                                cell.Style.SelectionBackColor = Color.Gold;
                            }

                            currentDay++;
                        }
                        else
                        {
                            cell.Value = "";
                            cell.Tag = null;
                            cell.Style.BackColor = Color.WhiteSmoke;
                        }
                    }
                }

                // Adjust row heights to fill container
                int availableHeight = calendarGrid.Height - calendarGrid.ColumnHeadersHeight;
                int rowHeight = availableHeight / numberOfRows;
                foreach (DataGridViewRow row in calendarGrid.Rows)
                {
                    row.Height = rowHeight;
                }

                calendarGrid.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying calendar: {ex.Message}");
            }
        }

        private void CalendarGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = calendarGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (cell.Tag is DateTime date)
                {
                    // If clicking on already selected date, deselect it
                    if (selectedDate.HasValue && selectedDate.Value.Date == date.Date)
                    {
                        selectedDate = null;
                    }
                    else
                    {
                        selectedDate = date;
                    }

                    // Always update the appointment list and refresh calendar
                    UpdateAppointmentList();
                    DisplayCalendarGrid();
                }
            }
        }

        private void CalendarGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = calendarGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];

                // Set default style
                e.CellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
                e.CellStyle.Padding = new Padding(5);

                if (cell.Tag is DateTime date)
                {
                    // Format based on selection and appointments
                    if (selectedDate.HasValue && date.Date == selectedDate.Value.Date)
                    {
                        e.CellStyle.BackColor = Color.Yellow;
                        e.CellStyle.SelectionBackColor = Color.Gold;
                    }
                    else if (cell.Value.ToString().Contains("appointments"))
                    {
                        e.CellStyle.BackColor = Color.LightGreen;
                        e.CellStyle.SelectionBackColor = Color.PaleGreen;
                    }

                    // Today's date
                    if (date.Date == DateTime.Today)
                    {
                        e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                    }
                }
            }
        }

        private void bttnPreviosMonth_Click(object sender, EventArgs e)
        {
            try
            {
                currentMonth = currentMonth.AddMonths(-1);
                selectedDate = null;
                DisplayCalendarGrid();
                UpdateAppointmentList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to previous month: {ex.Message}");
            }
        }

        private void bttnNextMonth_Click(object sender, EventArgs e)
        {
            try
            {
                currentMonth = currentMonth.AddMonths(1);
                selectedDate = null;
                DisplayCalendarGrid();
                UpdateAppointmentList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error navigating to next month: {ex.Message}");
            }
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

            // Add the new report option
            reportsMenu.Items.Add("Duration Analysis", null, (s, args) =>
            {
                var appointments = GetAppointmentsForReports();
                if (appointments != null)
                    GenerateAppointmentDurationReport(appointments);
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

            return appointments.AsEnumerable()
                .Select(row => new
                {
                    Type = row.Field<string>("type"),
                    Start = row.Field<DateTime>("start"),
                    End = row.Field<DateTime>("end"),  // Add this line
                    CustomerName = row.Field<string>("customerName"),
                    UserId = row.Field<int>("userId"),
                    Title = row.Field<string>("title"),
                    Description = row.Field<string>("description"),
                    Location = row.Field<string>("location")
                })
                .ToList();
        }

        private void GenerateAppointmentDurationReport(IEnumerable<dynamic> appointments)
        {
            try
            {
                var report = appointments
                    .Select(a => new
                    {
                        Type = (string)a.Type,
                        StartTime = (DateTime)a.Start,
                        EndTime = (DateTime)a.End,
                        Customer = (string)a.CustomerName,
                        DurationInMinutes = (int)((DateTime)a.End - (DateTime)a.Start).TotalMinutes
                    })
                    .GroupBy(a => a.Type)
                    .Select(g => new
                    {
                        AppointmentType = g.Key,
                        Count = g.Count(),
                        TotalDuration = g.Sum(x => x.DurationInMinutes),
                        AverageDuration = (int)g.Average(x => x.DurationInMinutes)
                    })
                    .OrderByDescending(x => x.Count)
                    .ToList();

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Appointment Duration Analysis Report\n");
                sb.AppendLine("----------------------------------------\n");

                foreach (var typeStats in report)
                {
                    sb.AppendLine($"Type: {typeStats.AppointmentType}");
                    sb.AppendLine($"Total Appointments: {typeStats.Count}");
                    sb.AppendLine($"Total Duration: {typeStats.TotalDuration} minutes");
                    sb.AppendLine($"Average Duration: {typeStats.AverageDuration} minutes");
                    sb.AppendLine("----------------------------------------\n");
                }

                ShowReportDialog("Appointment Duration Analysis", sb.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating duration analysis report: {ex.Message}\n\nStack Trace: {ex.StackTrace}");
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

            var appointmentList = appointments.AsEnumerable()
                .Select(row => new
                {
                    Type = row.Field<string>("type"),
                    Start = row.Field<DateTime>("start"),
                    End = row.Field<DateTime>("end"),  // Add this line
                    CustomerName = row.Field<string>("customerName"),
                    UserId = row.Field<int>("userId"),
                    Title = row.Field<string>("title"),
                    Description = row.Field<string>("description"),
                    Location = row.Field<string>("location")
                }).ToList();

            GenerateAppointmentTypesByMonth(appointmentList);
            GenerateUserSchedules(appointmentList);
            GenerateCustomerLocationReport(appointmentList);
            GenerateAppointmentDurationReport(appointmentList);  // Add this line
        }



        //private void bttnReports_Click(object sender, EventArgs e)
        //{

        //    ContextMenuStrip reportsMenu = new ContextMenuStrip();
        //    reportsMenu.Items.Add("Appointment Types by Month", null, (s, args) =>
        //    {
        //        var appointments = GetAppointmentsForReports();
        //        if (appointments != null)
        //            GenerateAppointmentTypesByMonth(appointments);
        //    });

        //    reportsMenu.Items.Add("User Schedules", null, (s, args) =>
        //    {
        //        var appointments = GetAppointmentsForReports();
        //        if (appointments != null)
        //            GenerateUserSchedules(appointments);
        //    });

        //    reportsMenu.Items.Add("Location Analysis", null, (s, args) =>
        //    {
        //        var appointments = GetAppointmentsForReports();
        //        if (appointments != null)
        //            GenerateCustomerLocationReport(appointments);
        //    });

        //    reportsMenu.Items.Add(new ToolStripSeparator());

        //    reportsMenu.Items.Add("Generate All Reports", null, (s, args) =>
        //    {
        //        GenerateReports();
        //    });

        //    // Show menu
        //    reportsMenu.Show(bttnReports, new Point(0, bttnReports.Height));
        //}



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
            try
            {
                if (dbHelper == null)
                {
                    appointmentList.Items.Add("Database connection unavailable");
                    return;
                }
                UpdateAppointmentList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading appointments: {ex.Message}");
                appointmentList.Items.Add("Error loading appointments");
            }
        }

        private void UpdateAppointmentList()
        {
            try
            {
                appointmentList.Items.Clear();

                if (dbHelper == null)
                {
                    appointmentList.Items.Add("Database connection unavailable");
                    return;
                }

                DataTable appointments;
                try
                {
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
                        appointmentList.Items.Add("No appointments found");
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
                catch (Exception ex)
                {
                    appointmentList.Items.Add($"Error loading appointments: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating appointment list: {ex.Message}", "Update Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        //private void StartComp()
        //{
        //    bttnPreviosMonth.Click += bttnPreviosMonth_Click;
        //    bttnNextMonth.Click += bttnNextMonth_Click;
        //    this.Text = "Scheduling App";
        //}
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
                DisplayCalendarGrid();
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
        public class DoubleBufferedTableLayoutPanel : TableLayoutPanel
        {
            public DoubleBufferedTableLayoutPanel()
            {
                this.DoubleBuffered = true;
            }
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
            currentNewCustomerForm = new NewCustomerForm();
            currentNewCustomerForm.FormClosed += (s, args) =>
            {
                this.Show();
                currentNewCustomerForm = null;  
                DisplayCalendarGrid();
                UpdateAppointmentList();
            };
            currentNewCustomerForm.Show();
            this.Hide();

            //NewCustomerForm newCustomerForm = new NewCustomerForm();
            //newCustomerForm.FormClosed += (s, args) =>
            //{
            //    this.Show();
            //    // Refresh any necessary data
            //    DisplayCalendarGrid();
            //    UpdateAppointmentList();
            //};
            //newCustomerForm.Show();
            //this.Hide();
        }

        private void bttnEditCustomer_Click(object sender, EventArgs e)
        {
            EditCustomerForm editCustomerForm = new EditCustomerForm();
            editCustomerForm.FormClosed += (s, args) =>
            {
                this.Show();
                // Refresh the calendar and appointment list in case customer details affect appointments
                DisplayCalendarGrid();
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
                DisplayCalendarGrid();
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
                    DisplayCalendarGrid();
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
