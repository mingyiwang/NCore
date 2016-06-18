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
            Preconditions.CheckEquals(string.Empty,                Strings.Of(null, string.Empty));
            Preconditions.CheckEquals(int.MinValue.ToString(),     Strings.Of(int.MinValue));
            Preconditions.CheckEquals(int.MaxValue.ToString(),     Strings.Of(int.MaxValue));
            Preconditions.CheckEquals(double.MinValue.ToString(),  Strings.Of(double.MinValue));
            Preconditions.CheckEquals(double.MaxValue.ToString(),  Strings.Of(double.MaxValue));
            Preconditions.CheckEquals(long.MinValue.ToString(),    Strings.Of(long.MinValue));
            Preconditions.CheckEquals(long.MaxValue.ToString(),    Strings.Of(long.MaxValue));
            Preconditions.CheckEquals(float.MinValue.ToString(),   Strings.Of(float.MinValue));
            Preconditions.CheckEquals(float.MaxValue.ToString(),   Strings.Of(float.MaxValue));
            Preconditions.CheckEquals(decimal.MinValue.ToString(), Strings.Of(decimal.MinValue));
            Preconditions.CheckEquals(decimal.MaxValue.ToString(), Strings.Of(decimal.MaxValue));
            Preconditions.CheckEquals(true.ToString(),             Strings.Of(true));
            Preconditions.CheckEquals(false.ToString(),            Strings.Of(false));
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