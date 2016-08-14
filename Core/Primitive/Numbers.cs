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



        /// <summary>
        /// When exponent bits are all zero but significant are not all zero
        /// Then this number is denormalized number
        /// </summary>
        /// <param name="floatValue"></param>
        /// <returns></returns>

//        public static int ExponentOf(float floatValue)
//        {
//            if (IsDenormalized(floatValue))
//            {
//                return -126;
//            }
//            var intBitsForFloat = FloatToIntBits(floatValue);
//            var exponent = (intBitsForFloat >> 23) & 0x000000ff;
//            return exponent - 127;
//        }
//
//        public static int FloatToRawBits(float floatValue)
//        {
//            return BitConverter.ToInt32(BitConverter.GetBytes(floatValue), 0);
//        }
//
//        public static int FloatToIntBits(float floatValue)
//        {
//            var intRawValue = FloatToRawBits(floatValue);
//
//            // Pick up a NAN Number to represent all NAN Numbers
//            if((intRawValue & FLOAT_EXPONENT_MASK) == FLOAT_EXPONENT_MASK
//             &&(intRawValue & FLOAT_SIGNIFICANT_MASK) != 0)
//            {
//                return 0x7fc00000;
//            }
//
//            return intRawValue;
//        }
//
//        public static long DoubleToRawBits(double doubleValue)
//        {
//            return BitConverter.ToInt64(BitConverter.GetBytes(doubleValue), 0);
//        }
//
//        public static long DoubleToLongBits(double doubleValue)
//        {
//            var longRawValue = DoubleToRawBits(doubleValue);
//
//            // Pick up a NAN Number to represent all NAN Numbers
//            if((longRawValue & DOUBLE_EXPONENT_MASK) == DOUBLE_EXPONENT_MASK
//             &&(longRawValue & DOUBLE_SIGNIFICANT_MASK) != 0)
//            {
//                return 0x7ffc000000000000L;
//            }
//
//            return longRawValue;
//        }
//
//        public static bool IsNaN(float floatValue)
//        {
//            var intRawValue = BitConverter.ToInt32(BitConverter.GetBytes(floatValue), 0);
//            return (intRawValue & FLOAT_EXPONENT_MASK) == FLOAT_EXPONENT_MASK
//                && (intRawValue & FLOAT_SIGNIFICANT_MASK) != FLOAT_SIGNIFICANT_MASK;
//        }
//
//        public static bool IsNaN(double doubleValue)
//        {
//            var intRawValue = BitConverter.ToInt64(BitConverter.GetBytes(doubleValue), 0);
//            return (intRawValue & DOUBLE_EXPONENT_MASK) == DOUBLE_EXPONENT_MASK
//                && (intRawValue & DOUBLE_SIGNIFICANT_MASK) != 0;
//        }
//
//        public static bool IsDenormalized(float floatValue)
//        {
//            var intRawValue = BitConverter.ToInt32(BitConverter.GetBytes(floatValue), 0);
//            return (intRawValue & FLOAT_EXPONENT_MASK) == 0     // exponent must be '0'
//                && (intRawValue & FLOAT_SIGNIFICANT_MASK) != 0; // significant must not be '0'
//        }
//
//        public static bool IsDenormalized(double doubleValue)
//        {
//            var intRawValue = BitConverter.ToInt64(BitConverter.GetBytes(doubleValue), 0);
//            return (intRawValue & DOUBLE_EXPONENT_MASK) == 0 
//                && (intRawValue & DOUBLE_SIGNIFICANT_MASK) != 0;
//        }

    }
    

}