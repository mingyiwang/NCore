using System;

namespace Core.Primitive
{

    public static class ByteExtenstion
    {
        /// <summary>
        /// Byte is the smallest unit of address so doesn't need to consider edianess
        /// It is always from high bit to low bit
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string ToBinaryString(this byte b)
        {
            return Convert.ToString(b,2).PadLeft(Bits.BitsPerByte, Chars.Zero);
        }

        

    }


}