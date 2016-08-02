using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Core.Collection {

    public static class CollectionExtension {

        /// <summary>
        /// ForEach Extension
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action) {
            foreach(var item in collection) {
                var referenceCopy = item;
                action(referenceCopy); // in case action is running in another thread
            }
        }

        /// <summary>
        /// 1 based Action
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<int, T> action) {
            var index = 0;
            foreach(var item in collection) {
                var referenceCopy = item;
                action(index++, referenceCopy); // incase action is running in another thread
            }
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> collection)
        {
           return new HashSet<T>(collection);
        }

        public static string AsString<T>(this IEnumerable<T> collection)
        {
            return Collections.Join(collection);
        }

        public static void Clear<T>(this ConcurrentQueue<T> queue)
        {
            T result;
            while (queue.TryDequeue(out result)){}
        }

    }

}