using System;
using Core.Primitive;
using NUnit.Framework;

namespace Core.Test.Primitive {

    [TestFixture]
    public class NumbersTest {

        [Test]
        public void TestFloat()
        {

           Console.WriteLine(Bits.Of(-2L).ToBinaryString());
           Console.WriteLine(Convert.ToString(-2L, 2));
           Console.WriteLine(Bits.Of(-2).ToBinaryString());
           Console.WriteLine(Convert.ToString(-2, 2));
           
           Console.WriteLine(Bits.Of((long)int.MinValue).ToBinaryString());

            
        }

    }
}