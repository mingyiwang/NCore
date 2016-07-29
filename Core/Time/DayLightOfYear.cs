using System;
using System.Globalization;
using Core.Time.Zone;

namespace Core.Time
{

    public class DayLightOfYear
    {

        public static DayLightOfYear None => new DayLightOfYear(Year.Now, TimeZoneInfo.Local);

        public static DayLightOfYear Of(Year year)
        {
            return Of(year, TimeZoneInfo.Local);
        }

        public static DayLightOfYear Of(Year year, TimeZoneInfo zoneInfo)
        {
            return new DayLightOfYear(year, zoneInfo);
        }

        private readonly DateTimeRange _ambiguoiusTime;
        private readonly DateTimeRange _invalidTime;
        private readonly DateTimeRange _daylightSavingTime;

        private DayLightOfYear(Year year, TimeZoneInfo zoneInfo)
        {
            HasDayLightSavingTime = false;
            _ambiguoiusTime       = DateTimeRange.Of(DateTime.MinValue, DateTime.MinValue);
            _invalidTime          = DateTimeRange.Of(DateTime.MinValue, DateTime.MinValue);
            _daylightSavingTime   = DateTimeRange.Of(DateTime.MinValue, DateTime.MinValue);

            var dayLightTime = TimeZones.GetDaylightTime(zoneInfo, year.GetYear());
            if (dayLightTime != null)
            {
                       
            }
        }

        public bool HasDayLightSavingTime { get; }

        public bool IsAmbiguous(DateTime time)
        {
            return _ambiguoiusTime.Includes(time);
        }

        public bool IsInvalid(DateTime time)
        {
            return _invalidTime.Includes(time);
        }

        public bool IsDayLightSaving(DateTime time)
        {
            return _daylightSavingTime.Includes(time);
        }

        private void ParseDayLightTime(DaylightTime dayLightTime)
        {
            var startTime = dayLightTime.Start;
            var endTime   = dayLightTime.End;

            if (startTime.TimeOfDay < endTime.TimeOfDay)
            {

            }
            else
            {

            }

        }

    }

}