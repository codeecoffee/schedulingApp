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
    public partial class LoginForm : Form
    {
        private readonly Helper helper;
        private readonly DatabaseHelper dbHelper;


        public LoginForm()
        {
            InitializeComponent();
            helper = new Helper();
            dbHelper = new DatabaseHelper();

            // Verify file access on form load
            SimpleFileLogger.VerifyFileAccess();

            // Add a test button for direct testing


            Label locationLabel = new Label
            {
                AutoSize = true,
                Location = new Point(12, this.Height - 100),
                Text = helper.GetUserLocation()
            };
            this.Controls.Add(locationLabel);
            try
            {

                if (dbHelper.TestConnection())
                {
                    Console.WriteLine("Database connection successful!");
                }
                else
                {
                    MessageBox.Show("Could not connect to database. Please check your connection settings.",
                        "Connection Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}",
                    "Connection Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        

        
        }
        private void bttnLogin_Click(object sender, EventArgs e)
        {
            string username = usernameInput.Text;
            string password = passwordInput.Text;
            int? userId = dbHelper.GetUserIdByUsername(username);
           
            // Validate input
            var (isValid, message) = ValidationHelper.ValidateLoginInput(username, password);
            if (!isValid)
            {
                MessageBox.Show(
                    helper.TranslateMessage(message),
                    helper.TranslateMessage("ValidationError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (dbHelper.ValidateUser(username, password))
            {
                // Log the login - using new simple logger
                SimpleFileLogger.LogLogin(username);

                dbHelper.LogLoginAttempt(username, true);
                CheckUpcomingAppointments(userId.Value);

               
                MessageBox.Show(
                    helper.TranslateMessage("LoginSuccessful"),
                    "",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                DatabaseHelper.SetCurrentUser(username);
                MainForm mainForm = new MainForm(username);
                mainForm.Show();
                this.Hide();
            }
            else
            {
                dbHelper.LogLoginAttempt(username, false);
                MessageBox.Show(
                    helper.TranslateMessage("LoginFailed"),
                    helper.TranslateMessage("ValidationError"),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        //public void CheckUpcomingAppointments(int userId)
        //{
        //    try
        //    {
        //        // returns the appt time in UTC from db
        //        var appointments = dbHelper.GetTodaysUpcomingAppointments(userId);
        //        if (appointments.Count <= 0) { return; }

        //        // Get the EST timezone
        //        TimeZoneInfo estTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                

        //        foreach (var (customerName, title, startTimeUtc) in appointments)
        //        {
        //            //get local machine's time offset to Utc 
        //            double localMachineOffsetToUtc = TimeZoneInfo.Local.GetUtcOffset(DateTime.Now).TotalHours;
        //            double estOffset = estTimeZone.BaseUtcOffset.TotalHours;

        //            if (localMachineOffsetToUtc != estOffset) 
        //            {
        //                var diffOfTheTwoTimes = Math.Abs(localMachineOffsetToUtc - estOffset);
        //                MessageBox.Show($"diffOfTheTwoTimes: {diffOfTheTwoTimes}");
        //                var timeResult = startTimeUtc.AddHours(diffOfTheTwoTimes);
        //                MessageBox.Show($"timeResult: {timeResult}");
        //            }
        //            //MessageBox.Show($"localMachineTimeOffsetToUtc: {localMachineOffsetToUtc}");




        //            // Convert UTC to EST properly accounting for daylight savings
        //            //DateTime startTimeUtcSpecified = DateTime.SpecifyKind(startTimeUtc, DateTimeKind.Utc);
        //            //DateTime startTimeEst = TimeZoneInfo.ConvertTimeFromUtc(startTimeUtcSpecified, estTimeZone);
        //            //DateTime startTimeEst = TimeZoneInfo.ConvertTimeFromUtc(startTimeUtc, estTimeZone);

        //            //offset
        //            // Get the base UTC offset in hours
        //            //double offsetHours = estTimeZone.BaseUtcOffset.TotalHours;
        //            //DateTime utcNow = DateTime.UtcNow;
        //            //MessageBox.Show($"UtcNow: {utcNow}");
        //            //DateTime currentUtcToEst = DateTime.UtcNow.AddHours(offsetHours);
        //            //MessageBox.Show($"offsetHours: {offsetHours}");
        //            //MessageBox.Show($"currentUtcToEst: {currentUtcToEst}");

        //            ////DateTime currEst = TimeZoneInfo.ConvertTimeFromUtc(currentUtc, estTimeZone);
        //            ////currEst = DateTime.SpecifyKind(currEst, DateTimeKind.Local);


        //            ////DateTime currEst = TimeZoneInfo.ConvertTimeFromUtc(DateTime.Now, estTimeZone);
        //            //MessageBox.Show($"Start Time EST: {startTimeEst}");

        //            //// Calculate time difference from current time
        //            //TimeSpan timeDifference = startTimeEst - (currentUtcToEst);

        //            //MessageBox.Show($"[func: CheckUpcomingappts - LoginForm] Start Time EST: {startTimeEst} | currEST: {currentUtcToEst} | Time diff: {timeDifference}");

        //            //if (timeDifference.TotalMinutes is >= 0 and <= 15)
        //            //{
        //            //    MessageBox.Show(
        //            //        $"Upcoming Appointment Reminder:\n\n" +
        //            //        $"Customer: {customerName}\n" +
        //            //        $"Title: {title}\n" +
        //            //        $"Time: {startTimeEst:hh:mm tt}\n\n" +
        //            //        $"Starting in {Math.Round(timeDifference.TotalMinutes)} minutes",
        //            //        "Appointment Reminder",
        //            //        MessageBoxButtons.OK,
        //            //        MessageBoxIcon.Information);
        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Error checking upcoming appointments: {ex.Message}",
        //            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}


        public void CheckUpcomingAppointments(int userId)
        {
            try
            {
                //returns the appt time in Utc 
                //1. get appts in UTC from db 
                var appointments = dbHelper.GetTodaysUpcomingAppointments(userId);

                if (appointments.Count <= 0) { return; }

                foreach (var (customerName, title, startTimeUtc) in appointments)
                {
                    DateTime startTimeEst = startTimeUtc.AddHours(-5);
                    DateTime currDateTime = DateTime.Now;

                    //MessageBox.Show($"[func CheckUpcomingAppts ]This is the var startTimeEST {startTimeEst}");
                    TimeSpan timeDifference = startTimeEst - currDateTime;
                    //MessageBox.Show($"[func CheckUpcomingAppts ] timeDifference {timeDifference}");
                    //                    MessageBox.Show($"[Func GetTodaysAppt] upcomingAppts: {string.Join(", ", appointments.Select(appointment =>
                    //$"{appointment.CustomerName} - {appointment.Title} at {appointment.StartTime}"))}");

                    if (timeDifference.TotalMinutes is >= 0 and <= 15)
                    {
                        MessageBox.Show(
                            $"Upcoming Appointment Reminder:\n\n" +
                            $"Customer: {customerName}\n" +
                            $"Title: {title}\n" +
                            $"Time: {startTimeEst:hh:mm tt}\n\n" +
                            $"Starting in {Math.Round(timeDifference.TotalMinutes)} minutes",
                            "Appointment Reminder",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking upcoming appointments: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bttnRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            NewUserForm newUserForm = new NewUserForm();
            newUserForm.Show();
        }

        private void bttnExit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
        "Are you sure you want to exit the application?",
        "Confirm Exit",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {

                Application.Exit();
            }
        }
    }
}
