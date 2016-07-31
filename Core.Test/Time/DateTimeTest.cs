using System;
using Core.Time;
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

        }

        [Test]
        public void TestMonthOfYear()
        {
            
        }

        [Test]
        public void TestDaylightSaving()
        {
            
        }

        [Test]
        public void TestCalendar()
        {
            
        }

    }

}