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
            PreConditions.CheckEquals(string.Empty,                Strings.Of(null, string.Empty));
            PreConditions.CheckEquals(int.MinValue.ToString(),     Strings.Of(int.MinValue));
            PreConditions.CheckEquals(int.MaxValue.ToString(),     Strings.Of(int.MaxValue));
            PreConditions.CheckEquals(double.MinValue.ToString(),  Strings.Of(double.MinValue));
            PreConditions.CheckEquals(double.MaxValue.ToString(),  Strings.Of(double.MaxValue));
            PreConditions.CheckEquals(long.MinValue.ToString(),    Strings.Of(long.MinValue));
            PreConditions.CheckEquals(long.MaxValue.ToString(),    Strings.Of(long.MaxValue));
            PreConditions.CheckEquals(float.MinValue.ToString(),   Strings.Of(float.MinValue));
            PreConditions.CheckEquals(float.MaxValue.ToString(),   Strings.Of(float.MaxValue));
            PreConditions.CheckEquals(decimal.MinValue.ToString(), Strings.Of(decimal.MinValue));
            PreConditions.CheckEquals(decimal.MaxValue.ToString(), Strings.Of(decimal.MaxValue));
            PreConditions.CheckEquals(true.ToString(),             Strings.Of(true));
            PreConditions.CheckEquals(false.ToString(),            Strings.Of(false));
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