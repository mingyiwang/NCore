using System;
using System.Collections.Generic;
using System.Linq;
using Core.Concurrent;

namespace Core.Collection
{

    public class ReadWriteHashMap<TKey, TValue> : IDictionary<TKey, TValue>, IDisposable
    {

        private readonly Dictionary<TKey, TValue> _dict;
        private readonly ReadWrite _lock = new ReadWrite();

        /// <summary>
        /// Initializes the dictionary object
        /// </summary>
        public ReadWriteHashMap()
        {
            _dict = new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// Initializes the dictionary object
        /// </summary>
        /// <param name="capacity">initial capacity of the dictionary</param>
        public ReadWriteHashMap(int capacity)
        {
            _dict = new Dictionary<TKey, TValue>(capacity);
        }

        /// <summary>
        /// Initializes the dictionary object
        /// </summary>
        /// <param name="comparer">the comparer to use when comparing keys</param>
        public ReadWriteHashMap(IEqualityComparer<TKey> comparer)
        {
            _dict = new Dictionary<TKey, TValue>(comparer);
        }

        /// <summary>
        /// Initializes the dictionary object
        /// </summary>
        /// <param name="dictionary">the dictionary whose keys and values are copied to this object</param>
        public ReadWriteHashMap(IDictionary<TKey, TValue> dictionary)
        {
            _dict = new Dictionary<TKey, TValue>(dictionary);
        }

        /// <summary>
        /// Initializes the dictionary object
        /// </summary>
        /// <param name="capacity">initial capacity of the dictionary</param>
        /// <param name="comparer">the comparer to use when comparing keys</param>
        public ReadWriteHashMap(int capacity, IEqualityComparer<TKey> comparer)
        {
            _dict = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        /// <summary>
        /// Initializes the dictionary object
        /// </summary>
        /// <param name="dictionary">the dictionary whose keys and values are copied to this object</param>
        /// <param name="comparer">the comparer to use when comparing keys</param>
        public ReadWriteHashMap(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _dict = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        /// <summary>
        /// Adds an item to the dictionary
        /// </summary>
        /// <param name="key">the key to add</param>
        /// <param name="value">the value to add</param>
        public void Add(TKey key, TValue value)
        {
            _lock.Write(() => _dict.Add(key, value));
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            _lock.Write(() => _dict.Add(item.Key, item.Value));
        }

        /// <summary>
        /// Returns true if the key value pair exists in the dictionary
        /// </summary>
        /// <param name="item">key value pair to find</param>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return _lock.Read(() => ((_dict.ContainsKey(item.Key)) && (_dict.ContainsValue(item.Value))));
        }

        /// <summary>
        /// Returns true if the key exists in the dictionary
        /// </summary>
        /// <param name="key">the key to find in the dictionary</param>
        public bool ContainsKey(TKey key)
        {
            return _lock.Read(() => _dict.ContainsKey(key));
        }

        public TValue Get(TKey key)
        {
            return _lock.Read(() => {
                TValue value;
                return _dict.TryGetValue(key, out value) ? value : default(TValue);
            });
        }

        /// <summary>
        /// Returns the keys as a collection
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                return _lock.Read(() => _dict.Keys);
            }
        }

        public void Put(TKey key, TValue value)
        {
            _lock.Write(() => { _dict[key] = value; });
        }

        public void PutAll(IDictionary<TKey, TValue> parameters)
        {
            _lock.Write(() => {
                foreach(var parameter in parameters)
                {
                    _dict[parameter.Key] = parameter.Value;
                }
            });
        }

        /// <summary>
        /// Removes the element with this key name
        /// </summary>
        /// <param name="key">the key to remove</param>
        public bool Remove(TKey key)
        {
            return _lock.Write(() => _dict.Remove(key));
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return _lock.TryRead(out value, v => {
                _dict.TryGetValue(key, out v);
            });
        }

        /// <summary>
        /// Removes the element with this key name and value. Returns
        /// true if the item was removed.
        /// </summary>
        /// <param name="item">the key to remove</param>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return _lock.Write(() => {
                // skip if the key doesn't exist
                TValue tempVal;
                if(!_dict.TryGetValue(item.Key, out tempVal))
                    return false;

                // skip if the value's don't match
                if(!tempVal.Equals(item.Value))
                    return false;

                return _dict.Remove(item.Key);
            });
        }

        public void ReplaceAll(IDictionary<TKey, TValue> parameters)
        {
            _lock.Write(() => {
                _dict.Clear();
                foreach (var parameter in parameters)
                {
                    _dict[parameter.Key] = parameter.Value;
                }
            });
        }

        /// <summary>
        /// Attemtps to return the value found at element <paramref name="key"/>
        /// If no value is found, returns false
        /// </summary>
        /// <param name="key">the key to find</param>
        /// <param name="value">the value returned if the key is found</param>
       /* public bool TryGetValue(TKey key, out TValue value) {
            _lock.Lock.EnterReadLock();
            try {
                return _dict.TryGetValue(key, out value);
            } finally {
                _lock.Lock.ExitReadLock();
            }
        }*/

        /// <summary>
        /// Returns a collection of the values in the dictionary
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                return _lock.Read(() => _dict.Values);
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                return _lock.Read(() => _dict[key]);
            }
            set
            {
                _lock.Write(() => { _dict[key] = value; });
            }
        }

        /// <summary>
        /// Clears the dictionary
        /// </summary>
        public void Clear()
        {
            _lock.Write(() => _dict.Clear());
        }

        /// <summary>
        /// Copies the items of the dictionary to a key value pair array
        /// </summary>
        /// <param name="array">the key value pair collection to copy into</param>
        /// <param name="arrayIndex">the index to begin copying to</param>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            _lock.Write(() => _dict.ToArray().CopyTo(array, arrayIndex));
        }

        /// <summary>
        /// Returns the number of items in the dictionary
        /// </summary>
        public int Count
        {
            get
            {
                return _lock.Read(() => _dict.Count);
            }
        }

        public bool IsReadOnly => false;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return _lock.Read(() => ((IEnumerable<KeyValuePair<TKey, TValue>>)new Dictionary<TKey, TValue>(_dict)).GetEnumerator());
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _lock.Read(() => new Dictionary<TKey, TValue>(_dict).GetEnumerator());
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
            return _lock.WriteIf(() => {
                TValue value;
                return !_dict.TryGetValue(key, out value);
            }, () => {
                TValue value;
                if(_dict.TryGetValue(key, out value))
                {
                    return value;
                }

                value = func.Invoke();
                _dict.Add(key, value);
                return value;

            });

        }

        /// <summary>
        /// Adds the value if it's key does not already exist. Returns
        /// true if the value was added
        /// </summary>
        /// <param name="key">the key to check, add</param>
        /// <param name="value">the value to add if the key does not already exist</param>
        public bool AddIfNotExists(TKey key, TValue value)
        {
            return true;
        }

        /// <summary>
        /// Adds the list of value if the keys do not already exist.
        /// </summary>
        /// <param name="keys">the keys to check, add</param>
        /// <param name="defaultValue">the value to add if the key does not already exist</param>
        public void AddIfNotExists(IEnumerable<TKey> keys, TValue defaultValue)
        {

        }

        /// <summary>
        /// Updates the value of the key if the key exists. Returns true if updated
        /// </summary>
        /// <param name="key"></param>
        /// <param name="newValue"></param>
        public bool UpdateValueIfKeyExists(TKey key, TValue newValue)
        {
            return true;
        }

        /// <summary>
        /// Returns true if the dictionary contains this value
        /// </summary>
        /// <param name="value">the value to find</param>
        public bool ContainsValue(TValue value)
        {
            return true;
        }

        /// <summary>
        /// Removes items from the dictionary that match a pattern. Returns true
        /// on success
        /// </summary>
        /// <param name="predKey">Optional expression based on the keys</param>
        /// <param name="predValue">Option expression based on the values</param>
        public bool Remove(Predicate<TKey> predKey, Predicate<TValue> predValue)
        {
            return true;
        }

        public int Size()
        {
            return Count;
        }

        public override int GetHashCode()
        {
            return _dict.Count;
        }


        public bool Equals(ReadWriteHashMap<TKey, TValue> d)
        {
            if(d == null || d.Count != Count)
            {
                return false;
            }
            return d.All(entry => Get(entry.Key).Equals(entry.Value));
        }

        public override bool Equals(object obj)
        {
            if(ReferenceEquals(null, obj)) return false;
            if(ReferenceEquals(this, obj)) return true;
            var d = obj as ReadWriteHashMap<TKey, TValue>;
            if(d == null) return false;
            return Equals(d);
        }

        public void Dispose()
        {
            _lock.Dispose();
        }


    }
}