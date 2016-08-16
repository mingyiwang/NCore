using System;

namespace Core.Primitive
{

    public enum RoundKind
    {
        None     = 0, // No rounding, cut off all the digits after decimal point
        Ceil     = 1, // 
        Floor    = 1 << 1,
        Up       = 1 << 2,
        Down     = 1 << 3,
        HalfUp   = 1 << 4,
        HalfDown = 1 << 5,
        HalfEven = 1 << 6
    }

    public sealed class Numbers
    {

        public const int  FLOAT_EXPONENT_MASK     = 0x7f800000;
        public const int  FLOAT_SIGNIFICANT_MASK  = 0x007fffff;
        public const long DOUBLE_EXPONENT_MASK    = 0x7ff0000000000000L;
        public const long DOUBLE_SIGNIFICANT_MASK = 0x000fffffffffffffL;
        
        public static bool IsInt<T>(T obj)
        {
            return typeof(T) == typeof(int);
        }

        public static int IntOf(Enum input) {
            return Convert.ToInt32(input);
        }

        public static int IntOf(string value)
        {
            return IntOf(value, 0);
        }

        public static int IntOf(string value, int defaultValue)
        {
            int result;
            var parsed = int.TryParse(value, out result);
            return parsed 
                 ? result 
                 : defaultValue
                 ;
        }

        public static string ToBinaryString(int value) {
            return Bits.Of(value).ToString();
        }

        public static int IntOf(float value,  RoundKind round)
        {
            return 0;
        }

        public static int IntOf(double value,  RoundKind round)
        {
            return 0;
        }

        public static int IntOf(decimal value, RoundKind round)
        {
            return 0;
        }

        public static decimal ToDecimal(byte[] bytes)
        {
            Checks.NotNull(bytes);
            Checks.Equals(16, bytes.Length);
            var lo    = BitConverter.ToInt32(new[] { bytes[0],  bytes[1],  bytes[2],  bytes[3]  }, 0);
            var mid   = BitConverter.ToInt32(new[] { bytes[4],  bytes[5],  bytes[6],  bytes[7]  }, 0);
            var hi    = BitConverter.ToInt32(new[] { bytes[8],  bytes[9],  bytes[10], bytes[11] }, 0);
            var flags = BitConverter.ToInt32(new[] { bytes[12], bytes[13], bytes[14], bytes[15] }, 0);
            return new decimal(lo, mid, hi, (flags & 0x80000000) == 1, bytes[14]);
        }

    }
    

}