using System;

namespace Core.Time
{

    public class Month
    {
        
        public static readonly Month January   = new Month(1,  "January",   30);
        public static readonly Month February  = new Month(2,  "February",  28);
        public static readonly Month March     = new Month(3,  "March",     31);
        public static readonly Month April     = new Month(4,  "April",     30);
        public static readonly Month May       = new Month(5,  "May",       31);
        public static readonly Month June      = new Month(6,  "June",      30);
        public static readonly Month July      = new Month(7,  "July",      31);
        public static readonly Month August    = new Month(8,  "August",    30);
        public static readonly Month September = new Month(9,  "September", 31);
        public static readonly Month October   = new Month(10, "October",   30);
        public static readonly Month November  = new Month(11, "November",  31);
        public static readonly Month December  = new Month(12, "December",  30);
        

        private readonly int _month;
        private readonly int _days;
        private readonly string _name;

        private Month()
        {
        }

        private Month(int month, string name, int days)
        {
            _name  = name;
            _days  = days;
            _month = month;
        }

        public static Month Of(int month)
        {
            switch (month)
            {
               case 1  : return January;
               case 2  : return February;
               case 3  : return March;
               case 4  : return April;
               case 5  : return May;
               case 6  : return June;
               case 7  : return July;
               case 8  : return August;
               case 9  : return September;
               case 10 : return October;
               case 11 : return November;
               case 12 : return December;
               default : throw new ArgumentOutOfRangeException($"Invalid Month {month}");
            }
        }

        public string Name()
        {
            return _name;
        }

        public int GetMonth()
        {
            return _month;
        }

        public int GetDays(int year)
        {
            if (DateTime.IsLeapYear(year) && this == February)
            {
                return Dates.DaysOfLeapYearMonth;
            }

            return _days;
        }

    }

}