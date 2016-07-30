using System;
using Core.Primitive;

namespace Core.Time
{

    /// <summary>
    /// Year construct from number will be local year
    /// Year construct from TimeZoneInfo will be year in that zone
    /// Year construct from utc time will be converted to local year
    /// Year construct from offset time will be converted to local year
    /// </summary>
    public sealed class Year : IDateTime, IComparable<Year>, IComparable, IEquatable<Year>
    {

        public const int MinYear = 1;
        public const int MaxYear = 9999;

        /// <summary>
        /// Get Year in Local Time Zone
        /// </summary>
        public static Year Now
        {
            get
            {
                return new Year(DateTime.Now.Year);
            }
        }

        /// <summary>
        /// Get Year by numeric number of year
        /// </summary>
        /// <param name="year">Year</param>
        /// <returns>Year</returns>
        public static Year Of(int year)
        {
            CheckRange(year);
            return new Year(year);
        }

        /// <summary>
        /// Get Year of specified DateTime
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns>Year</returns>
        public static Year Of(DateTime dateTime)
        {
            return dateTime.Kind == DateTimeKind.Utc 
                 ? new Year(TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local).Year) 
                 : new Year(dateTime.Year)
                 ;
        }

        /// <summary>
        /// Get Year of specified DateTimeOffset
        /// </summary>
        /// <param name="dateTime">DateTimeOffset</param>
        /// <returns>Year</returns>
        public static Year Of(DateTimeOffset dateTime)
        {
            return new Year(dateTime.LocalDateTime.Year);
        }

        /// <summary>
        /// Check the numeric value of year is valid or not
        /// </summary>
        /// <param name="year">Year</param>
        /// <returns>Numeric value of Year</returns>
        public static int CheckRange(int year)
        {
            Checks.InRange(MinYear, MaxYear, year, $"{year} is out of range.");
            return year;
        }

        /// <summary>
        /// Get Total Mill Seconds of this Year
        /// </summary>
        public int TotalMillSeconds
        {
            get
            {
                return 1000 * TotalSeconds;
            }
        }

        /// <summary>
        /// Get Total Seconds of this Year
        /// </summary>
        public int TotalSeconds
        {
            get { return 60 * TotalMinitues; }
        }

        /// <summary>
        /// Get Total Minitues of this Year
        /// </summary>
        public int TotalMinitues
        {
            get {
                return 60 * TotalHours;
            }
        }

        /// <summary>
        /// Get Total Hours of This Year
        /// </summary>
        public int TotalHours
        {
            get
            {
                return 24 * TotalDays;
            }

        }

        /// <summary>
        /// Get Total Days of this Year
        /// </summary>
        public int TotalDays
        {
            get
            {
                return DateTime.IsLeapYear(_year) ? Dates.DaysPerLeapYear : Dates.DaysPerYear;
            }
        }

        /// <summary>
        /// Calculate Days From 1/1/0001 To the end of This Year
        /// </summary>
        public int TotalDaysToEndOfYear
        {
            get
            {
                return Dates.DaysPerYear  * _year
                    + (int) Math.Floor(_year * 0.25) 
                    - (int) Math.Floor(_year * 0.01)   // if can be divided by 100 then must be divided by 4
                    + (int) Math.Floor(_year * 0.0025) // if can be divided by 400 then must be divided by 100
                    ;
            }
        }


        /// <summary>
        /// Calculate Days From 1/1/0001 To the end of Previous Year
        /// </summary>
        public int TotalDaysBeforeStartOfYear
        {
            get
            {
                if (_year == 1)
                {
                    return 0;
                }
                return TotalDaysToEndOfYear - (DateTime.IsLeapYear(_year) ? Dates.DaysPerLeapYear : Dates.DaysPerYear);
            }
        }

        /// <summary>
        /// Check this Year is a Leap Year or Not
        /// </summary>
        public bool IsLeap
        {
            get
            {
                return DateTime.IsLeapYear(_year);
            }
        }


        /// <summary>
        /// Construct MonthOfYear by specified Month and this Year
        /// </summary>
        /// <param name="month">Month</param>
        /// <returns>MonthOfYear</returns>
        public MonthOfYear ForMonth(int month)
        {
            return MonthOfYear.Of(Month.Of(month), Of(_year));
        }

        /// <summary>
        /// Construct MonthOfYear by specified Month and this Year
        /// </summary>
        /// <param name="month">Month</param>
        /// <returns>MonthOfYear</returns>
        public MonthOfYear ForMonth(Month month)
        {
            return MonthOfYear.Of(Month.Of(month.GetMonth()), Of(_year));
        }

        
        public Year Add(Year year)
        {
            return new Year(CheckRange(_year + year._year));
        }

        public Year Minus(Year year)
        {
            return new Year(CheckRange(_year - year._year));
        }

        /// <summary>
        /// Get Numeric Value of This Year
        /// </summary>
        /// <returns>Year</returns>
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
            if(ReferenceEquals(obj, null))
            {
                return false;
            }

            if(ReferenceEquals(this, obj))
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