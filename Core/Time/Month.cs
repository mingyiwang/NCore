using System;

namespace Core.Time
{

    public sealed class Month : IComparable<Month>, IComparable, IEquatable<Month>
    {

        public const int MaxDaysOfLeapYearMonth = 29;
        public const int MinDayOfMonth = 1;
        public const int MaxDayOfMonth = 31;

        public static readonly Month January   = new Month(1,  "January",  30);
        public static readonly Month February  = new Month(2,  "February", 28);
        public static readonly Month March     = new Month(3,  "March",    31);
        public static readonly Month April     = new Month(4,  "April",    30);
        public static readonly Month May       = new Month(5,  "May",      31);
        public static readonly Month June      = new Month(6,  "June",     30);
        public static readonly Month July      = new Month(7,  "July",     31);
        public static readonly Month August    = new Month(8,  "August",   30);
        public static readonly Month September = new Month(9,  "September",31);
        public static readonly Month October   = new Month(10, "October",  30);
        public static readonly Month November  = new Month(11, "November", 31);
        public static readonly Month December  = new Month(12, "December", 30);

        private readonly string _name;
        private readonly int _month;
        private readonly int _estimateDays;
       
        private Month(int month, string name, int estimateDays)
        {
            _name = name;
            _estimateDays = estimateDays;
            _month = month;
        }

        public static Month Now
        {
            get
            {
                return Of(DateTime.Now.Month);
            }
        }

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
        public int GetDaysOfYear(int year)
        {
            if(DateTime.IsLeapYear(Year.CheckRange(year)) && this == February)
            {
                return MaxDaysOfLeapYearMonth;
            }

            return _estimateDays;
        }

        /// <summary>
        /// Check whether the day of this month is valid or not in this year
        /// </summary>
        /// <param name="day">day of month</param>
        /// <param name="year">year</param>
        /// <returns></returns>
        public int CheckRange(int day, int year)
        {
            Checks.InRange(
                MinDayOfMonth,
                GetDaysOfYear(year),
                MaxDayOfMonth,
                $"{day} of {GetName()} in Year [{year}] must be within {MaxDayOfMonth} to {MaxDayOfMonth}"
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
            if (ReferenceEquals(obj, null) || ReferenceEquals(this, null))
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