using System;
using Core.Primitive;
using NUnit.Framework;

namespace Core.Test.Primitive
{

    [TestFixture]
    public class BitsTest
    {

        [Test]
        public void TestBits()
        {
            Console.WriteLine(Bits.Of(true).ToIntBits());
            Console.WriteLine(Bits.Of(1.5d).ToIntBits());

            Console.WriteLine(Bits.Of(decimal.MaxValue).ToIntBits());
            Console.WriteLine(Bits.Of(decimal.MinValue).ToIntBits());

            Console.WriteLine(Bits.Of(long.MaxValue).ToIntBits());
            Console.WriteLine(Bits.Of(long.MinValue).ToIntBits());
        }


    }
}