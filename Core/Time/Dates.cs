using System;
using Core.Time.Zone;

namespace Core.Time
{

    public class Dates
    {

        public const long TicksPerMillSeconds = 10000;
        public const long TicksPerSecond = TicksPerMillSeconds * 1000;
        public const long TicksPerMinute = 60 * TicksPerSecond;
        public const long TicksPerHour   = 60 * TicksPerMinute;
        public const long TicksPerDay    = 24 * TicksPerHour;
        public const int  DaysOfLeapYearMonth = 29;
        public const int  DaysOfLeapYear = 366;
        public const int  DaysOfYear = 365;

        public static DateTime Of(int year, Month month, int day)
        {
            return Of(year, month, day, DateTimeKind.Local);
        }

        public static DateTime Of(int year, Month month, int day, DateTimeKind kind)
        {
            return DateTime.SpecifyKind(new DateTime(Year.CheckRange(year), month.GetMonth(), month.CheckRange(day, year)), kind);
        }

        public static bool Equals(DateTime dateTime1, DateTime dateTime2)
        {
            return Diff(dateTime1, dateTime2) == TimeSpan.Zero;
        }

        public static bool Equals(DateTimeOffset dateTime1, DateTimeOffset dateTime2)
        {
            return Diff(dateTime1, dateTime2) == TimeSpan.Zero;
        }

        public static TimeSpan Diff(DateTime dateTime1, DateTime dateTime2)
        {
            if (TimeZoneInfo.Local.IsInvalidTime(dateTime1) || TimeZoneInfo.Local.IsInvalidTime(dateTime2))
            {
                throw new ArgumentException("");
            }

            var tick1 = dateTime1.ToUniversalTime().Ticks;
            var tick2 = dateTime2.ToUniversalTime().Ticks;
            var diff  = Math.Abs(tick2 - tick1);
            return new TimeSpan(diff);
        }

        public static TimeSpan Diff(DateTimeOffset dateTime1, DateTimeOffset dateTime2)
        {
            if (TimeZoneInfo.Local.IsInvalidTime(dateTime1.LocalDateTime) 
            ||  TimeZoneInfo.Local.IsInvalidTime(dateTime2.LocalDateTime))
            {
                throw new ArgumentException("");
            }

            var diff = Math.Abs(dateTime2.Ticks - dateTime1.Ticks);
            return new TimeSpan(diff);
        }

        public static DateTime AdjustIn(DateTime dateTime, TimeZoneInfo timeZone)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return dateTime;
            }

            var dalighLight = TimeZones.GetDaylightTime(timeZone, dateTime.Year);
            return dateTime;
        }

    }

}