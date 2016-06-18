using Core.Concurrent;
using NUnit.Framework;

namespace Core.Test.Concurrent {

    [TestFixture]
    public class ReentrantTest {

        [Test]
        public void TestLock()
        {
            var locker = new Reentrant();
            locker.Lock();
            locker.Release();
            
        }

    }

}