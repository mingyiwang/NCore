using System;
using System.Globalization;
using System.Linq;
using NUnit.Framework;

namespace Core.Test.Primitive {

    [TestFixture]
    public class NumbersTest {

        [Test]
        public void TestDouble()
        {

            var dateTime = new DateTime(2016, 10, 2, 2, 0, 0);
            var dateTimeOffset = new DateTimeOffset(dateTime);
            
            Console.WriteLine(dateTime);
            Console.WriteLine(TimeZoneInfo.Local.IsDaylightSavingTime(dateTime));
            Console.WriteLine(TimeZoneInfo.Local.IsAmbiguousTime(dateTime));
            Console.WriteLine(TimeZoneInfo.Local.IsInvalidTime(dateTime));
            Console.WriteLine(dateTime.ToUniversalTime());

            var adjustRule = TimeZoneInfo.Local.GetAdjustmentRules()
                .Single(rule => rule.DateStart <= dateTime.Date && rule.DateEnd >= dateTime.Date);

            if (adjustRule != null)
            {
                // There is no 2:00 am in nsw sencond of octobor
                // There are repeating times between 2:00 to 3:00 am on second of april
                Console.WriteLine($"Delta      => {adjustRule.DaylightDelta}");
                Console.WriteLine($"Start      => {adjustRule.DateStart}");
                Console.WriteLine($"End        => {adjustRule.DateEnd}");
                Console.WriteLine($"Start Time => {adjustRule.DaylightTransitionStart.DayOfWeek}");
                Console.WriteLine($"End Time   => {adjustRule.DaylightTransitionEnd.DayOfWeek}");

            }
        }

    }
}