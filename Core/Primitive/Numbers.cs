using System;

namespace Core.Primitive
{

    public enum RoundKind
    {
        None     = 0,
        Ceil     = 1,
        Floor    = 1 << 1,
        Up       = 1 << 2,
        Down     = 1 << 3,
        HalfUp   = 1 << 4,
        HalfDown = 1 << 5,
        HalfEven = 1 << 6
    }

    public sealed class Numbers {

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

        public static unsafe string ExponentOf(float value)
        {
            Convert.ToInt32(value);
            var intBitsForFloat = *(int*)&value;
            var exponent = (intBitsForFloat >> 23) & 0x00007ff;
            return (exponent - 127).ToString();
        }

    }

    

}