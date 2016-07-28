using System;
using System.Globalization;

namespace Core.Time
{

    public class DayLightOfYear
    {

        public static DayLightOfYear None => new DayLightOfYear()
        {
            HasDayLightSavingTime = false
        };

        public bool HasDayLightSavingTime { get; private set; }

        public bool IsOverlapped(DateTime dateTime)
        {
            return true;
        }

        public bool IsInvalid(DateTime dateTime)
        {
            return false;
        }

        public bool Is(DateTime time)
        {
            return true;
        }

        public static DayLightOfYear Of(DaylightTime time)
        {
            return new DayLightOfYear();
        }

        public static DayLightOfYear Of(Year year)
        {
            return Of(TimeZones.GetDaylightTime(TimeZoneInfo.Local, year.GetYear()));
        }

        public static DayLightOfYear Of(Year year, TimeZoneInfo zoneInfo)
        {
            return Of(TimeZones.GetDaylightTime(zoneInfo, year.GetYear()));
        }

    }

}