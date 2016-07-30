using System;
using System.Globalization;

namespace Core.Time.Zone
{

    public sealed class DayLightOfYear
    {

        public DateTime AmbiguiousTimeStart { get; private set; }
        public DateTime AmbiguiousTimeEnd   { get; private set; }
        public DateTime InvalidTimeStart    { get; private set; }
        public DateTime InvalidTimeEnd      { get; private set; }

        private DayLightOfYear(Year year, TimeZoneInfo zoneInfo)
        {
            TimeZone = zoneInfo;
            Year = year;
            HasDayLightSavingTime = false;
            Delta = TimeSpan.Zero;
            AmbiguiousTimeStart = AmbiguiousTimeEnd = DateTime.MinValue;
            InvalidTimeStart = InvalidTimeEnd = DateTime.MinValue;

            var dayLightTime = TimeZones.GetDaylightTime(zoneInfo, year.GetYear());
            if (dayLightTime != null)
            {
                HasDayLightSavingTime = true;
                Delta = dayLightTime.Delta;
                ParseDayLightTime(dayLightTime);       
            }
        }

        public static DayLightOfYear Of(Year year)
        {
            return Of(year, TimeZoneInfo.Local);
        }

        public static DayLightOfYear Of(Year year, TimeZoneInfo zoneInfo)
        {
            return new DayLightOfYear(year, zoneInfo);
        }

        public static DayLightOfYear Of(DateTime dateTime)
        {
            return new DayLightOfYear(Year.Of(dateTime.Year), TimeZoneInfo.Local);
        }

        public static DayLightOfYear Of(DateTime dateTime, TimeZoneInfo zoneInfo)
        {
            return new DayLightOfYear(Year.Of(dateTime.Year), zoneInfo);
        }

        public static DayLightOfYear Of(DateTimeOffset dateTime)
        {
            return new DayLightOfYear(Year.Of(dateTime.LocalDateTime.Year), TimeZoneInfo.Local);
        }

        public static DateTime Adjust(DateTime dateTime)
        {
            var dayLight = Of(dateTime);
            if (dayLight.HasDayLighSavingTime())
            {
                return !dayLight.IsInvalid(dateTime) ? dateTime : dateTime.Add(dayLight.Delta);
            }

            return dateTime;
        }

        public bool HasDayLightSavingTime
        {
            get;
        }

        public TimeSpan Delta
        {
            get;
        }

        public TimeZoneInfo TimeZone
        {
            get;
        }

        public Year Year
        {
            get;
        }

        public bool HasDayLighSavingTime()
        {
            return HasDayLightSavingTime;
        }

        public bool IsAmbiguous(DateTime time)
        {
            Checks.Equals(Year.GetYear(), time.Year);
            if(!HasDayLightSavingTime)
            {
                return false;
            }
            return time >= AmbiguiousTimeStart && time <= AmbiguiousTimeEnd;
        }

        public bool IsInvalid(DateTime time)
        {
            Checks.Equals(Year.GetYear(), time.Year);
            if(!HasDayLightSavingTime)
            {
                return false;
            }
            return time >= InvalidTimeStart && time < InvalidTimeEnd;
        }

        private void ParseDayLightTime(DaylightTime dayLightTime)
        {
            var startTime = dayLightTime.Start;
            var endTime   = dayLightTime.End;
            var delta     = dayLightTime.Delta;

            if (startTime.TimeOfDay > endTime.TimeOfDay)
            {
                // Clock move backward
                AmbiguiousTimeStart = startTime.Subtract(delta);
                AmbiguiousTimeEnd  = startTime;
                InvalidTimeStart   = endTime;
                InvalidTimeEnd     = endTime.Add(delta);
            }
            else
            {
                // Clock move forward
                AmbiguiousTimeStart = endTime.Subtract(delta);
                AmbiguiousTimeEnd  = endTime;
                InvalidTimeStart   = startTime;
                InvalidTimeEnd     = startTime.Add(delta);
            }

        }

    }

}