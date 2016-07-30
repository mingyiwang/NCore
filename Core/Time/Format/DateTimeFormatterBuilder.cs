using System;
using System.Text;

namespace Core.Time.Format
{

    public class DateTimeFormatterBuilder : IFormatProvider
    {

        private readonly StringBuilder _builder;

        public DateTimeFormatterBuilder()
        {
            _builder = new StringBuilder("");
        }

        public DateTimeFormatterBuilder Append()
        {
            return this;
        }


        public object GetFormat(Type formatType)
        {
            throw new NotImplementedException();
        }

    }

}