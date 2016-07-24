using System;

namespace Core.Time
{
    public class Dates
    {

        public const long TicksPerMillSeconds = 10000L;
        public const long TicksPerSecond      = 10000000L;
        public const long TicksPerMinitutes   = 600000000L;
        public const long TicksPerHour        = 36000000000L;
        public const long TicksPerDay         = 864000000000L;
        
        public static DateTime To(DateTime source, TimeZoneInfo destinationZone)
        {
            return TimeZoneInfo.ConvertTime(source, destinationZone);
        }

        public static DateTime To(DateTimeOffset source, TimeZoneInfo destinationZone)
        {
            var ticks      = source.Ticks;
            var offset     = source.Offset;
            var baseOffset = destinationZone.BaseUtcOffset;
            var adjusted   = DateTime.Now;

            TimeZoneInfo.ConvertTime(source, destinationZone);
            if (offset == baseOffset)
            {
                        
            }

            TimeZoneInfo.ConvertTime(source, TimeZoneInfo.Utc);
            return TimeZoneInfo.ConvertTime(adjusted, TimeZoneInfo.Local, destinationZone);
        }

        public static TimeSpan Diff(DateTime dateTime1, DateTime dateTime2)
        {
            var tick1 = dateTime1.ToUniversalTime().Ticks;
            var tick2 = dateTime2.ToUniversalTime().Ticks;
            var diff  = Math.Abs(tick2 - tick1);
            return new TimeSpan(diff);
        }

        public static TimeSpan Diff(DateTimeOffset dateTime1, DateTimeOffset dateTime2)
        {
            var tick1 = dateTime1.ToUniversalTime().Ticks;
            var tick2 = dateTime2.ToUniversalTime().Ticks;
            var diff  = Math.Abs(tick2 - tick1);
            return new TimeSpan(diff);
        }

    }

}