using System;

namespace Core.Time.Zone
{

    public struct ZonedDateTime : IComparable, IComparable<ZonedDateTime>
    {

        private TimeZoneInfo _zone;
        private DateTime _date;
        private TimeSpan _offset;
        
        public long Ticks      => _date.Ticks;
        public Year Year       => Year.Of(_date.Year);
        public Month Month     => Month.Of(_date.Month);
        public int Day         => _date.Day;
        public int Hour        => _date.Hour;
        public int Minitues    => _date.Minute;
        public int Seconds     => _date.Second;
        public int MillSeconds => _date.Millisecond;

        private ZonedDateTime(DateTime dateTime, TimeZoneInfo zone)
        {
            _date   = dateTime;
            _zone   = zone;
            _offset = zone.GetUtcOffset(dateTime);
        }

        public static ZonedDateTime Now
        {
            get
            {
                var offsetNow = DateTimeOffset.Now;
                var dateTime  = DateTime.SpecifyKind(offsetNow.LocalDateTime, DateTimeKind.Local);
                var zone      = TimeZoneInfo.Local;
                return new ZonedDateTime(dateTime, zone);
            }
        }

        public static ZonedDateTime Of(Year year, Month month, int day)
        {
            return Of(year, month, day, TimeZoneInfo.Local);
        }

        public static ZonedDateTime Of(Year year, Month month, int day, TimeZoneInfo zone)
        {
            return new ZonedDateTime(Dates.Of(year.GetYear(), month, day), zone);
        }

        public ZonedDateTime ToUniverslTime()
        {
            var utcTicks = _date.Ticks - _offset.Ticks;
            var utc = new DateTime(utcTicks, DateTimeKind.Utc);
            return new ZonedDateTime(utc, TimeZoneInfo.Utc);
        }

        public ZonedDateTime ToZonedDateTime(TimeZoneInfo zone)
        {
            var utcTicks = _date.Ticks - _offset.Ticks;
            var ticks = utcTicks + zone.BaseUtcOffset.Ticks;
            var local = new DateTime(ticks, DateTimeKind.Local);
            return new ZonedDateTime(local, zone);
        }

        public int CompareTo(ZonedDateTime other)
        {
            return 0;
        }

        public int CompareTo(object obj)
        {
            return 0;
        }

        public bool Equals(ZonedDateTime other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            return true;
        }

        public override int GetHashCode()
        {
            return 0;
        }


    }



}