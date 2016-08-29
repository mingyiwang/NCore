using System;

namespace Core.Primitive
{

    public static class NumberExtenstion
    {

        /// <summary>
        /// Byte is the smallest unit of memory address so doesn't need to consider edianess
        /// It is always from high bit to low bit in all platforms
        /// </summary>
        /// <param name="b">byte</param>
        /// <returns>Binary String representation of <code>byte</code> </returns>
        public static string ToBinaryString(this byte b)
        {
            return Convert.ToString(b, 2).PadLeft(Bits.BitsPerByte, Chars.Zero);
        }

        public static bool IsNegative(this float value)
        {
            return (Bits.Of(value).ToIntBits().Last() & Numbers.NegativeSignMask) == Numbers.NegativeSignMask;            
        }

        public static bool IsNegative(this double value)
        {
            return (Bits.Of(value).ToIntBits().Last() & Numbers.NegativeSignMask) == Numbers.NegativeSignMask;
        }

    }


}