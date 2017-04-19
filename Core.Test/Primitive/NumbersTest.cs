using System;
using Core.Primitive;
using NUnit.Framework;

namespace Core.Test.Primitive {

    [TestFixture]
    public class NumbersTest {

        [Test]
        public void TestFloat()
        {
                     
            Console.WriteLine(Numbers.GetInt(-1.5f, RoundKind.HalfEven));

        }

    }
}