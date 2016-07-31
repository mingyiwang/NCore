using System;

namespace Core.Time.Extension
{

    public static class DayOfWeekExtension
    {

        public static int GetInt(this DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Monday    : return 1;
                case DayOfWeek.Tuesday   : return 2;
                case DayOfWeek.Wednesday : return 3;
                case DayOfWeek.Thursday  : return 4;
                case DayOfWeek.Friday    : return 5;
                case DayOfWeek.Saturday  : return 6;
                case DayOfWeek.Sunday    : return 0;
                default                  : return 0; // never happens
            }
        }

        public static DateTime ForWeekInMonthYear(this DayOfWeek dayOfWeek, int week, MonthOfYear my)
        {
            return my.GetDayOfWeekDateForWeek(week, dayOfWeek);
        }

    }

}