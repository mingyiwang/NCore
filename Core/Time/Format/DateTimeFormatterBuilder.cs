using System.Globalization;
using Core.Primitive;

namespace Core.Time.Format
{

    public class DateTimeFormatterBuilder
    {

        public string Pattern { get; private set; }
        private DateTimeFormatInfo cultureInfo;

        public DateTimeFormatterBuilder()
        {

        }

        public string GetPattern()
        {
            return Pattern;
        }

        public DateTimeFormatterBuilder Append(Symbol symbol)
        {
            return this;
        }

        public DateTimeFormatterBuilder Append(string pattern)
        {
            Pattern = pattern;
            return this;
        }

        public DateTimeFormatterBuilder Append(DateTimePattern pattern)
        {
            return this;
        }

        public DateTimeFormatterBuilder With(CultureInfo info)
        {
            cultureInfo = DateTimeFormatInfo.GetInstance(info);
            return this;
        }

        public DateTimeFormatter GetFormatter()
        {
            
            return DateTimeFormatter.Of(this);
        }

        public override string ToString()
        {
            return string.Empty;
        }


    }

}