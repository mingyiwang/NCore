using System;

namespace Core.Concurrent
{
    public static class OS
    {
        public static readonly int  BITS_PER_BYTE = 8;
        public static readonly int  CPU_COUNT = Environment.ProcessorCount;
        public static readonly int  CORE_POOL_SIZE = CPU_COUNT + 1;
        public static readonly int  MIN_POOL_SIZE = 1;
        public static readonly int  MAXIMUM_POOL_SIZE = CPU_COUNT * 2 + 1;
        public static readonly bool IS_LITTLE_ENDIAN = BitConverter.IsLittleEndian;

    }

}