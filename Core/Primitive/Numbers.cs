using System;

namespace Core.Primitive
{

    public enum RoundKind
    {
        /// <summary>
        /// No rounding, cut off all the digits after decimal point 
        /// </summary>
        None = 0,      

        /// <summary>
        /// The smallest interger greater or equal to current number
        /// </summary>
        Ceil = 1,

        /// <summary>
        /// The largest integer smaller or equal to current number
        /// </summary>
        Floor = 1 << 1,

        /// <summary>
        /// When there is a half of 1, then use the smallest integer greater than current number 
        /// </summary>
        HalfUp = 1 << 2,

        /// <summary>
        /// when there is a half of 1, then use the largest integer smaller than current number 
        /// </summary>
        HalfDown = 1 << 3,

        /// <summary>
        /// when there is a half of 1, then use nearest even number 
        /// </summary>
        HalfEven = 1 << 4  
    }

    public sealed class Numbers
    {

        public const int  FloatExponentMask     = 0x7f800000;
        public const int  FloatSignificantMask  = 0x007fffff;
        public const long DoubleExponentMask    = 0x7ff0000000000000L;
        public const long DoubleSignificantMask = 0x000fffffffffffffL;

        public const int    FloatSignMask   = int.MinValue;
        public const float  FloatHalfOfOne  = 0.5f;
        public const double DoubleHalfOfOne = 0.5d;

        public static bool Equals(float v1, float v2)
        {
            if (Bits.IsNaN(v1) || Bits.IsNaN(v2))
            {
                return false;
            }

            var v1Bits = Bits.Of(v1).ToIntBits();
            var v2Bits = Bits.Of(v2).ToIntBits();
            return v1Bits.Equals(v2Bits);
        }

        public static bool Equals(double v1, double v2)
        {
            if (Bits.IsNaN(v1) || Bits.IsNaN(v2))
            {
                return false;
            }

            var v1Bits = Bits.Of(v1).ToIntBits();
            var v2Bits = Bits.Of(v2).ToIntBits();
            return v1Bits.Equals(v2Bits);
        }

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
            return int.TryParse(value, out result) ? result : defaultValue;
        }

        public static int GetInt(long longValue)
        {
            return checked((int) longValue);
        }

        public static int GetInt(float value, RoundKind round = RoundKind.HalfUp)
        {

            if (value <= -2147483649f || value >= 2147483648f)
            {
                throw new OverflowException("Value was either too large or too small for an Int32.");
            }

            var intValue = (int)value;
            var diff = Math.Abs(value - intValue);
            if (Equals(diff, 0f) || intValue == int.MinValue || intValue == int.MaxValue)
            {
                return intValue;
            }

            switch (round)
            {
                case RoundKind.Ceil :
                {
                    return value.IsNegative() || intValue == int.MaxValue ? intValue : 1 + intValue;
                }
                case RoundKind.Floor:
                {
                    return value.IsNegative() && intValue != int.MinValue ? intValue - 1 : intValue;
                }
                case RoundKind.HalfUp:
                {
                    return diff > FloatHalfOfOne || Equals(diff, FloatHalfOfOne) ? intValue + 1 : intValue;
                }
                case RoundKind.HalfDown:
                {
                    return diff < FloatHalfOfOne || Equals(diff, FloatHalfOfOne) ? intValue : intValue + 1;
                }
                case RoundKind.HalfEven:
                {
                    if (Equals(diff, FloatHalfOfOne))
                    {
                        return (intValue & 1) == 0 ? intValue : intValue + 1;
                    }
                    return diff > FloatHalfOfOne ? intValue + 1 : intValue;
                }
                default : return intValue;
            }            
        }

        public static int GetInt(decimal value, RoundKind round)
        {
            if(value <= int.MinValue || value >= int.MaxValue)
            {
                throw new OverflowException("Value was either too large or too small for an Int64.");
            }

            var intValue = decimal.ToInt32(value);
            var diff = Math.Abs(value - intValue);
            switch(round)
            {
                case RoundKind.Ceil:
                {
                    return (value < 0) || diff == 0m ? intValue : 1 + intValue;
                }
                case RoundKind.Floor:
                {
                    return value < 0 ? intValue - 1 : intValue;
                }
                case RoundKind.HalfUp:
                {
                    return diff >= 0.5m ? intValue + 1 : intValue;
                }
                case RoundKind.HalfDown:
                {
                    return diff <= 0.5m ? intValue : intValue + 1;
                }
                case RoundKind.HalfEven:
                {
                    if(diff == 0.5m)
                    {
                        return (intValue & 1) == 0 ? intValue : intValue + 1;
                    }
                    return diff > 0.5m ? intValue + 1 : intValue;
                }
                default:
                return intValue;
            }
        }

        public static float GetFloat(double doubleValue)
        {
            return 0f;
        }

        public static float GetFloat(decimal decimalValue)
        {
            return 0f;
        }

        public static float GetFloat(string value)
        {
            return GetFloat(value, 0f);
        }

        public static float GetFloat(string value, float defaultValue)
        {
            float result;
            return float.TryParse(value, out result) ? result : defaultValue;
        }

        public static double GetDouble(float floatValue)
        {
            return 0d;
        }

        public static double GetDouble(decimal decimalValue)
        {
            return 0d;
        }

        public static double GetDouble(string value)
        {
            return GetDouble(value, 0d);
        }

        public static double GetDouble(string value, double defaultValue)
        {
            double result;
            return double.TryParse(value, out result) ? result : defaultValue;
        }

        public static decimal GetDecimal(int value)
        {
            return new decimal(value);
        }

        public static decimal GetDecimal(string value)
        {
            return GetDecimal(value, decimal.Zero);
        }

        public static decimal GetDecimal(string value, decimal defaultValue)
        {
            decimal result;
            return decimal.TryParse(value, out result) ? result : defaultValue;
        }
 
        public static int GetInt(double value, RoundKind kind)
        {
            return 0;
        }

        public static long GetLong(float value, RoundKind round)
        {
            return (long)value;
        }

        public static long GetLong(double value, RoundKind round)
        {
            if (value <= long.MinValue || value >= long.MaxValue)
            {
                throw new OverflowException("Value was either too large or too small for an Int64.");
            }

            var longValue = (long) value;
            var diff = Math.Abs(value - longValue);
            switch (round)
            {
                case RoundKind.Ceil:
                {
                    return (value < 0) || Equals(diff, 0d) ? longValue : 1 + longValue;
                }
                case RoundKind.Floor:
                {
                    return value < 0 ? longValue - 1 : longValue;
                }
                case RoundKind.HalfUp:
                {
                    return diff > 0.5d || Equals(diff, 0.5d) ? longValue + 1 : longValue;
                }
                case RoundKind.HalfDown:
                {
                    return diff < 0.5d || Equals(diff, 0.5d) ? longValue : longValue + 1;
                }
                case RoundKind.HalfEven:
                {
                    if (Equals(diff, 0.5d))
                    {
                        return (longValue & 1) == 0 ? longValue : longValue + 1;
                    }
                    return diff > 0.5d ? longValue + 1 : longValue;
                }
                default:
                return longValue;
            }
        }
       
        public static long GetLong(decimal value, RoundKind round)
        {
            if(value <= long.MinValue || value >= long.MaxValue)
            {
                throw new OverflowException("Value was either too large or too small for an Int64.");
            }

            var longValue = decimal.ToInt64(value);
            var diff = Math.Abs(value - longValue);

            switch (round)
            {
                case RoundKind.Ceil:
                {
                    return (value < 0) || diff == 0 ? longValue : 1 + longValue;
                }
                case RoundKind.Floor:
                {
                    return value < 0 ? longValue - 1 : longValue;
                }
                case RoundKind.HalfUp:
                {
                    return diff > 0.5m || diff == 0.5m ? longValue + 1 : longValue;
                }
                case RoundKind.HalfDown:
                {
                    return diff < 0.5m || diff == 0.5m ? longValue : longValue + 1;
                }
                case RoundKind.HalfEven:
                {
                    if(Equals(diff, 0.5m))
                    {
                        return (longValue & 1) == 0 ? longValue : longValue + 1;
                    }
                    return diff > 0.5m ? longValue + 1 : longValue;
                }
                default:
                return longValue;
            }
        }

      
        public static string GetBinaryString(int value)
        {
            return Bits.Of(value).ToBinaryString();
        }


    }


}