﻿using System;

namespace Core.Concurrent
{

    public static class OS
    {

        public static readonly int  BitsPerByte     = 8;
        public static readonly int  CpuCount        = Environment.ProcessorCount;
        public static readonly int  CorePoolSize    = CpuCount + 1;
        public static readonly int  MinPoolSize     = 1;
        public static readonly int  MaximumPoolSize = CpuCount * 2 + 1;
        public static readonly bool IsX64           = Environment.Is64BitOperatingSystem;
        public static readonly bool IsLittleEndian  = BitConverter.IsLittleEndian;

    }

}