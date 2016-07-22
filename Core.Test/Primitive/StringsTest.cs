using System;
using Core.Primitive;
using Moq;
using NUnit.Framework;

namespace Core.Test.Primitive
{
    [TestFixture]
    public class StringsTest
    {

        [SetUp]
        public void SetUp()
        {
            
        }

        [TearDown]
        public void TearDown()
        {

        }

        [Test]
        [ExpectedException(typeof(IndexOutOfRangeException))]
        public void TestOne()
        {

        }

        [Test]
        [Ignore("Ignore Excepiton for now")]
        public void TestBetween()
        {
           Assert.AreEqual(Strings.Between("123",0,0), string.Empty);
           Assert.AreEqual(Strings.Between("123",0,1), string.Empty);
           Assert.AreEqual(Strings.Between("123",0,2), "2");
           Assert.AreEqual(Strings.Between("123",0,5), "2");
        }

    }
}