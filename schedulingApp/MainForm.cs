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
    public partial class MainForm : Form
    {
        private readonly DatabaseHelper dbHelper;
        private DateTime currentMonth = DateTime.Now;
        private DateTime? selectedDate = null;

        private Dictionary<DateTime, List<string>> appointments; //old

        public MainForm()
        {

            InitializeComponent();
            dbHelper = new DatabaseHelper();
            StartComp();
            LoadAppointments();
            DisplayCalendar();
            ConfigureAppointmentList();
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.DoubleBuffered = true;  // Enable double buffering for the entire form
        }

        private void LoadAppointments()
        {
            //appointments = new Dictionary<DateTime, List<string>>
            //{
            //    { new DateTime(DateTime.Now.Year, DateTime.Now.Month, 5),
            //        new List<string> { "John Doe at 10:00 AM", "Jane Smith at 2:00 PM" } },
            //    { new DateTime(DateTime.Now.Year, DateTime.Now.Month, 18),
            //        new List<string> { "Client Meeting at 11:00 AM" } }
            //};
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
                string appointmentInfo = $"{row["customerName"]} - {Convert.ToDateTime(row["start"]).ToString("h:mm tt")}" +
                                       $"\n{row["type"]}: {row["description"]}";
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
            // Enable custom drawing for the appointment list
            appointmentList.DrawMode = DrawMode.OwnerDrawFixed;
            appointmentList.ItemHeight = 50; // Set height of each item
            appointmentList.DrawItem += AppointmentList_DrawItem;
        }
        // Load sample appointments (you can load from a database here)

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

            // Draw the appointment text with margin and bold font if selected
            e.Graphics.DrawString(appointmentText, appointmentFont, new SolidBrush(lb.ForeColor),
                                  new PointF(e.Bounds.X + marginLeft, e.Bounds.Y + marginTop));

            // Draw top and bottom border for the item
            Pen borderPen = new Pen(Color.Gray, 1);
            e.Graphics.DrawLine(borderPen, e.Bounds.Left, e.Bounds.Top, e.Bounds.Right, e.Bounds.Top);
            e.Graphics.DrawLine(borderPen, e.Bounds.Left, e.Bounds.Bottom - 1, e.Bounds.Right, e.Bounds.Bottom - 1);

            // Ensure item is correctly drawn
            e.DrawFocusRectangle();
        }

        private void appointmentList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        // Display the calendar for the current month
        private void DisplayCalendar()
        {
            calendarPanel.SuspendLayout();
            calendarPanel.Controls.Clear();

            DateTime firstDayOfMonth = new DateTime(currentMonth.Year, currentMonth.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(currentMonth.Year, currentMonth.Month);

            labelMonth.Text = currentMonth.ToString("MMMM yyyy");

            // Add day labels
            string[] dayNames = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            for (int i = 0; i < 7; i++)
            {
                Label dayLabel = new Label
                {
                    Text = dayNames[i],
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new Font("Arial", 9, FontStyle.Bold),
                    ForeColor = Color.White
                };
                calendarPanel.Controls.Add(dayLabel, i, 0);
            }

            // Get appointment counts for the month
            var appointmentCounts = dbHelper.GetMonthlyAppointmentCounts(currentMonth);

            int dayOffset = (int)firstDayOfMonth.DayOfWeek;

            // Create buttons for each day
            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime date = new DateTime(currentMonth.Year, currentMonth.Month, day);
                Button dayButton = new Button
                {
                    Text = day.ToString(),
                    Dock = DockStyle.Fill,
                    Tag = date,
                    FlatStyle = FlatStyle.Flat
                };

                // Style the button based on appointments
                if (appointmentCounts.ContainsKey(date))
                {
                    dayButton.BackColor = Color.LightGreen;
                    dayButton.Text = $"{day}\n({appointmentCounts[date]})";
                }
                else
                {
                    dayButton.BackColor = Color.White;
                }

                // Highlight selected date
                if (selectedDate.HasValue && date.Date == selectedDate.Value.Date)
                {
                    dayButton.BackColor = Color.Yellow;
                }

                dayButton.Click += (sender, e) => OnDaySelected(date);
                calendarPanel.Controls.Add(dayButton, (dayOffset + day - 1) % 7, (dayOffset + day - 1) / 7 + 1);
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
            DisplayCalendar();
        }

        private void bttnNextMonth_Click(object sender, EventArgs e)
        {
            currentMonth = currentMonth.AddMonths(1);
            DisplayCalendar();
        }
        public class DoubleBufferedTableLayoutPanel : TableLayoutPanel
        {
            public DoubleBufferedTableLayoutPanel()
            {
                this.DoubleBuffered = true;
            }
        }

        private void bttnNewCustomer_Click(object sender, EventArgs e)
        {
            NewCustomerForm newCustomerForm = new NewCustomerForm();
            newCustomerForm.Show();
            this.Hide();
        }
    }
}
