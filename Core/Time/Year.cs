using System;
using Core.Primitive;

namespace Core.Time
{

    public sealed class Year : IComparable<Year>, IComparable, IEquatable<Year>
    {

        public const int MinYear = 1;
        public const int MaxYear = 9999;

        public static Year Now
        {
            get
            {
                return new Year(DateTime.Now.Year);
            }
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

        public static Year Of(TimeZoneInfo zone)
        {
            return Of(TimeZoneInfo.ConvertTime(DateTime.Now, zone));
        }

        public static int CheckRange(int year)
        {
            Checks.InRange(MinYear, MaxYear, year, $"{year} is out of range.");
            return year;
        }

        public int HoursOfYear
        {
            get
            {
                return 24* DaysOfYear;
            }

        }

        public int MinituesOfYear
        {
            get
            {
                return 60*HoursOfYear;
            }
        }

        public int DaysOfYear
        {
            get
            {
                return DateTime.IsLeapYear(_year) ? Dates.DaysOfLeapYear : Dates.DaysOfYear;
            }
        }

        public bool IsLeap
        {
            get
            {
                return DateTime.IsLeapYear(_year);
            }
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

        public bool IsAfter(Year year)
        {
            return _year > year._year;
        }

        public bool IsBefore(Year year)
        {
            return _year < year._year;
        }

        public int CompareTo(Year other)
        {
            return _year.CompareTo(other._year);
        }

        public int CompareTo(object obj)
        {
            Checks.NotNull(obj);
            return ReferenceEquals(this, obj) ? 0 : CompareTo(obj as Year);
        }

        public bool Equals(Year other)
        {
            return _year == other._year;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null) || ReferenceEquals(this, null))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var y = obj as Year;
            return y != null && Equals(y);
        }

        public override string ToString()
        {
            return Strings.Of(_year);
        }

        /// <summary>
        /// The same year should have the same hashcode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _year;
        }

        private readonly int _year;

        private Year(int year)
        {
            _year = year;
        }

    }

}