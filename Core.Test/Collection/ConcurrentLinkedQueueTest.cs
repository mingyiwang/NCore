using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using Core.Collection;
using NUnit.Framework;

namespace Core.Test.Collection {

    [TestFixture]
    public class ConcurrentLinkedQueueTest {

        [Test]
        public void TestInit() {
            var lists = new LinkedList<T>();
            for(var i = 1; i <= 10000000; i++) {
                lists.AddLast(new T(i));
            }

            var watch = new Stopwatch();
            watch.Start();
            var queue1 = new ConcurrentLinkedQueue<T>(lists);
            watch.Stop();
            Console.WriteLine($"ConcrruentLinkedQueue Initialization took {watch.ElapsedMilliseconds} ms");

            var watch2 = new Stopwatch();
            watch2.Start();
            var queue2 = new ConcurrentQueue<T>(lists);
            watch2.Stop();
            Console.WriteLine($"ConcurrentQueue Initialization took {watch2.ElapsedMilliseconds} ms");

            var watch3 = new Stopwatch();
            watch3.Start();
            var queue3 = new Queue<T>(lists);
            watch3.Stop();
            Console.WriteLine($"Queue Initialization took {watch3.ElapsedMilliseconds} ms");

            Assert.AreEqual(queue1.Count, 10000000);
            Assert.AreEqual(queue2.Count, 10000000);
            Assert.AreEqual(queue3.Count, 10000000);
            Assert.AreNotEqual(queue1.First(), null);
            GC.Collect();
        }

        [Test]
        public void TestTakeItem() {
            var queue1 = new ConcurrentLinkedQueue<T>();
            T item;
            Assert.AreEqual(queue1.First(), null);
            Assert.AreEqual(queue1.TryTake(out item), false);
            queue1.TryAdd(new T(1));
            queue1.First();
            Assert.AreNotEqual(queue1.First(), null);
            queue1.TryTake(out item);
            Assert.AreEqual(queue1.First(), null);
            Assert.AreEqual(queue1.TryTake(out item), false);
            Assert.AreEqual(queue1.First(), null);
            queue1.TryAdd(new T(1));
            Assert.AreEqual(queue1.TryTake(out item), true);
            Assert.AreEqual(queue1.First(), null);

        }

        [Test]
        public void TestAddItem() {
            var queue1 = new ConcurrentLinkedQueue<T>();
            for(var i = 1; i <= 100; i++) {
                var item = new T(i);
                queue1.TryAdd(item);
                if(queue1.Tail.GetItem() != null)
                    Console.WriteLine("Tail => " + queue1.Tail.GetItem().GetValue());
            }

            /*
                Tests.Test(1, () => {
                    for (var i = 1; i <= 1000000; i++) {
                         var item = new T(i);
                         queue1.TryAdd(item);
                    }
                });
            */

            // Console.WriteLine("Head => " + queue1.Head.GetItem().GetValue());
            // Console.WriteLine("Tail => " + queue1.Tail.GetItem().GetValue());
            Console.WriteLine(queue1.Count);
        }

        [Test]
        public void TestTryTakeItem() {
            var queue1 = new ConcurrentLinkedQueue<T>();
            for(var i = 1; i <= 100; i++) {
                var item = new T(i);
                queue1.TryAdd(item);

                // if(queue1.Tail.GetItem() != null)
                 //Console.WriteLine("Tail => " + queue1.Tail.GetItem().GetValue());
            }

            for(var i = 1; i <= 100; i++) {
                T item;
                queue1.TryTake(out item);

                if(queue1.Head.GetItem() != null) {
                   Console.WriteLine("Head => " + queue1.Head.GetItem().GetValue());
                }
            }
            Console.WriteLine(queue1.Count);

        }

        class T {
            private int Value {
                get; set;
            }

            public int GetValue() {
                return Value;
            }

            public T(int val) {
                Value = val;
            }

        }

    }
}