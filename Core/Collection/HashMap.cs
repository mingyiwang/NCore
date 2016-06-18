using System;
using System.Collections;
using System.Collections.Generic;

namespace Core.Collection
{

    public class HashMap<TKey, TValue> : IDictionary<TKey, TValue>
    {

        ICollection<TKey> IDictionary<TKey, TValue>.Keys => _dict.Keys;
        ICollection<TValue> IDictionary<TKey, TValue>.Values => _dict.Values;

        public static HashMap<TKey, TValue> Of(Dictionary<TKey, TValue> dict)
        {
            return new HashMap<TKey, TValue>(dict);
        }

        private readonly Dictionary<TKey, TValue> _dict;

        public HashMap()
        {
            _dict = new Dictionary<TKey, TValue>();
        }

        public HashMap(Dictionary<TKey, TValue> init)
        {
            _dict = new Dictionary<TKey, TValue>(init);
        }

        public HashMap(HashMap<TKey, TValue> init)
        {
            _dict = new Dictionary<TKey, TValue>(init._dict);
        }

        public HashMap(int initialCapacity)
        {
            _dict = new Dictionary<TKey, TValue>(initialCapacity);
        }

       

        public int Count => _dict.Count;

        public bool IsReadOnly => false;

        public TValue this[TKey key]
        {
            get { return Get(key); }
            set { _dict[key] = value; }
        }

        public void Add(TKey key, TValue value)
        {
            _dict[key] = value;
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _dict[item.Key] = item.Value;
        }

        /// <summary>
        /// Adds the value if it's key does not already exist. Returns
        /// true if the value was added
        /// </summary>
        /// <param name="key">the key to check, add</param>
        /// <param name="value">the value to add if the key does not already exist</param>
        public bool AddIfAbsent(TKey key, TValue value)
        {
            if(_dict.ContainsKey(key)) {
                return false;
            }

            _dict.Add(key, value);
            return true;
        }

        /// <summary>
        /// Adds the list of value if the keys do not already exist.
        /// </summary>
        /// <param name="keys">the keys to check, add</param>
        /// <param name="defaultValue">the value to add if the key does not already exist</param>
        public void AddIfAbsent(IEnumerable<TKey> keys, TValue defaultValue)
        {
            foreach(TKey key in keys)
            {
                if(!_dict.ContainsKey(key))
                {
                    _dict.Add(key, defaultValue);
                }
            }
        }

        public void Clear()
        {
            _dict.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var value = Get(item.Key);
            return Equals(item.Value, value);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
        {
            if(index < 0 || index > array.Length)
            {
                throw new ArgumentOutOfRangeException();
            }

            if(array.Length - index < Count)
            {
                throw new ArgumentException();
            }

            int count = Count;
            foreach(var item in this)
            {
                array[count++] = new KeyValuePair<TKey, TValue>(item.Key, item.Value);
            }
        }

        public bool ContainsKey(TKey key)
        {
            return _dict.ContainsKey(key);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _dict.GetEnumerator();
        }


        /// <summary>
        /// Returns the value of <paramref name="key"/>. If <paramref name="key"/>
        /// does not exist, <paramref name="func"/> is performed and added to the 
        /// dictionary
        /// </summary>
        /// <param name="key">the key to check</param>
        /// <param name="func">the delegate to call if key does not exist</param>
        public TValue GetValueAddIfNotExist(TKey key, Func<TValue> func)
        {
            TValue value;
            if(_dict.TryGetValue(key, out value))
            {
                return value;
            }

            value = func.Invoke();
            _dict.Add(key, value);
            return value;
        }

        public bool Remove(TKey key)
        {
            return _dict.Remove(key);
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var value = Get(item.Key);
            if(Equals(item.Value, value))
            {
                _dict.Remove(item.Key);
                return true;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _dict.TryGetValue(key, out value);
        }

        public ICollection<TKey> Keys()
        {
            return _dict.Keys;
        }

        public TValue Get(TKey key)
        {
            TValue value;
            if(_dict.TryGetValue(key, out value))
            {
                return value;
            }
            return default(TValue);
        }

        public void Put(TKey key, TValue value)
        {
            _dict[key] = value;
        }

        public void PutAll(HashMap<TKey, TValue> parameters)
        {
            foreach (var parameter in parameters)
            {
                _dict[parameter.Key] = parameter.Value;
            }
        }

        public int Size()
        {
            return _dict.Count;
        }

        public ICollection<TValue> Values => _dict.Values;

        public Dictionary<TKey, TValue> ToDictionary()
        {
            return _dict;
        }

    }
}