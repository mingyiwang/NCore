using System;
using Core.Collection;
using NUnit.Framework;

namespace Core.Test.Collection {

    [TestFixture]
    public class BitsTest {

        [Test]
        public void TestBits()
        {
            Assert.AreEqual(int.MinValue, Bits.From(int.MinValue).ToInt());
            Assert.AreEqual(int.MaxValue, Bits.From(int.MaxValue).ToInt());
            for(int i = short.MinValue; i <= short.MaxValue; i++)
            {
                Assert.AreEqual(i, Bits.From(i).ToInt());
            }
        }

        [Test]
        public void TestNumberBinary() {
            Console.WriteLine(Bits.From(9));
            Console.WriteLine(9.1 - 0.1);
            Console.WriteLine(Bits.From((float)9));
            Console.WriteLine(Bits.From(-10.5f));
            Console.WriteLine(Bits.From(10.5f));

            Console.WriteLine(0.21f * 10.0f);
        }

    }
}