using System;
using Core.Primitive;
using NUnit.Framework;

namespace Core.Test.Primitive {

    [TestFixture]
    public class NumbersTest {

        [Test]
        public void TestFloat()
        {
           
            Console.WriteLine("4.5m => "  + (int)4.5m  + decimal.ToInt32(4.5m)  + " " + Convert.ToInt32(4.5m)   + " " + Numbers.GetInt(4.5m,   RoundKind.HalfDown));
            Console.WriteLine("4.6m => "  + (int)4.6m  + decimal.ToInt32(4.6m)  + " " + Convert.ToInt32(4.6m)   + " " + Numbers.GetInt(4.6m,   RoundKind.HalfDown));
            Console.WriteLine("4.8m => "  + (int)4.8m  + decimal.ToInt32(4.8m)  + " " + Convert.ToInt32(4.8m)   + " " + Numbers.GetInt(4.8m,   RoundKind.HalfDown));
            Console.WriteLine("4.3m => "  + (int)4.3m  + decimal.ToInt32(4.3m)  + " " + Convert.ToInt32(4.3m)   + " " + Numbers.GetInt(4.3m,   RoundKind.HalfDown));
            Console.WriteLine("4m => "    + (int)4m    + decimal.ToInt32(4m)    + " " + Convert.ToInt32(4m)     + " " + Numbers.GetInt(4m,     RoundKind.HalfDown));
            Console.WriteLine("0m => "    + (int)0m    + decimal.ToInt32(0m)    + " " + Convert.ToInt32(0m)     + " " + Numbers.GetInt(0m,     RoundKind.HalfDown));
            Console.WriteLine("-4.5m => " + (int)-4.5m + decimal.ToInt32(-4.5m) + " " + Convert.ToInt32(-4.5m)  + " " + Numbers.GetInt(-4.5m,  RoundKind.HalfDown));
            Console.WriteLine("-4.6m => " + (int)-4.6m + decimal.ToInt32(-4.6m) + " " + Convert.ToInt32(-4.6m)  + " " + Numbers.GetInt(-4.6m,  RoundKind.HalfDown));
            Console.WriteLine("-4.3m => " + (int)-4.3m + decimal.ToInt32(-4.3m) + " " + Convert.ToInt32(-4.3m)  + " " + Numbers.GetInt(-4.3m,  RoundKind.HalfDown));

           
            //Console.WriteLine(decimal.ToInt32(decimal.MaxValue));
            var f = -1.5f;
            var f2 = 1.5f;
            var z = 0f;
            Console.WriteLine(f.IsNegative());
            Console.WriteLine(f2.IsNegative());
            Console.WriteLine(z.IsNegative());
            Console.WriteLine(Bits.Of(f).ToIntBits());
            Console.WriteLine(Convert.ToString(0x80000000, 2));

            Console.WriteLine(int.MaxValue);
            Console.WriteLine(int.MinValue);
        }

    }
}