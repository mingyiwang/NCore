using System;
using Core.Primitive;
using NUnit.Framework;

namespace Core.Test.Primitive
{
    [TestFixture]
    public class StringsTest
    {
        [Test]
        public void TestStringOf()
        {
            Check.Equals(string.Empty,                Strings.Of(null, string.Empty));
            Check.Equals(int.MinValue.ToString(),     Strings.Of(int.MinValue));
            Check.Equals(int.MaxValue.ToString(),     Strings.Of(int.MaxValue));
            Check.Equals(double.MinValue.ToString(),  Strings.Of(double.MinValue));
            Check.Equals(double.MaxValue.ToString(),  Strings.Of(double.MaxValue));
            Check.Equals(long.MinValue.ToString(),    Strings.Of(long.MinValue));
            Check.Equals(long.MaxValue.ToString(),    Strings.Of(long.MaxValue));
            Check.Equals(float.MinValue.ToString(),   Strings.Of(float.MinValue));
            Check.Equals(float.MaxValue.ToString(),   Strings.Of(float.MaxValue));
            Check.Equals(decimal.MinValue.ToString(), Strings.Of(decimal.MinValue));
            Check.Equals(decimal.MaxValue.ToString(), Strings.Of(decimal.MaxValue));
            Check.Equals(true.ToString(),             Strings.Of(true));
            Check.Equals(false.ToString(),            Strings.Of(false));
        }

        [Test]
        public void TestIntOf()
        {
            Console.WriteLine(Convert.ToBoolean(0));
        }

        [Test]
        public void TestDoubleOf()
        {
            Console.WriteLine(Convert.ToInt32(25.499999));
        }

        [Test]
        public void TestBetween()
        {
            Console.WriteLine(Strings.Between("123", 0, 0));
        }

    }
}