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

        public static int GetInt(Enum input) {
            return Convert.ToInt32(input);
        }

        public static int GetInt(string value)
        {
            return GetInt(value, 0);
        }

        public static int GetInt(string value, int defaultValue)
        {
            int result;
            var parsed = int.TryParse(value, out result);
            return parsed 
                 ? result 
                 : defaultValue
                 ;
        }

        public static int GetInt(long longValue)
        {
            return checked((int) longValue);
        }

        public static string ToBinaryString(int value)
        {
            return Bits.Of(value).ToBinaryString();
        }

        public static int GetInt(float value, RoundKind round)
        {
            if (value <= int.MinValue)
            {
                return int.MinValue;
            }

            if (value >= int.MaxValue)
            {
                return int.MaxValue;
            }

            var intValue = (int) value;
            var diff = Math.Abs(value - intValue);

            switch (round)
            {
                case RoundKind.Up :
                {
                    return (value < 0) || Equals(diff, 0f) ? intValue : 1 + intValue;
                }
                case RoundKind.Ceil :
                {
                    return (value < 0) || Equals(diff, 0f) ? intValue : 1 + intValue;
                }
                case RoundKind.Floor:
                {
                    return value < 0 ? intValue - 1 : intValue;
                }
                case RoundKind.HalfUp:
                {
                    return diff > 0.5f || Equals(diff, 0.5f) ? intValue + 1 : intValue;
                }
                case RoundKind.HalfDown:
                {
                    return diff < 0.5f || Equals(diff, 0.5f) ? intValue : intValue + 1;
                }
                case RoundKind.HalfEven:
                {
                    if (Equals(diff, 0.5f))
                    {
                        return (intValue & 1) == 0 ? intValue : intValue + 1;
                    }
                    return diff > 0.5f ? intValue + 1 : intValue;
                }
                default : return intValue;
            }            
        }

        public static long GetLong(double value, RoundKind round)
        {
            var longValue = (long) value;
            var diff = Math.Abs(value - longValue);
            
            if (longValue < int.MinValue || longValue > int.MaxValue)
            {

            }

            return 0;
        }

        public static int GetInt(decimal value, RoundKind round)
        {

            return 0;
        }

        public static decimal GetDecimal(byte[] bytes)
        {
            Checks.NotNull(bytes);
            Checks.Equals(16, bytes.Length);

            var lo    = BitConverter.ToInt32(new[] { bytes[0],  bytes[1],  bytes[2],  bytes[3]  }, 0);
            var mid   = BitConverter.ToInt32(new[] { bytes[4],  bytes[5],  bytes[6],  bytes[7]  }, 0);
            var hi    = BitConverter.ToInt32(new[] { bytes[8],  bytes[9],  bytes[10], bytes[11] }, 0);
            var flags = BitConverter.ToInt32(new[] { bytes[12], bytes[13], bytes[14], bytes[15] }, 0);
            return new decimal(lo, mid, hi, (flags & 0x80000000) == 1, bytes[14]);
        }

        public static bool Equals(float v1, float v2)
        {
            if (Bits.IsNaN(v1) || Bits.IsNaN(v2))
            {
                return false;
            }

            var v1Bits = Bits.Of(v1).ToIntBits();
            var v2Bits = Bits.Of(v2).ToIntBits();
            return v1Bits == v2Bits;
        }

        public static bool Equals(double v1, double v2)
        {
            if(Bits.IsNaN(v1) || Bits.IsNaN(v2))
            {
                return false;
            }

            var v1Bits = Bits.Of(v1).ToLongBits();
            var v2Bits = Bits.Of(v2).ToLongBits();
            return v1Bits == v2Bits;
        }

    }
    

}