using System;
using Core.Primitive;
using NUnit.Framework;

namespace Core.Test.Primitive {

    [TestFixture]
    public class NumbersTest {

        [Test]
        public void TestFloat()
        {
           Console.WriteLine(Numbers.GetInt(-4.3f, RoundKind.Floor));
           Console.WriteLine(Numbers.GetInt(4.3f, RoundKind.Floor));
           Console.WriteLine(Math.Floor(-4.3f));
           Console.WriteLine(Math.Floor(4.3f));

           Console.WriteLine(Numbers.GetInt(-4.3f, RoundKind.Ceil));
           Console.WriteLine(Numbers.GetInt(4.3f,  RoundKind.Ceil));
           Console.WriteLine(Math.Ceiling(-4.3f));
           Console.WriteLine(Math.Ceiling(4.3f));
        }

    }
}