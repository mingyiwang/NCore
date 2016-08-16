using System;
using Core.Primitive;
using NUnit.Framework;

namespace Core.Test.Primitive {

    [TestFixture]
    public class NumbersTest {

        [Test]
        public void TestFloat()
        {
            Console.WriteLine(decimal.ToInt32(4.5m));
            Console.WriteLine(decimal.ToInt32(-4.5m));
            Console.WriteLine(Convert.ToInt32(-4.5m));

            Console.WriteLine(Bits.Of(1.5m).ToDecimal());
            Console.WriteLine(Bits.Of(129.95f * 10.0f).ToDouble());
            Console.WriteLine(Bits.Of(129.95f).ToBinaryString());

        }

    }
}