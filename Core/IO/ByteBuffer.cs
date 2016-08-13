
using System;

namespace Core.IO
{
    public class ByteBuffer : Buffer<byte>
    {
        
        public ByteBuffer(int capacity) : base(capacity){ }

        public override byte Get()
        {
            if (Position >= Limit)
            {
                throw new IndexOutOfRangeException();
            }
            
            SetPosition(Position + 1);
            return 0;
        }

        public override void Put(byte data)
        {
            
        }

    }
}