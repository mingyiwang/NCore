using System;
using System.Threading;

namespace Core.Concurrent
{

    /// <summary>
    /// http://www.albahari.com/threading/part5.aspx#_SpinLock_and_SpinWait
    /// </summary>
    public static class CAS
    {

        public static void Update<T>(ref T location, T item) where T : class
        {
            var spinWait = new SpinWait();
            while(true)
            {
                var snapshot = location; // old location
                var original = Interlocked.CompareExchange(ref location, item, snapshot); // before this operation , location might be changed
                if(snapshot == original)
                {
                    return;
                }
                spinWait.SpinOnce();
            }
        }

        public static void Update<T>(ref T location, Func<T> creator) where T : class
        {
            var spinWait = new SpinWait();
            while(true)
            {
                var snapshot = location;
                var original = Interlocked.CompareExchange(ref location, creator(), snapshot);
                if(snapshot == original)
                {
                    return;
                }

                spinWait.SpinOnce();
            }
        }

        public static void Update<T>(ref T location, Func<T, T> updator) where T : class
        {
            var spinWait = new SpinWait();
            while(true)
            {
                var snapshot = location;
                var original = Interlocked.CompareExchange(ref location, updator(snapshot), snapshot);
                if(snapshot == original)
                {
                    return;
                }
                spinWait.SpinOnce();
            }
        }

        public static bool TryUpdate<T>(ref T location, T item) where T : class
        {
            var snapshot = location;
            return snapshot == Interlocked.CompareExchange(ref location, item, snapshot);
        }

        public static bool TryUpdate<T>(ref T location, Func<T> creator) where T : class
        {
            var snapshot = location;
            return snapshot == Interlocked.CompareExchange(ref location, creator(), snapshot);
        }

        public static bool TryUpdate<T>(ref T location, Func<T, T> updator) where T : class
        {
            var snapshot = location;
            return snapshot == Interlocked.CompareExchange(ref location, updator(snapshot), snapshot);
        }

        public static bool TryCompareUpdate<T>(ref T location, T value, T comparand) where T : class
        {
            return comparand == Interlocked.CompareExchange(ref location, value, comparand);
        }

    }

}