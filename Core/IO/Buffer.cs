using System;
using Core.Collection;

namespace Core.IO
{

    public abstract class Buffer<T> : IEquatable<Buffer<T>>
    {
        private const int MarkUnset = -1;

        private readonly int _capacity;
        private int _position;
        private int _limit;
        private int _mark;
        private readonly T[] _store;

        public abstract T Get();
        public abstract void Put(T data);

        protected Buffer(int capacity)
        {
            _limit = _capacity = capacity;
            _position = 0;
            _mark = MarkUnset;
            _store = new T[_capacity];
        }

        public int Remaining => _limit - _position;
        public int Position  => _position;
        public int Limit     => _limit;
        public int Capacity  => _capacity;
       
        public void SetPosition(int pos)
        {
            if (pos < 0 || pos > _limit)
            {
                throw new ArgumentException("Bad position (limit " + _limit + "): " + pos);
            }

            _position = pos;
            if ((_mark != MarkUnset) && (_mark > pos))
            {
                _mark = MarkUnset;
            }
        }

        public void SetLimit(int limit)
        {
            if (limit < 0 || limit > _capacity)
            {
                throw new ArgumentException("Bad limit (capacity " + _capacity + "): " + limit);
            }

            _limit = limit;
            if (_position > limit)
            {
                _position = limit;
            }

            if ((_mark != MarkUnset) && (_mark > limit))
            {
                _mark = MarkUnset;
            }
        }

        public void Flip()
        {
            _limit    = _position;
            _position = 0;
            _mark     = MarkUnset;
        }

        public void Mark()
        {
            _mark = _position;
        }

        public void Reset()
        {
            _position = _mark;
            _mark = MarkUnset;
        }

        public void Clear()
        {
            _position = 0;
            _limit = _capacity;
            _mark  = MarkUnset;
        }

        public bool Equals(Buffer<T> other)
        {
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            
            return Arrays.Equals(_store, other._store) 
                && _position == other._position
                && _capacity == other._capacity
                && _limit    == other._limit
                && _mark     == other._mark
                ;
        }

        public int CompareTo(Buffer<T> other)
        {
            throw new NotImplementedException();
        }

    }
}