using System;

namespace Core.Time
{

    public class DateTimeRange
    {

        private readonly long _from;
        private readonly long _to;

        private DateTimeRange(long from, long to)
        {
            _from = from;
            _to = to;
        }

        public static DateTimeRange Of(long from, long to)
        {
            return new DateTimeRange(from, to);
        }

        public static DateTimeRange Of(DateTime from, DateTime to)
        {
            var fromUtcTicks = from.ToUniversalTime().Ticks;
            var toUtcTicks   = to.ToUniversalTime().Ticks;
            if (toUtcTicks < fromUtcTicks)
            {
                throw new ArgumentOutOfRangeException();
            }

            return new DateTimeRange(fromUtcTicks, toUtcTicks);
        }

        public DateTime From
        {
            get
            {
                return DateTime.Now;
            }
        }

        public bool Includes(DateTime time)
        {
            return time.ToUniversalTime().Ticks <= _to && time.ToUniversalTime().Ticks >= _from;
        }

    }
}