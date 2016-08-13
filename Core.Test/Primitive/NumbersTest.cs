using System;
using Core.Primitive;
using NUnit.Framework;

namespace Core.Test.Primitive {

    [TestFixture]
    public class NumbersTest {

        [Test]
        public void TestDouble()
        {
           
//           Console.WriteLine(0xffffff);
//           Console.WriteLine(Bits.Of(16777215f).ToBinaryString());
//           Console.WriteLine(Numbers.ExponentOf(Convert.ToSingle(16777215f)));
//
//           Console.WriteLine(Bits.Of(16777216f).ToBinaryString());
//           Console.WriteLine(Numbers.ExponentOf(16777216f));
//
//           Console.WriteLine(Bits.Of(16777217f).ToBinaryString());
//           Console.WriteLine(Numbers.ExponentOf(16777217f));
//
//           Console.WriteLine(Bits.Of(float.Parse("16777216")).ToBinaryString());
//           Console.WriteLine(decimal.Parse("16777217"));
//
//           Console.WriteLine(Bits.Of((double)999999.845f).ToBinaryString());
//           Console.WriteLine((double)999999.845f);
//           Console.WriteLine(999999.845f);
            // 

//           Console.WriteLine(Bits.Of(5.34375f).ToBinaryString(ByteOrder.HigherFirst));
//           Console.WriteLine(Bits.Of(534375).ToBinaryString(ByteOrder.HigherFirst));
//           Console.WriteLine(DoubleConverter.ToExactString(5.34375d));
           
           Console.WriteLine("Signif Bit   => " + Bits.Of(8388607).ToBinaryString(ByteOrder.HigherFirst));
           Console.WriteLine("Exponent Bit => " + Bits.Of(2139095040).ToBinaryString(ByteOrder.HigherFirst));
           Console.WriteLine(0x7fc00000);
           Console.WriteLine(int.MaxValue);
           Console.WriteLine(Bits.Of(0x7fc00000).ToBinaryString(ByteOrder.HigherFirst));
           Console.WriteLine("NAN => " + Bits.Of(float.NaN).ToBinaryString(ByteOrder.HigherFirst));
           Console.WriteLine("Positve  Infinity  => " + Bits.Of(float.PositiveInfinity).ToBinaryString(ByteOrder.HigherFirst));
           Console.WriteLine("Negative Infinity => " + Bits.Of(float.NegativeInfinity).ToBinaryString(ByteOrder.HigherFirst));
           Console.WriteLine("Max => " + Bits.Of(float.MaxValue).ToBinaryString(ByteOrder.HigherFirst));
           Console.WriteLine("Min => " + Bits.Of(float.MinValue).ToBinaryString(ByteOrder.HigherFirst));

           Console.WriteLine("Exponent => " + Numbers.ExponentOf(float.NaN));
           Console.WriteLine((int) (1.6));
           
        }

    }
}