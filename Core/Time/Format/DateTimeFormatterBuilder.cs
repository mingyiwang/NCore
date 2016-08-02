using System;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text;
using Core.Collection;

namespace Core.Time.Format
{

    public sealed class DateTimeFormatterBuilder : IDisposable
    {

        private readonly ConcurrentQueue<DateTimeFormatToken> _tokens;
        private CultureInfo _cultureInfo;
        
        public DateTimeFormatterBuilder()
        {
            _tokens = new ConcurrentQueue<DateTimeFormatToken>();
        }

        public DateTimeFormatterBuilder Append(char literal)
        {
            Checks.NotNull(literal);
            _tokens.Enqueue(DateTimeFormatToken.Of(literal));
            return this;
        }

        public DateTimeFormatterBuilder Append(DateTimeFormatToken token)
        {
            Checks.NotNull(token);
            _tokens.Enqueue(token);
            return this;
        }

        public DateTimeFormatterBuilder With(CultureInfo info)
        {
            Checks.NotNull(info);
            _cultureInfo = info;
            return this;
        }

        public DateTimeFormatter GetFormatter()
        {
            if (_cultureInfo == null)
            {
                _cultureInfo = CultureInfo.CurrentCulture;
            }
            return new DateTimeFormatter(ToString(), _cultureInfo);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            DateTimeFormatToken token;
            while (_tokens.TryDequeue(out token))
            {
                builder.Append(token.GetCode());
            }
            return builder.ToString();
        }

        public void Dispose()
        {
            try
            {
                _cultureInfo = null;
                _tokens.Clear();
            }
            catch
            {
                //
            }
        }

    }

}