using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace schedulingApp
{
        public class TimeZoneDisplayInfo
        {
            public string Id { get; set; }
            public string DisplayName { get; set; }
            public TimeSpan UtcOffset { get; set; }
            public bool IsDaylightSavingTime { get; set; }
        }
    public static class AppointmentHelper
    {
       
        private static readonly TimeZoneInfo EasternZone = TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
        private static readonly TimeZoneInfo LocalZone = TimeZoneInfo.Local;

        public static List<TimeZoneDisplayInfo> GetAvailableTimeZones()
        {
            return TimeZoneInfo.GetSystemTimeZones()
                .Select(tz => new TimeZoneDisplayInfo
                {
                    Id = tz.Id,
                    DisplayName = tz.DisplayName,
                    UtcOffset = tz.GetUtcOffset(DateTime.Now),
                    IsDaylightSavingTime = tz.IsDaylightSavingTime(DateTime.Now)
                })
                .OrderBy(tz => tz.UtcOffset)
                .ToList();
        }

        public static (DateTime start, DateTime end) AdjustAppointmentTimesForTimeZone(
            DateTime startLocal,
            DateTime endLocal,
            string sourceTimeZoneId,
            string targetTimeZoneId)
        {
            try
            {
                var sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZoneId);
                var targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(targetTimeZoneId);

                // Convert to UTC first
                DateTime startUtc = TimeZoneInfo.ConvertTimeToUtc(startLocal, sourceTimeZone);
                DateTime endUtc = TimeZoneInfo.ConvertTimeToUtc(endLocal, sourceTimeZone);

                // Then convert to target timezone
                DateTime startTarget = TimeZoneInfo.ConvertTimeFromUtc(startUtc, targetTimeZone);
                DateTime endTarget = TimeZoneInfo.ConvertTimeFromUtc(endUtc, targetTimeZone);

                return (startTarget, endTarget);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adjusting timezone: {ex.Message}");
            }
        }
        public static (DateTime start, DateTime end) GetAdjustedBusinessHours(
           DateTime date,
           string userTimeZoneId)
        {
            try
            {
                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(userTimeZoneId);

                // Create Eastern Time business hours (9 AM to 5 PM ET)
                DateTime easternDate = TimeZoneInfo.ConvertTime(date, userTimeZone, EasternZone).Date;
                DateTime easternStart = easternDate.Add(BusinessHours.Start);
                DateTime easternEnd = easternDate.Add(BusinessHours.End);

                // Convert back to user's timezone
                DateTime userStart = TimeZoneInfo.ConvertTime(easternStart, EasternZone, userTimeZone);
                DateTime userEnd = TimeZoneInfo.ConvertTime(easternEnd, EasternZone, userTimeZone);

                return (userStart, userEnd);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calculating business hours: {ex.Message}");
            }
        }

        public static string GetTimeZoneInfoMessage(string timeZoneId)
        {
            try
            {
                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                DateTime now = DateTime.Now;
                DateTime userTime = TimeZoneInfo.ConvertTime(now, LocalZone, userTimeZone);
                DateTime easternTime = TimeZoneInfo.ConvertTime(now, LocalZone, EasternZone);

                TimeSpan userOffset = userTimeZone.GetUtcOffset(now);
                TimeSpan easternOffset = EasternZone.GetUtcOffset(now);
                TimeSpan difference = userOffset - easternOffset;

                bool userIsDst = userTimeZone.IsDaylightSavingTime(now);
                bool easternIsDst = EasternZone.IsDaylightSavingTime(now);

                var (businessStart, businessEnd) = GetAdjustedBusinessHours(now, timeZoneId);

                return $"Your timezone: {userTimeZone.DisplayName}\n" +
                       $"Current time in your zone: {userTime:HH:mm}\n" +
                       $"Eastern time: {easternTime:HH:mm}\n" +
                       $"Time difference with ET: {(difference.TotalHours > 0 ? "+" : "")}{difference.TotalHours:0.#} hours\n" +
                       $"Your timezone {(userIsDst ? "is" : "is not")} in DST\n" +
                       $"Eastern timezone {(easternIsDst ? "is" : "is not")} in DST\n\n" +
                       $"Business hours in your timezone: {businessStart:HH:mm} - {businessEnd:HH:mm}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting timezone info: {ex.Message}");
            }
        }

        public static string FormatAppointmentTimeZones(DateTime appointmentTime, string userTimeZoneId)
        {
            try
            {
                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(userTimeZoneId);
                DateTime userTime = TimeZoneInfo.ConvertTime(appointmentTime, LocalZone, userTimeZone);
                DateTime easternTime = TimeZoneInfo.ConvertTime(appointmentTime, LocalZone, EasternZone);
                DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(appointmentTime, LocalZone);

                return $"Your Time ({userTimeZone.StandardName}): {userTime:g}\n" +
                       $"Eastern Time (ET): {easternTime:g}\n" +
                       $"UTC: {utcTime:g}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error formatting appointment times: {ex.Message}");
            }
        }


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