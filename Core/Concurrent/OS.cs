using System;

namespace Core.Concurrent
{
    public static class OS
    {
        public readonly static int  BITS_PER_BYTE = 8;
        public readonly static int  CPU_COUNT = Environment.ProcessorCount;
        public readonly static int  CORE_POOL_SIZE = CPU_COUNT + 1;
        public readonly static int  MIN_POOL_SIZE = 1;
        public readonly static int  MAXIMUM_POOL_SIZE = CPU_COUNT * 2 + 1;
        public readonly static bool IS_LITTLE_ENDIAN = BitConverter.IsLittleEndian;

    }

}