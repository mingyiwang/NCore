using System;
using System.Collections.Generic;
using Core.Collection;
using Core.Concurrent;

namespace Core.Primitive
{
   
    public sealed class Numbers {
       
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
            return parsed ? result : defaultValue;
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

    }

    public enum RoundKind
    {
        Ceil,
        Floor,
        None
    }

}