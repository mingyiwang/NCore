using System;

namespace Core.Time
{

    public sealed class Month : IDateTime, IComparable<Month>, IComparable, IEquatable<Month>
    {

        public const int MaxDaysOfLeapYearMonth = 29;
        public const int MinDayOfMonth = 1;
        public const int MaxDayOfMonth = 31;

        public static readonly Month January   = new Month(1,  "January",   31, 31);
        public static readonly Month February  = new Month(2,  "February",  29, 28);
        public static readonly Month March     = new Month(3,  "March",     31, 31);
        public static readonly Month April     = new Month(4,  "April",     30, 30);
        public static readonly Month May       = new Month(5,  "May",       31, 31);
        public static readonly Month June      = new Month(6,  "June",      30, 30);
        public static readonly Month July      = new Month(7,  "July",      31, 31);
        public static readonly Month August    = new Month(8,  "August",    31, 31);
        public static readonly Month September = new Month(9,  "September", 30, 30);
        public static readonly Month October   = new Month(10, "October",   31, 31);
        public static readonly Month November  = new Month(11, "November",  30, 30);
        public static readonly Month December  = new Month(12, "December",  31, 31);

        private readonly string _name;
        private readonly int _month;
        private readonly int _daysOfLeapYear;
        private readonly int _days;
       
        private Month(int month, string name, int days, int daysOfLeapYear)
        {
            _name = name;
            _daysOfLeapYear = daysOfLeapYear;
            _days  = days;
            _month = month;
        }

        /// <summary>
        /// Construct Month in Loca Time Zone
        /// </summary>
        public static Month Now
        {
            get
            {
                return Of(DateTime.Now.Month);
            }
        }

        /// <summary>
        /// Construct <code>Month</code> by numberic Value of Month
        /// </summary>
        /// <param name="month">Integer Value of Month</param>
        /// <returns>Month</returns>
        public static Month Of(int month)
        {
            switch(month)
            {
                case 1 : return January;
                case 2 : return February;
                case 3 : return March;
                case 4 : return April;
                case 5 : return May;
                case 6 : return June;
                case 7 : return July;
                case 8 : return August;
                case 9 : return September;
                case 10: return October;
                case 11: return November;
                case 12: return December;
                default: throw new ArgumentOutOfRangeException($"Invalid Month [{month}]");
            }
        }

        public static Month Of(DateTime dateTime)
        {
            return dateTime.Kind == DateTimeKind.Utc
                 ? Of(TimeZoneInfo.ConvertTimeFromUtc(dateTime, TimeZoneInfo.Local).Month)
                 : Of(dateTime.Month);
        }

        /// <summary>
        /// Construct MonthOfYear by specified Year and This Month
        /// </summary>
        /// <param name="year"><code>int</code></param>
        /// <returns><code>MonthOfYear</code></returns>
        public MonthOfYear ForYear(int year)
        {
            return MonthOfYear.Of(Of(_month), Year.Of(year));
        }

        public MonthOfYear ForYear(Year year)
        {
            return MonthOfYear.Of(Of(_month), year);
        }

        /// <summary>
        /// Returns the english name of this month
        /// </summary>
        /// <returns>English Name of This Month</returns>
        public string GetName()
        {
            return _name;
        }

        /// <summary>
        /// Get the month-of-year
        /// The value is numbered based on ISO-8601 standard
        /// from January(1) to December(12)
        /// </summary>
        /// <returns>month</returns>
        public int GetMonth()
        {
            return _month;
        }

        /// <summary>
        /// Get days of this month of this year
        /// The value is numbered based on ISO-8601 standard
        /// from 1 to 31
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public int GetTotalDaysInYear(int year)
        {
            return DateTime.IsLeapYear(Year.CheckRange(year)) ? _daysOfLeapYear : _days;
        }

        /// <summary>
        /// Get Total Days From Start of This Year to The End of Previous Month
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public int GetDaysToStartOfMonthInYear(int year)
        {
            if (_month == 1)
            {
                return 0;
            }
            return GetDaysToEndOfMonthInYear(year) - GetTotalDaysInYear(year);
        }

        /// <summary>
        /// Get Total Days From the Start Day to End of this Month in Year
        /// </summary>
        /// <param name="year"><code>int</code></param>
        /// <returns><code>int</code></returns>
        public int GetDaysToEndOfMonthInYear(int year)
        {
            var sum = 0;
            for (var i = 1; i <= _month; i++)
            {
                sum += Of(i).GetTotalDaysInYear(year);
            }
            return sum;
        }

        /// <summary>
        /// Check Day is valid of this month in specified year
        /// </summary>
        /// <param name="day">Day of Month</param>
        /// <param name="year">Year</param>
        /// <returns></returns>
        public bool IsValid(int day, int year)
        {
            return day >= MinDayOfMonth && day <= GetTotalDaysInYear(year);
        }

        /// <summary>
        /// Check whether the day of this month is valid or not in this year
        /// </summary>
        /// <param name="day">day of month</param>
        /// <param name="year">year</param>
        /// <returns></returns>
        public int CheckRange(int day, int year)
        {
            var maxDays = GetTotalDaysInYear(year);
            Checks.InRange(MinDayOfMonth, maxDays, day,
                $"{day} of {GetName()} in Year [{year}] must be within {MaxDayOfMonth} to {maxDays}"
            );
            return day;
        }

        public int CompareTo(Month other)
        {
            return _month.CompareTo(other._month);
        }

        public int CompareTo(object obj)
        {
            Checks.NotNull(obj, "Argument can not be null.");
            return ReferenceEquals(this, obj) ? 0 : CompareTo(obj as Month);
        }

        public bool Equals(Month other)
        {
            return _month == other._month;
        }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var m = obj as Month;
            return m != null && Equals(m);
        }

        /// <summary>
        /// The same month should have the same hashcode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return _month;
        }


    }

}