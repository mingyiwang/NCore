using System;

namespace Core.Time
{

    public class Dates
    {

        public DateTime Nil = new DateTime(-1, DateTimeKind.Unspecified);

        //1 Tick = 100 nano Seconds
        public const double TicksPerNanoSeconds = 0.01;

        // Ticks for 1 MillSeconds
        public const long TicksPerMillSeconds = 10000;

        // Ticks for 1 Second
        public const long TicksPerSecond = TicksPerMillSeconds * 1000;

        // Ticks for 1 Minute
        public const long TicksPerMinute = 60 * TicksPerSecond;

        // Ticks for 1 Hour
        public const long TicksPerHour   = 60 * TicksPerMinute;

        // Ticks for 1 Day
        public const long TicksPerDay    = 24 * TicksPerHour;

        // Number of days in Leap Year
        public const int DaysPerLeapYear = 366;

        // Number of day in Normal Year
        public const int DaysPerYear = 365;

        // Number of days in 4 years, Every four years
        public const int DaysPer4Years = DaysPerYear * 4 + 1;

        // Number of days in 100 years
        public const int DaysPer100Years = DaysPer4Years * 25 - 1;

        // Number of days in 400 years
        public const int DaysPer400Years = DaysPer100Years * 4 + 1;

        // Number of days from 1/1/0001 to The End of 12/31/1600 
        public const int DaysTo1601 = DaysPer400Years * 4;

        // Number of days from 1/1/0001 to The End of 12/30/1899
        public const int DaysTo1899 = DaysPer400Years * 4  + DaysPer100Years * 3 - 367;

        // Number of days from 1/1/0001 to The End of 12/31/1969 AM
        public const int DaysTo1970 = 719162;

        // Number of days from 1/1/0001 to The End of 12/31/9999
        public const int DaysTo10000 = DaysPer400Years * 25 - 366;
        
        // Day of Week of 1/1/0001
        public const DayOfWeek FirstDayOfWeek = DayOfWeek.Monday;

        public static readonly DateTime Epoch    = new DateTime(DaysTo1970 * TicksPerDay, DateTimeKind.Utc);
        public static readonly DateTime FirstDay = new DateTime(0, DateTimeKind.Utc);

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
                throw new ArgumentException("DateTime must not be invalid");
            }

            var diff = Math.Abs(dateTime2.Ticks - dateTime1.Ticks);
            return new TimeSpan(diff);
        }

    }

}