﻿using System;
using System.Globalization;

namespace Core.Primitive
{
    public class DoubleConverter
    {
        /// <summary>
        /// Converts the given double to a string representation of its
        /// exact decimal value.
        /// </summary>
        /// <param name="d">The double to convert.</param>
        /// <returns>A string representation of the double's exact decimal value.</return>
        public static string ToExactString(double d)
        {
            if(double.IsPositiveInfinity(d))
                return "+Infinity";
            if(double.IsNegativeInfinity(d))
                return "-Infinity";
            if(double.IsNaN(d))
                return "NaN";

            // Translate the double into sign, exponent and mantissa.
            long bits = BitConverter.DoubleToInt64Bits(d);
            // Note that the shift is sign-extended, hence the test against -1 not 1
            bool negative = (bits < 0);
            int exponent = (int)((bits >> 52) & 0x7ffL);
            long mantissa = bits & 0xfffffffffffffL;

            
            // Subnormal numbers; exponent is effectively one higher,
            // but there's no extra normalisation bit in the mantissa
            if(exponent == 0)
            {
                exponent++;
            }
            // Normal numbers; leave exponent as it is but add extra
            // bit to the front of the mantissa
            else
            {
                mantissa = mantissa | (1L << 52);
            }

            // Bias the exponent. It's actually biased by 1023, but we're
            // treating the mantissa as m.0 rather than 0.m, so we need
            // to subtract another 52 from it.
            exponent -= 1075;

            if(mantissa == 0)
            {
                return "0";
            }

            /* Normalize */
            while((mantissa & 1) == 0)
            {    /*  i.e., Mantissa is even */
                mantissa >>= 1;
                exponent++;
            }

            /// Construct a new decimal expansion with the mantissa
            ArbitraryDecimal ad = new ArbitraryDecimal(mantissa);

            // If the exponent is less than 0, we need to repeatedly
            // divide by 2 - which is the equivalent of multiplying
            // by 5 and dividing by 10.
            if(exponent < 0)
            {
                for (int i = 0; i < -exponent; i++)
                {
                    ad.MultiplyBy(5);
                    Console.WriteLine(i + " : ");
                    foreach(var b in ad.getDigits())
                    {
                        Console.Write(b);
                    }
                    Console.WriteLine("");
                }
                
                ad.Shift(-exponent);
            }
            // Otherwise, we need to repeatedly multiply by 2
            else
            {
                for (int i = 0; i < exponent; i++)
                {
                    ad.MultiplyBy(2);
                    
                }
                
            }

           

            // Finally, return the string with an appropriate sign
            if(negative)
                return "-" + ad.ToString();
            else
                return ad.ToString();
        }

        /// <summary>Private class used for manipulating
        public class ArbitraryDecimal
        {
            /// <summary>Digits in the decimal expansion, one byte per digit
            byte[] digits;
            /// <summary> 
            /// How many digits are *after* the decimal point
            /// </summary>
            int decimalPoint = 0;

            public byte[] getDigits()
            {
                return digits;
            }

            /// <summary> 
            /// Constructs an arbitrary decimal expansion from the given long.
            /// The long must not be negative.
            /// </summary>
            internal ArbitraryDecimal(long x)
            {
                var tmp = x.ToString(CultureInfo.InvariantCulture);
                Console.WriteLine(tmp);
                Console.WriteLine(tmp);
                digits = new byte[tmp.Length];
                for (var i = 0; i < tmp.Length; i++)
                {
                    digits[i] = (byte)(tmp[i] - '0');
                }

                Console.WriteLine("");
                foreach (var b in digits)
                {
                    Console.Write(b);
                }
                Normalize();
            }

            /// <summary>
            /// Multiplies the current expansion by the given amount, which should
            /// only be 2 or 5.
            /// </summary>
            internal void MultiplyBy(int amount)
            {
                var result = new byte[digits.Length + 1];
                for(var i = digits.Length - 1; i >= 0; i--)
                {
                    var resultDigit = digits[i] * amount + result[i + 1];
                    result[i]     = (byte)(resultDigit / 10);
                    result[i + 1] = (byte)(resultDigit % 10);
                    Console.WriteLine(resultDigit + "|" + result[i] + ":" +result[i+1]);
                    Console.WriteLine("");
                }

                if(result[0] != 0)
                {
                    digits = result;
                }
                else
                {
                    Array.Copy(result, 1, digits, 0, digits.Length);
                }
                Normalize();
            }

            /// <summary>
            /// Shifts the decimal point; a negative value makes
            /// the decimal expansion bigger (as fewer digits come after the
            /// decimal place) and a positive value makes the decimal
            /// expansion smaller.
            /// </summary>
            internal void Shift(int amount)
            {
                decimalPoint += amount;
            }

            /// <summary>
            /// Removes leading/trailing zeroes from the expansion.
            /// </summary>
            internal void Normalize()
            {
                int first;
                for(first = 0; first < digits.Length; first++)
                    if(digits[first] != 0)
                        break;
                int last;
                for(last = digits.Length - 1; last >= 0; last--)
                    if(digits[last] != 0)
                        break;

                if(first == 0 && last == digits.Length - 1)
                    return;

                byte[] tmp = new byte[last - first + 1];
                for(int i = 0; i < tmp.Length; i++)
                    tmp[i] = digits[i + first];

                decimalPoint -= digits.Length - (last + 1);
                digits = tmp;

            }

            /// <summary>
            /// Converts the value to a proper decimal string representation.
            /// </summary>
            public override String ToString()
            {
                char[] digitString = new char[digits.Length];
                for (int i = 0; i < digits.Length; i++)
                {
                    Console.WriteLine("Digits => " + digits[i]);
                    digitString[i] = (char)(digits[i] + '0');
                }
                

                // Simplest case - nothing after the decimal point,
                // and last real digit is non-zero, eg value=35
                if(decimalPoint == 0)
                {
                    return new string(digitString);
                }

                // Fairly simple case - nothing after the decimal
                // point, but some 0s to add, eg value=350
                if(decimalPoint < 0)
                {
                    return new string(digitString) +
                           new string('0', -decimalPoint);
                }

                // Nothing before the decimal point, eg 0.035
                if(decimalPoint >= digitString.Length)
                {
                    return "0." +
                        new string('0', (decimalPoint - digitString.Length)) +
                        new string(digitString);
                }

                // Most complicated case - part of the string comes
                // before the decimal point, part comes after it,
                // eg 3.5
                return new string(digitString, 0,
                                   digitString.Length - decimalPoint) +
                    "." +
                    new string(digitString,
                                digitString.Length - decimalPoint,
                                decimalPoint);
            }
        }
    }
}