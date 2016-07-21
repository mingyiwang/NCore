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

        public static string BinaryOf(int value) {
            return Bits.From(value).ToString();
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

        public static float FloatOf(int value)
        {
            return 0f;
        }

        public static float FloatOf(double value)
        {
            return 0f;
        }

        public static double DoubleOf(int value)
        {
            return 0d;
        }

        public static double DoubleOf(float value)
        {
            return 0d;
        }

        public static decimal DecimalOf(int value)
        {
            return 0;
        }

        public static decimal DecimalOf(float value)
        {
            return 0;
        }

        public static decimal DecimalOf(double value)
        {
            return 0;
        }

        public static string BinaryOf(byte value)
        {
            var bits = new Stack<int>(OS.BitsPerByte);
            int quotient;
            int input = value;
            do
            {
                int remainder;
                quotient = Math.DivRem(input, 2, out remainder);
                input = quotient;
                bits.Push(remainder);
            }
            while(
                quotient != 0
            );

            var count = bits.Count;
            if(count >= OS.BitsPerByte)
            {
                return bits.AsString();
            }

            for(var index = 1; index <= (OS.BitsPerByte - count); index++)
            {
                bits.Push(0);
            }

            return bits.AsString();
        }

    }

    public enum RoundKind
    {
        Ceil,
        Floor,
        None
    }

}