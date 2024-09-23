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
        private DateTime currentMonth = DateTime.Now;
        private Dictionary<DateTime, List<string>> appointments;

        public MainForm()
        {
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new MainForm());
            InitializeComponent();
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
        private void LoadAppointments()
        {
            appointments = new Dictionary<DateTime, List<string>>
            {
                { new DateTime(DateTime.Now.Year, DateTime.Now.Month, 5),
                    new List<string> { "John Doe at 10:00 AM", "Jane Smith at 2:00 PM" } },
                { new DateTime(DateTime.Now.Year, DateTime.Now.Month, 18),
                    new List<string> { "Client Meeting at 11:00 AM" } }
            };
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

            labelMonth.Text = currentMonth.ToString("MMM yyyy");

            // Add day labels (Sun, Mon, ...)
            for (int i = 0; i < 7; i++)
            {
                calendarPanel.Controls.Add(new Label
                {
                    Text = Enum.GetName(typeof(DayOfWeek), i).Substring(0, 3),
                    TextAlign = ContentAlignment.MiddleCenter,
                    AutoSize = true
                });
            }

            // Get the day of the week the month starts on
            int dayOffset = (int)firstDayOfMonth.DayOfWeek;

            // Create buttons for each day of the month
            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime date = new DateTime(currentMonth.Year, currentMonth.Month, day);
                Button dayButton = new Button
                {
                    Text = day.ToString(),
                    BackColor = appointments.ContainsKey(date) ? Color.LightGreen : Color.White, // Highlight days with appointments
                    Dock = DockStyle.Fill
                };

                dayButton.Click += (sender, e) => OnDaySelected(date);
                calendarPanel.Controls.Add(dayButton, (dayOffset + day - 1) % 7, (dayOffset + day - 1) / 7 + 1);
            }
            // Resume layout to finalize changes and prevent flicker
            calendarPanel.ResumeLayout();
        }
        // Handle when a day is clicked
        private void OnDaySelected(DateTime date)
        {
            appointmentList.Items.Clear();

            if (appointments.ContainsKey(date))
            {
                appointmentList.Items.AddRange(appointments[date].ToArray());
            }
            else
            {
                appointmentList.Items.Add("There are no appointments for this day.");
            }
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

    }
}
