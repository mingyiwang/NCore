using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using Core.Concurrent;

namespace Core.Collection
{

    public class ConcurrentLinkedQueue<T> : IProducerConsumerCollection<T>, IReadOnlyCollection<T> where T : class
    {

        private volatile Node<T> _head;
        private volatile Node<T> _tail;
        private volatile int _count;
        public int Count => _count;
        public Node<T> Tail => _tail;
        public Node<T> Head => _head;

        public bool IsSynchronized
        {
            get {
                return false;
            }
        }

        public object SyncRoot
        {
            get {
                throw new NotSupportedException();
            }
        }

        public ConcurrentLinkedQueue()
        {
            _head = _tail = new Node<T>(null);
        }

        public ConcurrentLinkedQueue(IEnumerable<T> collection)
        {
            Node<T> h;
            var t = h = new Node<T>(null);
            foreach(var item in collection)
            {
                PreConditions.CheckNotNull(item);
                var node = new Node<T>(item);
                t.UnsafeSet(node);
                t = node;
                _count++;
            }
            _head = h;
            _tail = t;
        }

        public bool TryAdd(T item)
        {
            PreConditions.CheckNotNull(item);
            var node = new Node<T>(item);

            Node<T> t;
            var p = t = _tail;
            var spin = new SpinWait();
            while(true)
            {
                var next = p.GetNext();
                if(next == null)
                {
                    if (p.CasNext(null, node))
                    {
                        Interlocked.Increment(ref _count);
                        if (p != t)
                        {
                            CasTail(t, node);
                        }
                        return true;
                    }

                    // contention failed then we do a small wait and retry
                    spin.SpinOnce();
                }
                else
                {
                    if(p == next)
                    {
                        p = (t != (t = _tail)) ? t : _head;
                    }
                    else
                    {
                        p = (p != t && t != (t = _tail)) ? t : next;
                    }
                }
            }
        }

        public bool TryTake(out T item)
        {
            Node<T> h;
            var p = h = _head;
            while(true)
            {
                item = p.GetItem();
                if(item != null && p.CasItem(item, null))
                {
                    Interlocked.Decrement(ref _count);
                    if(p != h)
                    {
                        UpdateHead(h, p.GetNext() ?? p);
                    }
                    return true;
                }

                var next = p.GetNext();
                if(next == null)
                {
                    UpdateHead(h, p);
                    item = null;
                    return true;
                }

                if(p == next)
                {
                    p = h = _head;
                }
                else
                {
                    p = next;
                }
            }
        }

        public T First()
        {
            Node<T> h;
            var p = h = _head;
            while(true)
            {
                var item = p.GetItem();
                if (item != null)
                {
                    if(p != h)
                    {
                        UpdateHead(h, p.GetNext() ?? p);
                    }
                    return item;
                }

                //This is a empty list
                var next = p.GetNext();
                if(next == null)
                {
                    UpdateHead(h, p);
                    return null;
                }

                if(p == next)
                {
                    p = h = _head;
                }
                else
                {
                    p = next;
                }

            }
        }

        public T Last()
        {
            Node<T> t;
            for(var p = t = _tail;;)
            {
                var next = p.GetNext();
                if(next == null)
                {
                    return p.GetItem();
                }
                if(p == next)
                {
                    p = (t != (t = _tail)) ? t : _head;
                }
                else
                {
                    p = (p != t && t != (t = _tail)) ? t : next;
                }
            }
        }

        /// <summary>
        /// When A Node is off list then it's Next should pointing to itself
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private Node<T> GetNext(Node<T> p)
        {
            var next = p.GetNext();
            return (p == next) ? _head : next;
        }

        /// <summary>
        /// Try Set Head Node of this Queue
        /// </summary>
        /// <param name="h"></param>
        /// <param name="p"></param>
        private void UpdateHead(Node<T> h, Node<T> p)
        {
            if(h != p && CasHead(h, p))
            {
                h.SetNext(h);
            }
        }

        public T[] ToArray()
        {
            var list = new List<T>(_count);
            return list.ToArray();
        }

        public void CopyTo(T[] array, int index)
        {
            Arrays.CopyTo(ToArray(), index, ref array);
        }

        public void CopyTo(Array array, int index)
        {
            // Arrays.CopyTo(ToArray(), index, ref array);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
           // Node<T> start = First();

            return new Iterator(this);
        }

        private bool CasHead(Node<T> comparand, Node<T> node)
        {
            return CAS.TryCompareSet(ref _head, node, comparand);
        }

        private void CasTail(Node<T> comparand, Node<T> node)
        {
            CAS.TryCompareSet(ref _tail, node, comparand);
        }

        class Iterator : IEnumerator<T>
        {
            private Node<T> _nextNode;
            private T _lastItem;
            private ConcurrentLinkedQueue<T> _queue;

            public Iterator(ConcurrentLinkedQueue<T> queue)
            {
                _queue = queue;
            }

            public T Current => _queue.First();

            object IEnumerator.Current
            {
                get {
                    return Current;
                }
            }

            public void Dispose()
            {
                _queue = null;
            }

            public bool MoveNext()
            {
                return false;
            }

            public void Reset()
            {
                
            }

            
        }

    }

    public sealed class Node<T> where T : class
    {

        private volatile T _item;
        private volatile Node<T> _next;

        public Node(T item)
        {
            _item = item;
        }

        public Node<T> GetNext()
        {
            return _next;
        }

        public T GetItem()
        {
            return _item;
        }

        public void SetNext(Node<T> node)
        {
            CAS.Set(ref _next, node);
        }

        public bool CasNext(Node<T> comparand, Node<T> node)
        {
            return CAS.TryCompareSet(ref _next, node, comparand);
        }

        public void UnsafeSet(Node<T> node)
        {
            _next = node;
        }

        public bool CasItem(T comparand, T item)
        {
            return CAS.TryCompareSet(ref _item, item, comparand);
        }

        public void SetItem(T item)
        {
            CAS.Set(ref _item, item);
        }

    }
}