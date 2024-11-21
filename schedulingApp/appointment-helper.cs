using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace schedulingApp
{
    public static class AppointmentHelper
    {
        private static readonly TimeZoneInfo EasternZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
        private static readonly TimeZoneInfo LocalZone = TimeZoneInfo.Local;

        public static bool IsWithinBusinessHours(DateTime localStartTime, DateTime localEndTime)
        {
            // Convert local time to Eastern Time
            DateTime easternStart = TimeZoneInfo.ConvertTime(localStartTime, LocalZone, EasternZone);
            DateTime easternEnd = TimeZoneInfo.ConvertTime(localEndTime, LocalZone, EasternZone);

            // Check if it's weekend
            if (easternStart.DayOfWeek == DayOfWeek.Saturday ||
                easternStart.DayOfWeek == DayOfWeek.Sunday ||
                easternEnd.DayOfWeek == DayOfWeek.Saturday ||
                easternEnd.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            // Business hours: 9 AM to 5 PM ET
            TimeSpan businessStart = new TimeSpan(9, 0, 0);
            TimeSpan businessEnd = new TimeSpan(17, 0, 0);

            return easternStart.TimeOfDay >= businessStart &&
                   easternEnd.TimeOfDay <= businessEnd;
        }

        public static bool HasOverlappingAppointments(DateTime newStartTime, DateTime newEndTime, DataTable existingAppointments)
        {
            if (existingAppointments == null || existingAppointments.Rows.Count == 0)
                return false;
            MessageBox.Show($"This is the start and end times received: start {newStartTime}, end {newEndTime} and dataTable: {existingAppointments}");
            // Convert new appointment times to UTC for comparison
            DateTime newStartUtc = TimeZoneInfo.ConvertTimeToUtc(newStartTime, LocalZone);
            DateTime newEndUtc = TimeZoneInfo.ConvertTimeToUtc(newEndTime, LocalZone);

            foreach (DataRow row in existingAppointments.Rows)
            {
                DateTime existingStartUtc = ((DateTime)row["start"]);
                DateTime existingEndUtc = ((DateTime)row["end"]);
                MessageBox.Show($"This is the dataTable start/end time: {existingStartUtc} endtime: {existingEndUtc}");

                // Check for overlap
                if (!(newEndUtc <= existingStartUtc || newStartUtc >= existingEndUtc))
                {
                    return true; // Overlap found
                }
            }

            return false;
        }

        public static DateTime AdjustAppointmentTimeForTimeZone(DateTime appointmentTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo targetTimeZone)
        {
            return TimeZoneInfo.ConvertTime(appointmentTime, sourceTimeZone, targetTimeZone);
        }

        public static string GetTimezoneDifferenceMessage()
        {
            DateTime now = DateTime.Now;
            DateTime easternTime = TimeZoneInfo.ConvertTime(now, LocalZone, EasternZone);

            TimeSpan localOffset = LocalZone.GetUtcOffset(now);
            TimeSpan easternOffset = EasternZone.GetUtcOffset(now);
            TimeSpan difference = localOffset - easternOffset;

            bool localIsDst = LocalZone.IsDaylightSavingTime(now);
            bool easternIsDst = EasternZone.IsDaylightSavingTime(now);

            string message = $"Your timezone: {LocalZone.DisplayName}\n" +
                           $"Current local time: {now:HH:mm}\n" +
                           $"Eastern time: {easternTime:HH:mm}\n" +
                           $"Time difference with ET: {(difference.TotalHours > 0 ? "+" : "")}{difference.TotalHours:0.#} hours\n" +
                           $"Your timezone {(localIsDst ? "is" : "is not")} in DST\n" +
                           $"Eastern timezone {(easternIsDst ? "is" : "is not")} in DST";

            return message;
        }

        public static (List<DataRow> upcomingAppointments, string alertMessage) CheckForUpcomingAppointments(
            DataTable appointments,
            string userId,
            int minutesThreshold = 15)
        {
            var upcomingAppointments = new List<DataRow>();
            if (appointments == null || appointments.Rows.Count == 0)
                return (upcomingAppointments, null);

            DateTime now = DateTime.Now;
            DateTime thresholdTime = now.AddMinutes(minutesThreshold);

            foreach (DataRow row in appointments.Rows)
            {
                if (row["userId"].ToString() != userId)
                    continue;

                DateTime appointmentStart = TimeZoneInfo.ConvertTimeFromUtc(
                    ((DateTime)row["start"]).ToUniversalTime(),
                    LocalZone);

                if (appointmentStart >= now && appointmentStart <= thresholdTime)
                {
                    upcomingAppointments.Add(row);
                }
            }

            if (upcomingAppointments.Count == 0)
                return (upcomingAppointments, null);

            // Generate alert message for all upcoming appointments
            string alertMessage = "Upcoming Appointments:\n\n" +
                                string.Join("\n\n", upcomingAppointments.Select(FormatAppointmentAlert));

            return (upcomingAppointments, alertMessage);
        }

        public static string FormatAppointmentAlert(DataRow appointment)
        {
            DateTime startUtc = ((DateTime)appointment["start"]).ToUniversalTime();
            DateTime localStart = TimeZoneInfo.ConvertTimeFromUtc(startUtc, LocalZone);
            DateTime easternStart = TimeZoneInfo.ConvertTimeFromUtc(startUtc, EasternZone);

            return $"Appointment Details:\n" +
                   $"Customer: {appointment["customerName"]}\n" +
                   $"Title: {appointment["title"]}\n" +
                   $"Time (Your local time): {localStart:g}\n" +
                   $"Time (Eastern Time): {easternStart:g}";
        }

        public static class BusinessHours
        {
            public static TimeSpan Start => new TimeSpan(9, 0, 0);  // 9 AM
            public static TimeSpan End => new TimeSpan(17, 0, 0);   // 5 PM

            public static (DateTime start, DateTime end) GetBusinessHoursInLocalTime(DateTime date)
            {
                // Create Eastern Time business hours
                DateTime easternDate = TimeZoneInfo.ConvertTime(date, LocalZone, EasternZone).Date;
                DateTime easternStart = easternDate.Add(Start);
                DateTime easternEnd = easternDate.Add(End);

                // Convert back to local time
                DateTime localStart = TimeZoneInfo.ConvertTime(easternStart, EasternZone, LocalZone);
                DateTime localEnd = TimeZoneInfo.ConvertTime(easternEnd, EasternZone, LocalZone);

                return (localStart, localEnd);
            }

            public static string GetBusinessHoursMessage(DateTime date)
            {
                var (localStart, localEnd) = GetBusinessHoursInLocalTime(date);
                return $"Business hours for {date:d}:\n" +
                       $"Your time: {localStart:HH:mm} - {localEnd:HH:mm}\n" +
                       $"Eastern Time: {Start:hh\\:mm} - {End:hh\\:mm}";
            }
        }

        public static string FormatTimeWithZones(DateTime localTime)
        {
            DateTime easternTime = TimeZoneInfo.ConvertTime(localTime, LocalZone, EasternZone);
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(localTime, LocalZone);

            return $"Local: {localTime:g} ({LocalZone.StandardName})\n" +
                   $"Eastern: {easternTime:g} (ET)\n" +
                   $"UTC: {utcTime:g} (UTC)";
        }
    }
}