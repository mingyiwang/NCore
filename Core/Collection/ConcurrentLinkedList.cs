using System.Collections;
using System.Collections.Generic;
using Core.Concurrent;

namespace Core.Collection
{

    public class ConcurrentLinkedList<T> : IList<T> where T : class
    {

        private volatile LinkedNode<T> _head;
        private volatile LinkedNode<T> _tail;

        public ConcurrentLinkedList()
        {
            _head = _tail = new LinkedNode<T>(default(T));
        }

        public bool IsReadOnly => false;

        public int Count
        {
            get {
                return 0;
            }
        }

        public T this[int index]
        {
            get {
                throw new System.NotImplementedException();
            }
            set {
                throw new System.NotImplementedException();
            }
        }

        public void AddFirst(T item)
        {
            Preconditions.CheckNotNull(item, "Item can not be null.");
            var node = new LinkedNode<T>(item);

            CAS.Update(ref _head, n => node);
        }

        public void AddLast(T item)
        {
            var node = new LinkedNode<T>(item);

        }

        public void AddBefore(LinkedNode<T> node, T item)
        {

        }

        public void AddAfter(LinkedNode<T> node, T item)
        {

        }

        public void Add(T item)
        {
            AddLast(item);
        }

        public bool Contains(T item)
        {
            throw new System.NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new System.NotImplementedException();
        }

        public int IndexOf(T item)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveFirst()
        {

        }

        public void RemoveLast()
        {

        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    public class LinkedNode<T> where T : class
    {
        private T _item;
        private LinkedNode<T> _next;
        private LinkedNode<T> _prev;

        public LinkedNode<T> Next => _next;
        public LinkedNode<T> Prev => _prev;
        public T Item => _item;

        public LinkedNode(T item)
        {
            SetItem(item);
            _next = null;
            _prev = null;
        }

        public LinkedNode<T> SetItem(T item)
        {
            CAS.Update(ref _item, () => item);
            return this;
        }

        public LinkedNode<T> SetNext(LinkedNode<T> node)
        {
            CAS.Update(ref _next, () => node);
            return this;
        }

        public LinkedNode<T> SetPrev(LinkedNode<T> node)
        {
            CAS.Update(ref _prev, () => node);
            return this;
        }

        public bool TrySetItem(T item)
        {
            return CAS.TryUpdate(ref _item, () => item);
        }

        public bool TrySetNext(LinkedNode<T> node)
        {
            return CAS.TryUpdate(ref _next, () => node);
        }

        public bool TrySetPrev(LinkedNode<T> node)
        {
            return CAS.TryUpdate(ref _prev, () => node);
        }

    }

}
