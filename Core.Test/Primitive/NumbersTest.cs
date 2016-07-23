using System;
using System.Globalization;
using NUnit.Framework;

namespace Core.Test.Primitive {

    [TestFixture]
    public class NumbersTest {

        [Test]
        public void TestDouble()
        {
            var dataTime = new DateTime();
            var dataTimeOffset = new DateTimeOffset(dataTime);
            var calendar = new GregorianCalendar();
            Console.WriteLine("Is Day Light Saving => " + TimeZoneInfo.Local.IsDaylightSavingTime(DateTime.Now.AddMonths(-5)));
            Console.WriteLine("Is Ambigous => " + TimeZoneInfo.Local.IsAmbiguousTime(DateTime.Now.AddMonths(-5)));
                
        }

    }
}