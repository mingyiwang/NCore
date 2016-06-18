using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Core.Collection
{

    public class ConcurrentHashMap<TKey, TValue> : IDictionary<TKey, TValue>
    {

        private readonly ConcurrentDictionary<TKey, TValue> _dictionary;

        public ConcurrentHashMap()
        {
            _dictionary = new ConcurrentDictionary<TKey, TValue>();
        }

        public ConcurrentHashMap(ConcurrentDictionary<TKey, TValue> init)
        {
            _dictionary = new ConcurrentDictionary<TKey, TValue>(init);
        }

        public ConcurrentHashMap(ConcurrentHashMap<TKey, TValue> init)
        {
            _dictionary = new ConcurrentDictionary<TKey, TValue>(init._dictionary);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            throw new System.NotImplementedException();
        }

        public int Count { get; }
        public bool IsReadOnly { get; }
        public bool ContainsKey(TKey key)
        {
            throw new System.NotImplementedException();
        }

        public void Add(TKey key, TValue value)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(TKey key)
        {
            throw new System.NotImplementedException();
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            throw new System.NotImplementedException();
        }

        public TValue this[TKey key]
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public ICollection<TKey> Keys { get; }
        public ICollection<TValue> Values { get; }
    }

}