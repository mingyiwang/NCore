using System.Text;

namespace Core.Time.Format
{

    public class DateTimeFormatterBuilder
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

        
    }

}