using System;
using System.Linq;
using Castle.Core.Internal;
using Core.Time;
using NUnit.Framework;

namespace Core.Test.Time
{

    [TestFixture]
    public class TimeZoneTest
    {

        [Test]
        public void TestDaylightSaving()
        {
            
//            var rules = TimeZones.AuEast.Value.GetAdjustmentRules().ToList();
//            foreach (var rule in rules)
//            {
//                Console.WriteLine("-------- Rule ------------------------------");
//                Console.WriteLine($"{rule.DateStart} To {rule.DateEnd} For {rule.DaylightDelta.Hours}");
//                Console.WriteLine("--------Transition Start ------------------------------");
//                PrintTransitionTime(rule.DaylightTransitionStart);
//
//                Console.WriteLine("--------Transition End ------------------------------");
//                PrintTransitionTime(rule.DaylightTransitionEnd);
//
//            }

            Enumerable.Range(1, 3000).ForEach(i =>
            {
                var dst = TimeZones.GetDaylightTime(TimeZones.AuEast.Value, 2006);
                Console.WriteLine(dst.Start);
                Console.WriteLine(dst.End);
            });

        }

        private void PrintTransitionTime(TimeZoneInfo.TransitionTime time)
        {
            Console.WriteLine($"IsFixedRate -> {time.IsFixedDateRule}");
            Console.WriteLine($"DayOfWeek ->   {time.TimeOfDay.ToShortTimeString()} {time.DayOfWeek} the {time.Week}th Week Of { Month.Of(time.Month).Name()}");
        }

    }

}