using System;
using System.Globalization;
using System.Threading;

namespace Core.Concurrent
{

    public static class CAS
    {

        public static bool TrySet<T>(ref T oldValue, T item) where T : class
        {
            var snapshot = oldValue;
            return snapshot == Interlocked.CompareExchange(ref oldValue, item, snapshot);
        }

        public static bool TrySet<T>(ref T oldValue, Func<T> creator) where T : class
        {
            var newValue = creator();
            var snapshot = oldValue;
            return snapshot == Interlocked.CompareExchange(ref oldValue, newValue, snapshot);
        }

        public static void Set<T>(ref T oldValue, T item) where T : class
        {
            var spinWait = new SpinWait();
            while(true)
            {
                var snapshot = oldValue;
                if(snapshot == Interlocked.CompareExchange(ref oldValue, item, snapshot))
                {
                    return;
                }
                spinWait.SpinOnce();
            }
        }

        public static void Set<T>(ref T location, Func<T> creator) where T : class
        {
            var newValue = creator();
            var spinWait = new SpinWait();
            while(true)
            {
                var snapshot = location;
                if(snapshot == Interlocked.CompareExchange(ref location, newValue, snapshot))
                {
                    return;
                }

                spinWait.SpinOnce();
            }
        }

        public static bool TryCompareSet<T>(ref T oldValue, T newValue, T comparand) where T : class
        {
            return comparand == Interlocked.CompareExchange(ref oldValue, newValue, comparand);
        }

        public static bool TryCompareSet<T>(ref T oldValue, Func<T> creator, T comparand) where T : class
        {
            var newValue = creator();
            return comparand == Interlocked.CompareExchange(ref oldValue, newValue, comparand);
        }

    }

}