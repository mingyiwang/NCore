using System;

namespace Core.Time.Format
{

    public class DateTimeFormatter
    {
        private DateTimeFormatter(){}

        public string Format(DateTime dateTime)
        {
            return dateTime.ToString("");
        }

        public DateTime Parse(string dateTimeString, DateTime returnIfNull)
        {
            DateTime result;
            
            return returnIfNull;
        }

    }

}
