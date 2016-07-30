using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Core.Time.Zone
{

    public sealed class TimeZones
    {

        public static readonly Lazy<TimeZoneInfo> AuEast    = GetTimeZoneById("AUS Eastern Standard Time");
        public static readonly Lazy<TimeZoneInfo> AuCentral = GetTimeZoneById("AUS Central Standard Time");
        public static readonly Lazy<TimeZoneInfo> AuWest    = GetTimeZoneById("W. Australia Standard Time");
        public static readonly Lazy<TimeZoneInfo> China     = GetTimeZoneById("China Standard Time");

        public static Lazy<TimeZoneInfo> GetTimeZoneById(string id)
        {
            return new Lazy<TimeZoneInfo>(() =>
            {
                var timeZone = TimeZoneInfo.GetSystemTimeZones().SingleOrDefault(zone => zone.Id.Equals(id));
                return timeZone ?? TimeZoneInfo.Utc;
            }, LazyThreadSafetyMode.ExecutionAndPublication
            );
        }

        public static DaylightTime GetDaylightTime(TimeZoneInfo timeZone, int year)
        {
            
            var adjustRule = timeZone
                .GetAdjustmentRules()
                .SingleOrDefault(rule => rule.DateStart.Year <= year && rule.DateEnd.Year >= year);

            // There is no Day Light Time Saving of this year in this time zone
            if (adjustRule == null)
            {
                return null;
            }
          
            var start = adjustRule.DaylightTransitionStart;
            var end   = adjustRule.DaylightTransitionEnd;
            var startDate = start.IsFixedDateRule 
                          ? Dates.Of(year, Month.Of(start.Month), start.Day).Add(TimeSpan.FromTicks(start.TimeOfDay.Ticks)) 
                          : MonthOfYear.Of(start.Month, year)
                                       .GetDayOfWeekDateInWeek(start.Week, start.DayOfWeek)
                                       .Add(TimeSpan.FromTicks(start.TimeOfDay.Ticks));

            DateTime endDate;
            if (end.IsFixedDateRule)
            {
                if (end.Day == 1 && end.Month == 1 && end.TimeOfDay == DateTime.MinValue)
                {
                    endDate = DateTime.MaxValue; // never end
                }
                else
                {
                    endDate = Dates.Of(year, Month.Of(end.Month), end.Day).Add(TimeSpan.FromTicks(end.TimeOfDay.Ticks));
                }
            }
            else
            {
                endDate = MonthOfYear.Of(end.Month, year)
                                     .GetDayOfWeekDateInWeek(end.Week, end.DayOfWeek)
                                     .Add(TimeSpan.FromTicks(end.TimeOfDay.Ticks));
            }

            return new DaylightTime(startDate, endDate, adjustRule.DaylightDelta);
        }

        private TimeZones(){}

      
    }
}