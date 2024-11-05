using System;
using System.Collections.Generic;
using System.Data;

namespace schedulingApp
{
    public static class AppointmentHelper
    {
        private static readonly TimeZoneInfo PacificZone = TimeZoneInfo.FindSystemTimeZoneById("America/Los_Angeles");
        private static readonly TimeZoneInfo LocalZone = TimeZoneInfo.Local;

        public static bool IsWithinBusinessHours(DateTime localStartTime, DateTime localEndTime)
        {
            // Convert local time to Pacific Time
            DateTime pacificStart = TimeZoneInfo.ConvertTime(localStartTime, LocalZone, PacificZone);
            DateTime pacificEnd = TimeZoneInfo.ConvertTime(localEndTime, LocalZone, PacificZone);

            // Check if it's weekend
            if (pacificStart.DayOfWeek == DayOfWeek.Saturday || 
                pacificStart.DayOfWeek == DayOfWeek.Sunday ||
                pacificEnd.DayOfWeek == DayOfWeek.Saturday || 
                pacificEnd.DayOfWeek == DayOfWeek.Sunday)
            {
                return false;
            }

            // Business hours: 9 AM to 5 PM PT
            TimeSpan businessStart = new TimeSpan(9, 0, 0);
            TimeSpan businessEnd = new TimeSpan(17, 0, 0);

            return pacificStart.TimeOfDay >= businessStart &&
                   pacificEnd.TimeOfDay <= businessEnd;
        }

        public static string GetTimezoneDifferenceMessage()
        {
            DateTime now = DateTime.Now;
            DateTime pacificTime = TimeZoneInfo.ConvertTime(now, LocalZone, PacificZone);
            
            TimeSpan localOffset = LocalZone.GetUtcOffset(now);
            TimeSpan pacificOffset = PacificZone.GetUtcOffset(now);
            TimeSpan difference = localOffset - pacificOffset;

            bool localIsDst = LocalZone.IsDaylightSavingTime(now);
            bool pacificIsDst = PacificZone.IsDaylightSavingTime(now);

            string message = $"Your timezone: {LocalZone.DisplayName}\n" +
                           $"Current local time: {now:HH:mm}\n" +
                           $"Pacific time: {pacificTime:HH:mm}\n" +
                           $"Time difference with PT: {(difference.TotalHours > 0 ? "+" : "")}{difference.TotalHours:0.#} hours\n" +
                           $"Your timezone {(localIsDst ? "is" : "is not")} in DST\n" +
                           $"Pacific timezone {(pacificIsDst ? "is" : "is not")} in DST";

            return message;
        }

        public static string FormatTimeWithZones(DateTime localTime)
        {
            DateTime pacificTime = TimeZoneInfo.ConvertTime(localTime, LocalZone, PacificZone);
            return $"Local: {localTime:g} ({LocalZone.StandardName})\n" +
                   $"Pacific: {pacificTime:g} (PT)";
        }

        public static bool HasUpcomingAppointment(DataTable appointments)
        {
            if (appointments == null || appointments.Rows.Count == 0)
                return false;

            DateTime now = DateTime.Now;
            DateTime fifteenMinutesFromNow = now.AddMinutes(15);

            foreach (DataRow row in appointments.Rows)
            {
                // Convert UTC appointment time to local time
                DateTime appointmentStart = TimeZoneInfo.ConvertTimeFromUtc(
                    ((DateTime)row["start"]).ToUniversalTime(), 
                    LocalZone);

                if (appointmentStart >= now && appointmentStart <= fifteenMinutesFromNow)
                {
                    return true;
                }
            }

            return false;
        }

        public static string FormatAppointmentAlert(DataRow appointment)
        {
            // Convert UTC appointment time to local time
            DateTime startUtc = ((DateTime)appointment["start"]).ToUniversalTime();
            DateTime localStart = TimeZoneInfo.ConvertTimeFromUtc(startUtc, LocalZone);
            DateTime pacificStart = TimeZoneInfo.ConvertTimeFromUtc(startUtc, PacificZone);

            return $"You have an upcoming appointment:\n" +
                   $"Customer: {appointment["customerName"]}\n" +
                   $"Title: {appointment["title"]}\n" +
                   $"Time (Your local time): {localStart:g}\n" +
                   $"Time (Pacific Time): {pacificStart:g}";
        }

        public static class BusinessHours
        {
            public static TimeSpan Start => new TimeSpan(9, 0, 0);  // 9 AM
            public static TimeSpan End => new TimeSpan(17, 0, 0);   // 5 PM

            public static (DateTime start, DateTime end) GetBusinessHoursInLocalTime(DateTime date)
            {
                // Create Pacific Time business hours
                DateTime pacificDate = TimeZoneInfo.ConvertTime(date, LocalZone, PacificZone).Date;
                DateTime pacificStart = pacificDate.Add(Start);
                DateTime pacificEnd = pacificDate.Add(End);

                // Convert back to local time
                DateTime localStart = TimeZoneInfo.ConvertTime(pacificStart, PacificZone, LocalZone);
                DateTime localEnd = TimeZoneInfo.ConvertTime(pacificEnd, PacificZone, LocalZone);

                return (localStart, localEnd);
            }

            public static string GetBusinessHoursMessage(DateTime date)
            {
                var (localStart, localEnd) = GetBusinessHoursInLocalTime(date);
                return $"Business hours for {date:d}:\n" +
                       $"Your time: {localStart:HH:mm} - {localEnd:HH:mm}\n" +
                       $"Pacific Time: {Start:hh\\:mm} - {End:hh\\:mm}";
            }
        }
    }
}
