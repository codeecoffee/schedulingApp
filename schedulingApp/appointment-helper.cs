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

        public static class BusinessHours
        {
            public static TimeSpan Start => new TimeSpan(9, 0, 0);  // 9 AM
            public static TimeSpan End => new TimeSpan(17, 0, 0);   // 5 PM
        }

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

        public static string GetTimeZoneInfoMessage(string timeZoneId)
        {
            try
            {
                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

               
                DateTime now = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Local);

                // Convert to UTC first, then to EST
                DateTime utcNow = TimeZoneInfo.ConvertTimeToUtc(now);
                DateTime userTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, userTimeZone);
                DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, EasternZone);

                TimeSpan userOffset = userTimeZone.GetUtcOffset(utcNow);
                TimeSpan easternOffset = EasternZone.GetUtcOffset(utcNow);
                TimeSpan difference = userOffset - easternOffset;

                bool userIsDst = userTimeZone.IsDaylightSavingTime(userTime);
                bool easternIsDst = EasternZone.IsDaylightSavingTime(easternTime);

                
                var (businessStart, businessEnd) = GetAdjustedBusinessHours(
                    TimeZoneInfo.ConvertTimeFromUtc(utcNow, userTimeZone),
                    timeZoneId);

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

        public static (DateTime start, DateTime end) GetAdjustedBusinessHours(DateTime date, string userTimeZoneId)
        {
            try
            {
                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(userTimeZoneId);

                // Convert to UTC first
                DateTime utcDate = TimeZoneInfo.ConvertTimeToUtc(date);

                // Convert UTC to Eastern time
                DateTime easternDate = TimeZoneInfo.ConvertTimeFromUtc(utcDate, EasternZone).Date;

                // Create business hours in Eastern time
                DateTime easternStart = easternDate.Add(BusinessHours.Start);
                DateTime easternEnd = easternDate.Add(BusinessHours.End);

                // Convert Eastern time to UTC
                DateTime startUtc = TimeZoneInfo.ConvertTimeToUtc(easternStart, EasternZone);
                DateTime endUtc = TimeZoneInfo.ConvertTimeToUtc(easternEnd, EasternZone);

                // Convert UTC to user timezone
                DateTime userStart = TimeZoneInfo.ConvertTimeFromUtc(startUtc, userTimeZone);
                DateTime userEnd = TimeZoneInfo.ConvertTimeFromUtc(endUtc, userTimeZone);

                return (userStart, userEnd);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error calculating business hours: {ex.Message}");
            }
        }

        public static string FormatAppointmentTimeZones(DateTime appointmentTime, string userTimeZoneId)
        {
            try
            {
                var userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(userTimeZoneId);
                DateTime userTime = TimeZoneInfo.ConvertTime(appointmentTime, LocalZone, userTimeZone);
                DateTime easternTime = TimeZoneInfo.ConvertTime(appointmentTime, LocalZone, EasternZone);

                // Get timezone abbreviations
                string userAbbr = GetTimeZoneAbbreviation(userTimeZone);

                return $"[{userAbbr}] {userTime:MM/dd/yyyy hh:mm tt} (Your time)\n" +
                       $"[EST] {easternTime:MM/dd/yyyy hh:mm tt}";
            }
            catch (Exception ex)
            {
                throw new Exception($"Error formatting appointment times: {ex.Message}");
            }
        }

        private static string GetTimeZoneAbbreviation(TimeZoneInfo timeZone)
        {
            // Common abbreviations
            if (timeZone.Id.Contains("Pacific"))
                return "PST";
            if (timeZone.Id.Contains("Mountain"))
                return "MST";
            if (timeZone.Id.Contains("Central"))
                return "CST";
            if (timeZone.Id.Contains("Eastern"))
                return "EST";
            if (timeZone.Id.Contains("Alaska"))
                return "AKST";
            if (timeZone.Id.Contains("Hawaii"))
                return "HST";

            // other timezones, use UTC offset
            var offset = timeZone.GetUtcOffset(DateTime.Now);
            return $"UTC{(offset.Hours >= 0 ? "+" : "")}{offset.Hours}";
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
            return easternStart.TimeOfDay >= BusinessHours.Start &&
                   easternEnd.TimeOfDay <= BusinessHours.End;
        }

        public static bool HasOverlappingAppointments(DateTime newStartTime, DateTime newEndTime, DataTable existingAppointments)
        {
            if (existingAppointments == null || existingAppointments.Rows.Count == 0)
                return false;

            // Convert new appointment times to UTC
            DateTime newStartUtc = TimeZoneInfo.ConvertTimeToUtc(newStartTime, LocalZone);
            DateTime newEndUtc = TimeZoneInfo.ConvertTimeToUtc(newEndTime, LocalZone);

            foreach (DataRow row in existingAppointments.Rows)
            {
                // Get existing appointment times and ensure they're in UTC
                DateTime existingStartUtc = DateTime.SpecifyKind(
                    Convert.ToDateTime(row["start"]),
                    DateTimeKind.Utc);
                DateTime existingEndUtc = DateTime.SpecifyKind(
                    Convert.ToDateTime(row["end"]),
                    DateTimeKind.Utc);

                // Check for any type of overlap:
                // 1. New appointment starts during existing appointment
                // 2. New appointment ends during existing appointment
                // 3. New appointment completely contains existing appointment
                // 4. New appointment is completely contained within existing appointment
                if (newStartUtc < existingEndUtc && newEndUtc > existingStartUtc)
                {
                    return true; // Overlap found
                }
            }

            return false;
        }
        //public static bool HasOverlappingAppointments(DateTime newStartTime, DateTime newEndTime, DataTable existingAppointments)
        //{
        //    if (existingAppointments == null || existingAppointments.Rows.Count == 0)
        //        return false;


        //    DateTime newStartUtc = TimeZoneInfo.ConvertTimeToUtc(newStartTime, LocalZone);
        //    DateTime newEndUtc = TimeZoneInfo.ConvertTimeToUtc(newEndTime, LocalZone);

        //    foreach (DataRow row in existingAppointments.Rows)
        //    {
        //        DateTime existingStartUtc = ((DateTime)row["start"]).ToUniversalTime();
        //        DateTime existingEndUtc = ((DateTime)row["end"]).ToUniversalTime();

        //        // Check for overlap
        //        if (!(newEndUtc <= existingStartUtc || newStartUtc >= existingEndUtc))
        //        {
        //            return true; // Overlap found
        //        }
        //    }

        //    return false;
        //}    

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