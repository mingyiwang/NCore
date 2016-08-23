using System;
using Core.Primitive;
using NUnit.Framework;

namespace Core.Test.Primitive {

    [TestFixture]
    public class NumbersTest {

        [Test]
        public void TestFloat()
        {
           
            Console.WriteLine("4.5m => "  + decimal.ToInt32(4.5m));
            Console.WriteLine("4.6m => "  + decimal.ToInt32(4.6m));
            Console.WriteLine("4.3m => "  + decimal.ToInt32(4.3m));
            Console.WriteLine("4m => "    + decimal.ToInt32(4m));
            Console.WriteLine("0m => "    + decimal.ToInt32(0m));
            Console.WriteLine("-4.5m => " + decimal.ToInt32(-4.5m));
            Console.WriteLine("-4.6m => " + decimal.ToInt32(-4.6m));
            Console.WriteLine("-4.3m => " + decimal.ToInt32(-4.3m));

            Console.WriteLine(int.MaxValue);
            Console.WriteLine(float.MaxValue);
            Console.WriteLine(double.MaxValue);
            Console.WriteLine(long.MaxValue);

        }

    }
}