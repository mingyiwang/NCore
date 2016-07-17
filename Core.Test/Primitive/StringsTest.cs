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
            Checking.CheckEquals(string.Empty,                Strings.Of(null, string.Empty));
            Checking.CheckEquals(int.MinValue.ToString(),     Strings.Of(int.MinValue));
            Checking.CheckEquals(int.MaxValue.ToString(),     Strings.Of(int.MaxValue));
            Checking.CheckEquals(double.MinValue.ToString(),  Strings.Of(double.MinValue));
            Checking.CheckEquals(double.MaxValue.ToString(),  Strings.Of(double.MaxValue));
            Checking.CheckEquals(long.MinValue.ToString(),    Strings.Of(long.MinValue));
            Checking.CheckEquals(long.MaxValue.ToString(),    Strings.Of(long.MaxValue));
            Checking.CheckEquals(float.MinValue.ToString(),   Strings.Of(float.MinValue));
            Checking.CheckEquals(float.MaxValue.ToString(),   Strings.Of(float.MaxValue));
            Checking.CheckEquals(decimal.MinValue.ToString(), Strings.Of(decimal.MinValue));
            Checking.CheckEquals(decimal.MaxValue.ToString(), Strings.Of(decimal.MaxValue));
            Checking.CheckEquals(true.ToString(),             Strings.Of(true));
            Checking.CheckEquals(false.ToString(),            Strings.Of(false));
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

       

    }
}