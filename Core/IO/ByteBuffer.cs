
using System;

namespace Core.IO
{
    public class ByteBuffer : Buffer<byte>
    {
        private readonly byte[] _storage;

        public ByteBuffer(int capacity) : base(capacity)
        {
            _storage = new byte[capacity];
        }

        public override byte Get()
        {
            if (Position >= Limit)
            {
                throw new IndexOutOfRangeException();
            }

            var next = _storage[Position];
            SetPosition(Position + 1);
            return next;
        }

        public override void Put(byte data)
        {
            _storage[Position + 1] = data;
            SetPosition(Position + 1);
        }

    }
}