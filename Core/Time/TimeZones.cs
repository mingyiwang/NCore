using System;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Core.Time
{

    public class TimeZones
    {

        public static readonly Lazy<TimeZoneInfo> AuEast    = GetTimeZoneById("AUS Eastern Standard Time");
        public static readonly Lazy<TimeZoneInfo> AuCentral = GetTimeZoneById("AUS Central Standard Time");
        public static readonly Lazy<TimeZoneInfo> AuWest    = GetTimeZoneById("W. Australia Standard Time");
        public static readonly Lazy<TimeZoneInfo> China     = GetTimeZoneById("China Standard Time");

        private static Lazy<TimeZoneInfo> GetTimeZoneById(string id)
        {
            return new Lazy<TimeZoneInfo>(() =>
            {
                var timeZone = TimeZoneInfo.GetSystemTimeZones().SingleOrDefault(zone => zone.Id.Equals(id));
                return timeZone ?? TimeZoneInfo.Utc;
            }, LazyThreadSafetyMode.ExecutionAndPublication
            );
        }

        internal static DaylightTime GetDaylightTime(TimeZoneInfo timeZone, int year)
        {
            
            var adjustRule = timeZone
                .GetAdjustmentRules()
                .SingleOrDefault(rule => rule.DateStart.Year <= year && rule.DateEnd.Year >= year);

            // There is no Day Light Time Saving of this year in this time zone
            if (adjustRule == null)
            {
                return null;
            }

            DateTime endDate;
                    
            var start = adjustRule.DaylightTransitionStart;
            var end   = adjustRule.DaylightTransitionEnd;
            var startDate = start.IsFixedDateRule 
                          ? Dates.Of(year, Month.Of(start.Month), start.Day).Add(TimeSpan.FromTicks(start.TimeOfDay.Ticks)) 
                          : GetTransitionTimeDay(start, year);

            if (end.IsFixedDateRule)
            {
                if (end.Day == 1 && end.Month == 1 && end.TimeOfDay == DateTime.MinValue)
                {
                    endDate = DateTime.MaxValue;
                } else
                {
                    endDate = Dates.Of(year, Month.Of(end.Month), end.Day).Add(TimeSpan.FromTicks(end.TimeOfDay.Ticks));
                }
            }
            else
            {
                endDate = GetTransitionTimeDay(end, year);
            }

            return new DaylightTime(startDate, endDate, adjustRule.DaylightDelta);
        }

        private static DateTime GetTransitionTimeDay(TimeZoneInfo.TransitionTime transitionTime, int year)
        {
            DateTime endDate;
            var endMonth = Month.Of(transitionTime.Month);
            var lastDayOfMonth = Dates.Of(year, endMonth, 7 * (transitionTime.Week - 1) + 1); // get last day of this week

            var firstDayOfLastWeek  = (int) lastDayOfMonth.DayOfWeek;
            var targetDayOfThisWeek = (int) transitionTime.DayOfWeek;
            
            if (targetDayOfThisWeek == firstDayOfLastWeek)
            {
                endDate = lastDayOfMonth.Add(TimeSpan.FromTicks(transitionTime.TimeOfDay.Ticks));
            }
            else
            {
                var days = TimeSpan.FromDays(Math.Abs(targetDayOfThisWeek - firstDayOfLastWeek));
                endDate  = lastDayOfMonth.Add(days).Add(TimeSpan.FromTicks(transitionTime.TimeOfDay.Ticks));
            }
            return endDate;
        }

        private TimeZones(){}

      
    }
}