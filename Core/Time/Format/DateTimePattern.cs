namespace Core.Time.Format
{

    public class DateTimePattern
    {

        public string Pattern { get; private set; }

        private DateTimePattern(string pattern)
        {
            Pattern = pattern;
        }

        public DateTimePattern Year;
        public DateTimePattern Month;
        public DateTimePattern DayOfMonth;
        public DateTimePattern Hour;
        public DateTimePattern Minitues;
        public DateTimePattern Seconds;
        public DateTimePattern MillSeconds;
        public DateTimePattern Era;
        public DateTimePattern TimeZone;


    }

}