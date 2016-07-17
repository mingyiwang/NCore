using System;
using System.Threading;

namespace Core.Concurrent
{

    public static class CAS
    {

        public static void Set<T>(ref T oldValue, T item) where T : class
        {
            var spinWait = new SpinWait();
            while(true)
            {
                var snapshot = oldValue;
                var original = Interlocked.CompareExchange(ref oldValue, item, snapshot); // before this operation , location might be changed
                if(snapshot == original)
                {
                    return;
                }
                spinWait.SpinOnce();
            }
        }

        public static void Set<T>(ref T location, Func<T> creator) where T : class
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

        public static void Set<T>(ref T oldValue, Func<T, T> updator) where T : class
        {
            var spinWait = new SpinWait();
            while(true)
            {
                var snapshot = oldValue;
                var original = Interlocked.CompareExchange(ref oldValue, updator(snapshot), snapshot);
                if(snapshot == original)
                {
                    return;
                }
                spinWait.SpinOnce();
            }
        }

        public static bool TrySet<T>(ref T oldValue, T item) where T : class
        {
            var snapshot = oldValue;
            return snapshot == Interlocked.CompareExchange(ref oldValue, item, snapshot);
        }

        public static bool TrySet<T>(ref T oldValue, Func<T> creator) where T : class
        {
            var snapshot = oldValue;
            var newValue = creator();
            return snapshot == Interlocked.CompareExchange(ref oldValue, newValue, snapshot);
        }

        public static bool TrySet<T>(ref T oldValue, Func<T,T> updator) where T : class
        {
            var snapshot = oldValue;
            var newValue = updator(snapshot);
            return snapshot == Interlocked.CompareExchange(ref oldValue, newValue, snapshot);
        }

        public static bool TryCompareSet<T>(ref T oldValue, Func<T> creator, T comparand) where T : class
        {
            var newValue = creator();
            return comparand == Interlocked.CompareExchange(ref oldValue, newValue, comparand);
        }

        public static bool TryCompareSet<T>(ref T oldValue, Func<T, T> updator, T comparand) where T : class
        {
            var snapshot = oldValue;
            var newValue = updator(snapshot);
            return comparand == Interlocked.CompareExchange(ref oldValue, newValue, comparand);
        }

        public static bool TryCompareSet<T>(ref T oldValue, T newValue, T comparand) where T : class
        {
            return comparand == Interlocked.CompareExchange(ref oldValue, newValue, comparand);
        }

    }

}