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

        }

        [Test]
        public void TestBetween()
        {
            Console.WriteLine(Strings.Between("123", 0, 0));
        }

    }
}