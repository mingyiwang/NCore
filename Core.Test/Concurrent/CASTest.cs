using System;
using Core.Collection;
using Core.Concurrent;
using NUnit.Framework;

namespace Core.Test.Concurrent
{
    [TestFixture]
    public class CASTest
    {
        private readonly static ReadWriteList<string> SucceedList = new ReadWriteList<string>();
        private readonly static ReadWriteList<string> FailedList = new ReadWriteList<string>();

        [Test]
        public void TestCAS()
        {
            A a = null;
            A b = new A(30);

            var count = Tests.Test(20, () => {
                CAS.Set(ref a, b);
            });

            Console.WriteLine(count);
            Assert.AreEqual(a, b);

        }

        [Test]
        public void TestReference()
        {
            A tail = new A(1);
            A t;
            var p = t = tail;

            Tests.Test(10, () => {
                CAS.Set(ref tail, new A(2));
            });

            Console.WriteLine(ReferenceEquals(t, tail));
            Console.WriteLine(ReferenceEquals(p, t));
        }

        class A
        {
            private int Value { get; set; }

            public A(int val)
            {
                Value = val;
            }

        }


    }
}