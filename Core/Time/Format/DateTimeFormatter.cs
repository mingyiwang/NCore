using System;
using System.Globalization;
using Core.Primitive;

namespace Core.Time.Format
{

    public sealed class DateTimeFormatter
    {

        public static DateTimeFormatter IsoDate
        {
            get
            {
                return Of("yyyy-MM-dd");
            }
        }

        public static DateTimeFormatter IsoTime
        {
            get {
                return Of("hh:mm:ss");
            }
        }

        public static DateTimeFormatter IsoDateTime
        {
            get {
                return Of("yyyy-MM-ddThh:mm:ss");
            }
        }

        public static DateTimeFormatter YYYY_MM_DD
        {
            get
            {
               return new DateTimeFormatterBuilder()
                    .Append(DateTimeFormatToken.Year.WithKind(4))
                    .Append('_')
                    .Append(DateTimeFormatToken.Month.WithKind(2))
                    .Append('_')
                    .Append(DateTimeFormatToken.Day.WithKind(2))
                    .GetFormatter();
            }
        }

        public static DateTimeFormatter YYYY_S_MM_S_DD
        {
            get {
                return new DateTimeFormatterBuilder()
                     .Append(DateTimeFormatToken.Year.WithKind(4))
                     .Append('/')
                     .Append(DateTimeFormatToken.Month.WithKind(2))
                     .Append('/')
                     .Append(DateTimeFormatToken.Day.WithKind(2))
                     .GetFormatter();
            }
        }

        private readonly CultureInfo _formatInfo;
        private readonly string _pattern;
        
        internal DateTimeFormatter(string pattern, CultureInfo cultureInfo)
        {
            _formatInfo = cultureInfo;
            _pattern = pattern;
        }

        public static DateTimeFormatter Of(string pattern)
        {
            return new DateTimeFormatter(pattern, CultureInfo.CurrentCulture);
        }

        public string Format(DateTime dateTime)
        {
            return dateTime.ToString(_pattern);
        }

        public DateTime Parse(string dateTimeString)
        {
            DateTime result;
            var parsed = DateTime.TryParseExact(dateTimeString,
                         Strings.Of(_pattern, _formatInfo.DateTimeFormat.FullDateTimePattern),
                         _formatInfo,
                         DateTimeStyles.AllowWhiteSpaces,
                         out result);
            if (parsed)
            {
                return result;
            }

            throw new FormatException($"{dateTimeString} can not be parsed as DateTime Object");
        }

        public DateTime Parse(string dateTimeString, DateTime returnIfNotParsed)
        {
            if (string.IsNullOrEmpty(dateTimeString))
            {
                return returnIfNotParsed;
            }

            DateTime result;
            var parsed = DateTime.TryParseExact(dateTimeString,
                         Strings.Of(_pattern, _formatInfo.DateTimeFormat.FullDateTimePattern),
                         _formatInfo,
                         DateTimeStyles.AllowWhiteSpaces,
                         out result);

            return !parsed ? returnIfNotParsed : result;
        }

    }

}
