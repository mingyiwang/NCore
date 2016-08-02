using System;
using Core.Time;
using Core.Time.Format;
using Core.Time.Zone;
using NUnit.Framework;

namespace Core.Test.Time
{

    [TestFixture]
    public class DateTimeTest
    {
        [Test]
        public void TestYear()
        {
            Console.WriteLine(Year.Of(1970).TotalDaysBeforeStartOfYear);
            Console.WriteLine(Year.Of(1970).TotalDaysBeforeStartOfYear * Dates.DaysPerYear);

            Console.WriteLine(Dates.Epoch);
            Console.WriteLine(Dates.FirstDay.AddHours(23));
        }

        [Test]
        public void TestMonth()
        {
            DateTimeFormatter.Of("").Parse("");
            DateTimeFormatter.Of("").Format(DateTime.Now);
        }

        [Test]
        public void TestMonthOfYear()
        {
            
        }

        [Test]
        public void TestDaylightSaving()
        {
            Console.WriteLine(
               DayLightOfYear.Of(Year.Of(2017)).InvalidTimeStart
            );
        }

        [Test]
        public void TestFormat()
        {
            var builder = new DateTimeFormatterBuilder();
            var dateString = builder.Append('-')
                                    .Append(DateTimeFormatToken.Day)
                                    .GetFormatter()
                                    .Format(DateTime.Now);

            Console.WriteLine(dateString);
            Console.WriteLine(DateTimeFormatter.YYYY_MM_DD.Format(DateTime.Now));
            Console.WriteLine(DateTimeFormatter.YYYY_S_MM_S_DD.Format(DateTime.Now));
            Console.WriteLine(DateTimeFormatter.IsoDate.Format(DateTime.Now));
            Console.WriteLine(DateTimeFormatter.IsoDateTime.Format(DateTime.Now));
            Console.WriteLine(DateTimeFormatter.IsoTime.Format(DateTime.Now));
        }

    }

}