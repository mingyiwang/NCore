using System;

namespace Core.Time
{

    public class Dates
    {

        public const long TicksPerMillSeconds = 10000;
        public const long TicksPerSecond = TicksPerMillSeconds * 1000;
        public const long TicksPerMinute = 60 * TicksPerSecond;
        public const long TicksPerHour   = 60 * TicksPerMinute;
        public const long TicksPerDay    = 24 * TicksPerHour;

        public const int DaysOfLeapYearMonth = 29;

        public static DateTime Of(int year, Month month, int day)
        {
            return Of(year, month, day, DateTimeKind.Local);
        }

        public static DateTime Of(int year, Month month, int day, DateTimeKind kind)
        {
            return DateTime.SpecifyKind(new DateTime(year, month.GetMonth(), day), kind);
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
            if (TimeZoneInfo.Local.IsInvalidTime(dateTime1) 
             || TimeZoneInfo.Local.IsInvalidTime(dateTime2))
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
            if(TimeZoneInfo.Local.IsInvalidTime(dateTime1.LocalDateTime) 
            || TimeZoneInfo.Local.IsInvalidTime(dateTime2.LocalDateTime))
            {
                throw new ArgumentException("");
            }

            var diff = Math.Abs(dateTime2.Ticks - dateTime1.Ticks);
            return new TimeSpan(diff);
        }


        public static TimeSpan From(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday    : return TimeSpan.FromDays(1);
                case DayOfWeek.Monday    : return TimeSpan.FromDays(2);
                case DayOfWeek.Tuesday   : return TimeSpan.FromDays(3);
                case DayOfWeek.Wednesday : return TimeSpan.FromDays(4);
                case DayOfWeek.Thursday  : return TimeSpan.FromDays(5);
                case DayOfWeek.Friday    : return TimeSpan.FromDays(5);
                case DayOfWeek.Saturday  : return TimeSpan.FromDays(7);
                default                  : return TimeSpan.Zero;
            }
        }

        public static DateTime AdjustIn(DateTime dateTime, TimeZoneInfo timeZone)
        {
            if (dateTime.Kind == DateTimeKind.Utc)
            {
                return dateTime;
            }

            if (dateTime.Kind == DateTimeKind.Local)
            {
                        
            }

            if (timeZone.IsInvalidTime(dateTime))
            {
                
                
            }

            if (timeZone.IsDaylightSavingTime(dateTime) || timeZone.IsAmbiguousTime(dateTime))
            {
                return dateTime;
            }

            // we move time forward by some delta
            if (!timeZone.IsInvalidTime(dateTime)) return dateTime;
            dateTime.IsDaylightSavingTime();
            //var dst = RuleOf(dateTime, timeZone);
            return dateTime;
        }

    }

}