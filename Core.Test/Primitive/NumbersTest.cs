using System;
using NUnit.Framework;

namespace Core.Test.Primitive {

    [TestFixture]
    public class NumbersTest {

        [Test]
        public void TestDouble()
        {
            decimal d1 = new decimal(1.0);
            decimal d2 = new decimal(2.0);

            Console.WriteLine((d1 / d2) * d2 == d1);

            Console.WriteLine((1.0 / 103.0) * 103.0 < 1.0);
        }

    }
}