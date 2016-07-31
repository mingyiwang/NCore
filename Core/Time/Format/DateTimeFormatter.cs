using System;
using System.Globalization;
using Core.Primitive;

namespace Core.Time.Format
{

    public class DateTimeFormatter
    {

        private readonly string _pattern;
        private readonly DateTimeFormatInfo _formatInfo;

        DateTimeFormatter(string pattern, DateTimeFormatInfo formatInfo)
        {
            _pattern = pattern;
            _formatInfo = formatInfo;
        }

        public static DateTimeFormatter Of(string pattern)
        {
            return new DateTimeFormatterBuilder()
                      .Append(pattern)
                      .GetFormatter();
        }

        public static DateTimeFormatter Of(DateTimeFormatterBuilder builder)
        {
            return builder.GetFormatter();
        }

        public string Format(DateTime dateTime)
        {
            return dateTime.ToString(_pattern);
        }

        public DateTime Parse(string dateTimeString)
        {
            DateTime result;
            var parsed = DateTime.TryParseExact(dateTimeString,
                            Strings.Of(_pattern, _formatInfo.LongDatePattern),
                            _formatInfo,
                            DateTimeStyles.AllowWhiteSpaces,
                            out result)
                            ;

            if (parsed)
            {
                return result;
            }

            throw new FormatException($"{dateTimeString} can not be parsed as DateTime Object");
        }

        public DateTime Parse(string dateTimeString, DateTime returnIfNotParsed)
        {
            DateTime result;
            var parsed = DateTime.TryParseExact(dateTimeString,
                            Strings.Of(_pattern, _formatInfo.LongDatePattern),
                            _formatInfo,
                            DateTimeStyles.AllowWhiteSpaces,
                            out result)
                            ;

            return !parsed ? returnIfNotParsed : result;
        }

    }

}
