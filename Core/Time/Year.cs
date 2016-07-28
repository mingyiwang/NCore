using System;
using System.Globalization;
using Core.Primitive;

namespace Core.Time
{

    public sealed class Year
    {

        public const int MinYear = 0;
        public const int MaxYear = 9999;

        public static Year Now    => new Year(DateTime.Now.Year);

        public int HoursOfYear    => 24 * DayOfYear;
        public int MinituesOfYear => 60 * HoursOfYear;
        public int DayOfYear      => DateTime.IsLeapYear(_year) ? 366 : 365;

        public static int CheckRange(int year)
        {
            Checks.InRange(MinYear, MaxYear, year, $"{year} is out of range.");
            return year;
        }

        public static Year Of(int year)
        {
            CheckRange(year);
            return new Year(year);
        }

        public static Year Of(DateTime dateTime)
        {
            return dateTime.Kind == DateTimeKind.Utc 
                 ? new Year(TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local).Year) 
                 : new Year(dateTime.Year)
                 ;
        }

        public static Year Of(DateTimeOffset dateTime)
        {
            return new Year(dateTime.LocalDateTime.Year);
        }

        public Year Add(Year year)
        {
            return new Year(CheckRange(_year + year._year));
        }

        public Year Minus(Year year)
        {
            return new Year(CheckRange(_year - year._year));
        }

        public int GetYear()
        {
            return _year;
        }

        //Todo: Using Number Format
        public override string ToString()
        {
            return Strings.Of(_year);
        }

        private readonly int _year;

        private Year(int year)
        {
            _year = year;
        }

    }

}