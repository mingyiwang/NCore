using System.Collections;
using System.Collections.Generic;
using Core.Concurrent;
using Core.Time.Format;

namespace Core.Collection
{

    public class ReadWriteHashSet<T> : ISet<T>
    {

        private readonly HashSet<T> _hashSet = new HashSet<T>();
        private readonly ReadWrite _lock = new ReadWrite();

        public bool IsReadOnly => false;

        public int Count
        {
            get {

                return _lock.Read(() => _hashSet.Count);
            }
        }

        public ReadWriteHashSet()
        {
        }

        public ReadWriteHashSet(IEnumerable<T> collection) : base()
        {
            foreach (var item in collection)
            {
                _hashSet.Add(item);
            }
        }

        void ICollection<T>.Add(T item)
        {
            _lock.Write(() => _hashSet.Add(item));
        }

        public void UnionWith(IEnumerable<T> other)
        {
            _lock.Write(() => _hashSet.UnionWith(other));
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            _lock.Write(() => _hashSet.IntersectWith(other));
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            _lock.Write(() => _hashSet.ExceptWith(other));
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            _lock.Write(() => _hashSet.SymmetricExceptWith(other));
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return _lock.Read(() => _hashSet.IsSubsetOf(other));
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return _lock.Read(() => _hashSet.IsSupersetOf(other));
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return _lock.Read(() => _hashSet.IsProperSupersetOf(other));
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return _lock.Read(() => _hashSet.IsProperSubsetOf(other));
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return _lock.Read(() => _hashSet.Overlaps(other));
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return _lock.Read(() => _hashSet.SetEquals(other));
        }

        public bool Add(T item)
        {
            return _lock.Write(() => _hashSet.Add(item));
        }

        public void Clear()
        {
            _lock.Write(() => _hashSet.Clear());
        }

        public bool Contains(T item)
        {
            return _lock.Read(() => _hashSet.Contains(item));
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _lock.Write(() => _hashSet.CopyTo(array, arrayIndex));
        }

        public bool Remove(T item)
        {
            return _lock.Write(() => _hashSet.Remove(item));
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _lock.Read(() => _hashSet.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

}