using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Core.Collection;

namespace Core.Primitive
{

    /// <summary>
    /// This class is used to working with byte, bool, short, char, int, float, long, double, decimal, 
    /// byte    : 8   bits which is 1  byte
    /// int     : 32  bits which is 4  bytes
    /// short   : 16  bits which is 4  bytes
    /// float   : 32  bits which is 4  bytes
    /// double  : 64  bits which is 8  bytes
    /// decimal : 128 bits which is 16 bytes
    /// char    : 16  bits which is 2  bytes
    /// long    : 64  bits which is 4  bytes
    /// bool    : 8   bits which is 1  byte
    /// 
    /// This class intend to be immutable 
    /// and operation will yield a new bit class
    /// 
    /// .Net is form lower bit to higher bi    
    /// 
    /// </summary>
    public sealed class Bits : IEquatable<Bits>
    {

        public const int BytesPerByte    = 1;
        public const int BitsPerByte     = 8;
        public const int BytesPerInt32   = 4;
        public const int BitsPerInt32    = BitsPerByte * BytesPerInt32;
        public const int BytesPerInt16   = 2;
        public const int BitsPerInt16    = BitsPerByte * BytesPerInt16;
        public const int BytesPerFloat   = 4;
        public const int BitsPerFloat    = BitsPerByte * BytesPerFloat;
        public const int BytesPerDouble  = 64;
        public const int BitsPerDouble   = BitsPerByte * BytesPerDouble;
        public const int BytesPerDecimal = 16;
        public const int BitsPerDecimal  = BitsPerByte * BytesPerDecimal;
        public const int BytesPerChar    = 2;
        public const int BitsPerChar     = BitsPerByte * BytesPerChar;
        public const int BytesPerLong    = 8;
        public const int BitsPerLong     = BitsPerByte * BytesPerLong;        
        public const int BytesPerBoolean = 1;
        public const int BitsPerBoolean  = BitsPerByte * BytesPerBoolean;

        public const int MAX_FLOAT_EXPONENT = 127;
        public const int MIN_FLOAT_EXPONENT = -126;

        public const int  FLOAT_EXPONENT_MASK     = 0x7f800000;
        public const int  FLOAT_SIGNIFICANT_MASK  = 0x007fffff;
        public const long DOUBLE_EXPONENT_MASK    = 0x7ff0000000000000L;
        public const long DOUBLE_SIGNIFICANT_MASK = 0x000fffffffffffffL;

        private const short TYPE_UNDEFINED     = -1;
        private const short TYPE_BYTE          = 0;
        private const short TYPE_SHORT_INTEGER = 1;
        private const short TYPE_INTEGER       = 1 << 1;
        private const short TYPE_LONG          = 1 << 2;
        private const short TYPE_FLOAT         = 1 << 3;
        private const short TYPE_DOUBLE        = 1 << 4;
        private const short TYPE_DECIMAL       = 1 << 5;
        private const short TYPE_CHAR          = 1 << 6;
        private const short TYPE_BOOLEAN       = 1 << 7;

        public bool IsByte    => _type == TYPE_BYTE;
        public bool IsLong    => _type == TYPE_LONG;
        public bool IsChar    => _type == TYPE_CHAR;
        public bool IsShort   => _type == TYPE_SHORT_INTEGER;
        public bool IsInt32   => _type == TYPE_INTEGER;
        public bool IsFloat   => _type == TYPE_FLOAT;
        public bool IsDouble  => _type == TYPE_DOUBLE;
        public bool IsDecimal => _type == TYPE_DECIMAL;
        public bool IsBoolean => _type == TYPE_BOOLEAN;

        public int  Length => _bytes.Length;

        private readonly byte[] _bytes;
        private short _type;

        private Bits(byte[] bytes)
        {
            Checks.NotNull(bytes);
            _bytes = bytes;
            _type  = TYPE_UNDEFINED; // means nothing
        }

        public byte ToByte()
        {
            var integer = ToInt();
            if (integer > byte.MaxValue || integer < byte.MinValue)
            {
                throw new OverflowException($"Integer : [{integer}] can not be converted to byte.");
            }
            return (byte) integer;
        }

        public int ToInt()
        {
            if (IsBoolean || IsByte)
            {
                return _bytes[0];
            }

            if (IsShort || IsChar)
            {
                return BitConverter.ToInt16(_bytes, 0);
            }

            if (IsInt32)
            {
                return BitConverter.ToInt32(_bytes, 0);
            }

            if (IsFloat)
            {
                var floatValue = BitConverter.ToSingle(_bytes, 0);
                return Numbers.GetInt(floatValue, RoundKind.Up);
            }

            if (IsDouble)
            {
                var doubleValue = BitConverter.ToDouble(_bytes, 0);
                var longValue = (long) doubleValue;
                return (int) longValue;
            }

            if (IsLong)
            {
                var longBits = BitConverter.ToInt64(_bytes, 0);
                if (longBits < int.MinValue || longBits > int.MaxValue)
                {
                    throw new OverflowException($"64 bits Integer [{longBits}] can not be converted to 32 bits Integer");
                }

                return (int) longBits;
            }

            if (IsDecimal)
            {
                return Convert.ToInt32(Numbers.GetDecimal(_bytes));
            }

            if (Length > BytesPerInt32)
            {
                throw new OverflowException("Integer can only hold 32 bits");
            }

            return BitConverter.ToInt32(_bytes, 0);
        }

        public long ToLong()
        {
            if (IsLong)
            {
                return BitConverter.ToInt64(_bytes, 0);
            }

            if (IsInt32 || IsByte || IsShort || IsChar || IsBoolean || IsFloat)
            {
                return ToInt();
            }
            
            if (IsDouble)
            {
                var doubleValue = BitConverter.ToDouble(_bytes, 0);
                var longValue = (long) doubleValue;
                return (int)longValue;
            }

            if (IsDecimal)
            {
                return Convert.ToInt64(Numbers.GetDecimal(_bytes));
            }

            if (Length > BytesPerLong)
            {
                throw new OverflowException("");
            }

            return BitConverter.ToInt64(_bytes, 0);
        }

        public float ToFloat()
        {
            if (IsByte || IsChar || IsBoolean || IsInt32 || IsShort)
            {
                return float.Parse(ToInt().ToString());
            }

            if (IsFloat)
            {
                return BitConverter.ToSingle(_bytes, 0);
            }

            if (IsDouble || IsLong)
            {
                return Convert.ToSingle(ToDouble());
            }

            if (Length > BytesPerFloat)
            {
                throw new OverflowException("");
            }

            return BitConverter.ToSingle(_bytes, 0);
        }

        public double ToDouble()
        {
            if (IsDouble)
            {
                return BitConverter.ToDouble(_bytes, 0);
            }

            if (IsInt32 || IsBoolean || IsByte || IsShort || IsChar)
            {
                return Convert.ToDouble(ToInt());
            }

            if (IsLong)
            {
                return Convert.ToDouble(ToLong());
            }

            if (IsFloat)
            {
                return Convert.ToDouble(ToFloat());
            }

            if (IsDecimal)
            {
                return Convert.ToDouble(Numbers.GetDecimal(_bytes));
            }

            if (Length > BytesPerDouble)
            {
                throw new OverflowException($"{Length} is out of range.");
            }

            return BitConverter.ToDouble(_bytes, 0);
        }

        public decimal ToDecimal()
        {
            if (IsDecimal)
            {
                return Numbers.GetDecimal(_bytes);
            }

            if (IsBoolean || IsByte || IsChar || IsShort || IsInt32)
            {
               return new decimal(ToInt());
            }

            if (IsLong)
            {
                return new decimal(ToLong());
            }

            if (IsFloat)
            {
                return new decimal(ToFloat());
            }

            if (IsDouble)
            {
                return new decimal(ToDouble());
            }

            if (Length > BytesPerDecimal)
            {
                throw new OverflowException();
            }
            return 0;
        }

        public char ToChar()
        {
            if (IsChar || IsShort)
            {
                return BitConverter.ToChar(_bytes, 0);
            }

            if (IsByte || IsBoolean)
            {
                return (char)ToInt();
            }

            if (IsInt32 || IsLong)
            {
                var integral = ToInt();
                if (integral < char.MinValue || integral > char.MaxValue)
                {
                    throw new OverflowException();
                }
                return (char) integral;
            }

            if (IsDecimal || IsFloat || IsDouble)
            {
                throw new InvalidCastException("Can not cast float or double to char");
            }

            return  IsChar ? BitConverter.ToChar(_bytes, 0) : '0';
        }

        public bool ToBool()
        {
            return IsBoolean && BitConverter.ToBoolean(_bytes, 0);
        }

        public int ToIntBits()
        {
            if(IsFloat)
            {
                return FloatToIntBits(BitConverter.ToSingle(_bytes, 0));
            }

            if (IsDouble)
            {
                var longBits = DoubleToLongBits(ToDouble());
                if(longBits < int.MinValue || longBits > int.MaxValue)
                {
                    throw new OverflowException($"64 bits Integer [{longBits}] can not be converted to 32 bits Integer");
                }

                return (int)longBits;
            }

            return ToInt();
        }

        public long ToLongBits()
        {
            if (IsFloat)
            {
                return FloatToIntBits(BitConverter.ToSingle(_bytes, 0));
            }

            if (IsDouble)
            {
                return DoubleToLongBits(ToDouble());
            }

            return ToLong();
        }

        /// <summary>
        /// Print the real presentation of primitive value
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (IsInt32 || IsShort || IsByte )
            {
                return Convert.ToString(ToInt());
            }

            if(IsLong)
            {
                return Convert.ToString(ToLong());
            }

            if (IsChar)
            {
                return Convert.ToString(ToChar());
            }

            if (IsBoolean)
            {
                return Convert.ToString(ToBool());
            }

            if (IsDecimal)
            {
                return ToDecimal().ToString(CultureInfo.InvariantCulture);
            }

            if (IsFloat)
            {
                return Convert.ToString(ToFloat(), CultureInfo.InvariantCulture);
            }

            if (IsDouble)
            {
                return Convert.ToString(ToDouble(), CultureInfo.InvariantCulture);
            }

            return string.Empty;
        }

        /// <summary>
        /// Convert byte array to binary string from high bit to low bit
        /// Windows is using Litter Edianess so we need to reverse the bytes
        /// </summary>
        /// <returns></returns>
        public string ToBinaryString(ByteOrder order = ByteOrder.HigherBitsFirst)
        {
            return order == ByteOrder.LowerBitsFirst
                 ? Arrays.CopyOf(_bytes).Select(b => b.ToBinaryString()).AsString()
                 : Arrays.Reverse(_bytes).Select(b => b.ToBinaryString()).AsString();
        }

        public bool Equals(Bits other)
        {
            return ToBinaryString().Equals(other.ToBinaryString()) && _type == other._type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var b = obj as Bits;
            return b != null && Equals(b);
        }

        public override int GetHashCode()
        {
            if (IsInt32 | IsByte | IsBoolean | IsChar | IsShort)
            {
                return ToInt();
            }

            if (IsFloat)
            {
                return FloatToIntBits(ToFloat());
            }

            if (IsDouble)
            {
                var longValue = DoubleToLongBits(ToDouble());
                return (int)(longValue ^ (longValue >> 32));
            }

            if (IsLong)
            {
                var longValue = BitConverter.ToInt64(_bytes, 0);
                return (int)(longValue ^ (longValue >> 32));
            }

            return 0;
        }

        public static Bits Of(byte value)
        {
            return new Bits(new[] { value })
            {
                _type = TYPE_BYTE
            };
        }

        public static Bits Of(bool value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _type = TYPE_BOOLEAN
            };
        }

        public static Bits Of(short value)
        {
            return new Bits(null)
            {
                _type = TYPE_SHORT_INTEGER
            };
        }

        public static Bits Of(char value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _type = TYPE_CHAR
            };
        }

        public static Bits Of(int value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _type = TYPE_INTEGER
            };
        }

        public static Bits Of(float value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _type = TYPE_FLOAT
            };
        }

        public static Bits Of(long value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _type = TYPE_LONG
            };
        }

        public static Bits Of(double value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _type = TYPE_DOUBLE
            };
        }
        
        public static Bits Of(decimal value)
        {
            var bytes = new List<byte>();
            foreach(var integral in decimal.GetBits(value))
            {
                bytes.AddRange(BitConverter.GetBytes(integral));
            }

            return new Bits(bytes.ToArray())
            {
                _type = TYPE_DECIMAL
            };
        }

        public static int GetExponent(float floatValue)
        {
            if (IsDenormalized(floatValue))
            {
                return -126;
            }

            var intBitsForFloat = FloatToIntBits(floatValue);
            var exponent = (intBitsForFloat >> 23) & 0x000000ff;
            return exponent - 127;
        }

        public static bool IsNaN(float floatValue)
        {
            var intRawValue = BitConverter.ToInt32(BitConverter.GetBytes(floatValue), 0);
            return (intRawValue & FLOAT_EXPONENT_MASK) == FLOAT_EXPONENT_MASK
                && (intRawValue & FLOAT_SIGNIFICANT_MASK) != FLOAT_SIGNIFICANT_MASK;
        }

        public static bool IsNaN(double doubleValue)
        {
            var intRawValue = BitConverter.ToInt64(BitConverter.GetBytes(doubleValue), 0);
            return (intRawValue & DOUBLE_EXPONENT_MASK) == DOUBLE_EXPONENT_MASK
                && (intRawValue & DOUBLE_SIGNIFICANT_MASK) != 0;
        }

        public static bool IsDenormalized(float floatValue)
        {
            var intRawValue = BitConverter.ToInt32(BitConverter.GetBytes(floatValue), 0);
            return (intRawValue & FLOAT_EXPONENT_MASK) == 0     // exponent must be '0'
                && (intRawValue & FLOAT_SIGNIFICANT_MASK) != 0; // significant must not be '0'
        }

        public static bool IsDenormalized(double doubleValue)
        {
            var intRawValue = BitConverter.ToInt64(BitConverter.GetBytes(doubleValue), 0);
            return (intRawValue & DOUBLE_EXPONENT_MASK) == 0
                && (intRawValue & DOUBLE_SIGNIFICANT_MASK) != 0;
        }

        private static int FloatToRawBits(float floatValue)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(floatValue), 0);
        }

        private static int FloatToIntBits(float floatValue)
        {
            var intRawValue = FloatToRawBits(floatValue);

            // Pick up a NAN Number to represent all NAN Numbers
            if((intRawValue  & FLOAT_EXPONENT_MASK) == FLOAT_EXPONENT_MASK
             &&(intRawValue & FLOAT_SIGNIFICANT_MASK) != 0)
            {
                return 0x7fc00000;
            }

            return intRawValue;
        }

        private static long DoubleToRawBits(double doubleValue)
        {
            return BitConverter.ToInt64(BitConverter.GetBytes(doubleValue), 0);
        }

        private static long DoubleToLongBits(double doubleValue)
        {
            var longRawValue = DoubleToRawBits(doubleValue);

            // Pick up a NAN Number to represent all NAN Numbers
            if((longRawValue & DOUBLE_EXPONENT_MASK) == DOUBLE_EXPONENT_MASK
             &&(longRawValue & DOUBLE_SIGNIFICANT_MASK) != 0)
            {
                return 0x7ffc000000000000L;
            }

            return longRawValue;
        }

    }

}