using System.Collections;
using System.Collections.Generic;
using Core.Concurrent;

namespace Core.Collection
{

    public class ReadWriteList<T> : IList<T>
    {

        private readonly List<T> _list;
        private readonly ReadWrite _lock = new ReadWrite();

        public ReadWriteList()
        {
            _list = new List<T>();
        }

        public ReadWriteList(IEnumerable<T> items)
        {
            _list = new List<T>(items);
        }

        public bool IsReadOnly => false;

        public int Count
        {
            get
            {
                return _lock.Read(() => _list.Count);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _lock.Read(() => _list.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item) { _lock.Write(() => _list.Add(item)); }
        public void Clear() { _lock.Write(() => _list.Clear()); }

        public bool Contains(T item) { return _lock.Read(() => _list.Contains(item)); }
        public void CopyTo(T[] array, int arrayIndex) { _lock.Write(() => _list.CopyTo(array, arrayIndex)); }

        public bool Remove(T item) { return _lock.Write(() => _list.Remove(item)); }
        public int IndexOf(T item) { return _lock.Read(() => _list.IndexOf(item)); }

        public void Insert(int index, T item) { _lock.Write(() => _list.Insert(index, item)); }
        public void RemoveAt(int index) { _lock.Write(() => RemoveAt(index)); }

        public T this[int index]
        {
            get { return _lock.Read(() => _list[index]); }
            set { _lock.Write(value, v => { _list[index] = v; }); }
        }

    }
}