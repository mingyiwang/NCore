using System.Collections;
using System.Collections.Generic;
using Core.Concurrent;

namespace Core.Collection
{

    public class ReadWriteLinkedList<T> : IList<T>
    {
        private readonly LinkedList<T> _list;
        private readonly ReadWrite _lock = new ReadWrite();

        public ReadWriteLinkedList()
        {
            _list = new LinkedList<T>();
        }

        public ReadWriteLinkedList(IEnumerable<T> collection)
        {
            _list = new LinkedList<T>(collection);
        }

        public int Count
        {
            get
            {
                return _lock.Read(() => _list.Count);
            }
        }

        public bool IsReadOnly => false;

        public T this[int index]
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public void Add(T item) {_lock.Write(() => _list.AddLast(item));}
        public void Clear() { _lock.Write(() => _list.Clear()); }
        public bool Contains(T item) { return _lock.Read(() => _list.Contains(item)); }
        public void CopyTo(T[] array, int arrayIndex) { _lock.Write(() => _list.CopyTo(array, arrayIndex)); }
        public bool Remove(T item) { return _lock.Write(() => _list.Remove(item)); }

        public int IndexOf(T item)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAt(int index) { _lock.Write(() => RemoveAt(index)); }

        
        public IEnumerator<T> GetEnumerator()
        {
            return _lock.Read(() => _list.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}