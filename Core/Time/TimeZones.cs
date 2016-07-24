using System;
using System.Linq;
using System.Threading;

namespace Core.Time
{

    public class TimeZones
    {

        public static readonly Lazy<TimeZoneInfo> AuEast    = GetTimeZoneInternal("AUS Eastern Standard Time");
        public static readonly Lazy<TimeZoneInfo> AuCentral = GetTimeZoneInternal("AUS Central Standard Time");
        public static readonly Lazy<TimeZoneInfo> AuWest    = GetTimeZoneInternal("W. Australia Standard Time");
        public static readonly Lazy<TimeZoneInfo> China     = GetTimeZoneInternal("China Standard Time");

        private static Lazy<TimeZoneInfo> GetTimeZoneInternal(string id)
        {
            return new Lazy<TimeZoneInfo>(() =>
            {
                var timeZone = TimeZoneInfo.GetSystemTimeZones().SingleOrDefault(zone => zone.Id.Equals(id));
                return timeZone ?? TimeZoneInfo.Utc;
            }, LazyThreadSafetyMode.ExecutionAndPublication);

        }

        private TimeZones()
        {

        }

    }
}