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

        public const int MaxFloatExponent = 127;
        public const int MinFloatExponent = -126;

        public const int  FloatExponentMask     = 0x7f800000;
        public const int  FloatSignificantMask  = 0x007fffff;
        public const int  FloatNanBits          = 0x7fc00000;
        public const long DoubleExponentMask    = 0x7ff0000000000000L;
        public const long DoubleSignificantMask = 0x000fffffffffffffL;
        public const long DoubleNanBits         = 0x7ffc000000000000L;

        private const short TypeUndefined     = -1;
        private const short TypeByte          = 0;
        private const short TypeShortInteger  = 1;
        private const short TypeInteger       = 1 << 1;
        private const short TypeLong          = 1 << 2;
        private const short TypeFloat         = 1 << 3;
        private const short TypeDouble        = 1 << 4;
        private const short TypeDecimal       = 1 << 5;
        private const short TypeChar          = 1 << 6;
        private const short TypeBoolean       = 1 << 7;

        public bool IsByte    => _type == TypeByte;
        public bool IsLong    => _type == TypeLong;
        public bool IsChar    => _type == TypeChar;
        public bool IsShort   => _type == TypeShortInteger;
        public bool IsInt32   => _type == TypeInteger;
        public bool IsFloat   => _type == TypeFloat;
        public bool IsDouble  => _type == TypeDouble;
        public bool IsDecimal => _type == TypeDecimal;
        public bool IsBoolean => _type == TypeBoolean;

        public int  Length => _bytes.Length;

        private readonly byte[] _bytes;
        private short _type;

        private Bits(byte[] bytes)
        {
            Checks.NotNull(bytes);
            _bytes = bytes;
            _type  = TypeUndefined;
        }

        /// <summary>
        /// Returns byte array
        /// </summary>
        /// <returns><code>byte[]</code></returns>
        public byte[] ToBytes()
        {
            return Arrays.CopyOf(_bytes);
        }

        /// <summary>
        /// Returns char of this bits
        /// </summary>
        /// <returns><code>char</code></returns>
        public char ToChar()
        {
            if (IsChar || IsShort || IsByte)
            {
                return (char) ToInt();
            }

            if (IsInt32)
            {
                var intValue = ToInt();
                if (intValue < char.MinValue || intValue > char.MaxValue)
                {
                    throw new OverflowException("Value was either too large or too small for an char.");
                }
                return (char) intValue;
            }

            if (IsLong)
            {
                var longValue = ToLong();
                if (longValue < char.MinValue || longValue > char.MaxValue)
                {
                    throw new OverflowException("Value was either too large or too small for an char.");
                }
                return (char)longValue;
            }

            throw new ArgumentException("Value was either too large or too small for an char.");
        }

        /// <summary>
        /// Returns boolean representation
        /// </summary>
        /// <returns><code>bool</code></returns>
        public bool ToBool()
        {
            if (IsBoolean)
            {
                return BitConverter.ToBoolean(_bytes, 0);
            }
            return false;
        }

        /// <summary>
        /// Returns integer representation
        /// </summary>
        /// <param name="kind"><code>RoundKind</code></param>
        /// <returns><code>int</code></returns>
        public int ToInt(RoundKind kind = RoundKind.HalfUp)
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
                return Numbers.GetInt(ToFloat(), kind);
            }

            if (IsDouble)
            {
                return Numbers.GetInt(ToDouble(), kind);
            }

            if (IsLong)
            {
                return Numbers.GetInt(ToLong());
            }

            if (IsDecimal)
            {
                return Numbers.GetInt(ToDecimal(), kind);
            }

            if (Length != BytesPerInt32)
            {
                throw new OverflowException("Value was either too large or too small for an Int32.");
            }

            return BitConverter.ToInt32(_bytes, 0);
        }

        /// <summary>
        /// Returns long representation
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public long ToLong(RoundKind kind = RoundKind.HalfUp)
        {
            if (IsLong)
            {
                return BitConverter.ToInt64(_bytes, 0);
            }

            if (IsInt32 || IsByte || IsShort || IsChar || IsBoolean)
            {
                return ToInt();
            }

            if (IsFloat)
            {
                return Numbers.GetLong(ToFloat(), kind);
            }

            if (IsDouble)
            {
                return Numbers.GetLong(ToDouble(), kind);
            }

            if (IsDecimal)
            {
                return (long) ToDecimal();
            }

            if (Length != BytesPerLong)
            {
                throw new OverflowException("Value was either too large or too small for an Long.");
            }

            return BitConverter.ToInt64(_bytes, 0);
        }

        /// <summary>
        /// Returns float representation
        /// </summary>
        /// <returns></returns>
        public float ToFloat()
        {
            if (IsFloat)
            {
                return BitConverter.ToSingle(_bytes, 0);
            }

            if (IsByte || IsChar || IsBoolean || IsShort || IsInt32)
            {
                return ToInt();
            }

            if (IsLong)
            {
                return ToLong();
            }

            if (IsDouble)
            {
                return Numbers.GetFloat(ToDouble());
            }

            if (IsDecimal)
            {
                return Numbers.GetFloat(ToDecimal());
            }

            if (Length != BytesPerFloat)
            {
                throw new OverflowException("Value was either too large or too small for an Float");
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
                return ToInt();
            }

            if (IsLong)
            {
                return ToLong();
            }

            if (IsFloat)
            {
                return Numbers.GetDouble(ToFloat());
            }

            if (IsDecimal)
            {
                return Numbers.GetDouble(ToDecimal());
            }

            if (Length != BytesPerDouble)
            {
                throw new OverflowException("Value was either too large or too small for an Double.");
            }

            return BitConverter.ToDouble(_bytes, 0);
        }

        /// <summary>
        /// Returns decimal representation
        /// </summary>
        /// <returns></returns>
        public decimal ToDecimal()
        {
            if (IsDecimal)
            {
                return new decimal(new []
                {
                    BitConverter.ToInt32(new[] { _bytes[0],  _bytes[1],  _bytes[2],  _bytes[3]  }, 0),
                    BitConverter.ToInt32(new[] { _bytes[4],  _bytes[5],  _bytes[6],  _bytes[7]  }, 0),
                    BitConverter.ToInt32(new[] { _bytes[8],  _bytes[9],  _bytes[10], _bytes[11] }, 0),
                    BitConverter.ToInt32(new[] { _bytes[12], _bytes[13], _bytes[14], _bytes[15] }, 0)
                });
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

            throw new OverflowException("Value was either too large or too small for an Decimal.");
        }

        /// <summary>
        /// Returns Int bits
        /// </summary>
        /// <returns></returns>
        public IntBits ToIntBits()
        {
            if (IsFloat)
            {
                return IntBits.Of(FloatToIntBits(ToFloat()));
            }

            if (IsLong)
            {
                var longBits = ToLong();
                var bytes    = BitConverter.GetBytes(longBits);
                var lowInt   = BitConverter.ToInt32(new[] { bytes[0], bytes[1], bytes[2], bytes[3] }, 0);
                var highInt  = BitConverter.ToInt32(new[] { bytes[4], bytes[5], bytes[6], bytes[7] }, 0);
                return IntBits.Of(lowInt, highInt);
            }

            if (IsDouble)
            {
                var longBits = DoubleToLongBits(ToDouble());
                var bytes    = BitConverter.GetBytes(longBits);
                var lowInt   = BitConverter.ToInt32(new[] { bytes[0], bytes[1], bytes[2], bytes[3] }, 0);
                var highInt  = BitConverter.ToInt32(new[] { bytes[4], bytes[5], bytes[6], bytes[7] }, 0);
                return IntBits.Of(lowInt, highInt);
            }

            return IsDecimal 
                 ? IntBits.Of(decimal.GetBits(ToDecimal())) 
                 : IntBits.Of(ToInt())
                 ;
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

            return ToBinaryString();
        }
        
        /// <summary>
        /// Convert byte array to binary string from high bit to low bit
        /// Windows is using Litter Edianess so we need to reverse the bytes
        /// </summary>
        /// <returns></returns>
        public string ToBinaryString(ByteOrder order = ByteOrder.BigEndian)
        {
            return order == ByteOrder.LittleEndian
                 ? Arrays.CopyOf(_bytes).Select(b  => b.ToBinaryString()).AsString()
                 : Arrays.Reverse(_bytes).Select(b => b.ToBinaryString()).AsString();
        }

        public bool Equals(Bits other)
        {
            return _type == other._type && ToBinaryString().Equals(other.ToBinaryString());
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
                _type = TypeByte
            };
        }

        public static Bits Of(bool value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _type = TypeBoolean
            };
        }

        public static Bits Of(short value)
        {
            return new Bits(null)
            {
                _type = TypeShortInteger
            };
        }

        public static Bits Of(char value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _type = TypeChar
            };
        }

        public static Bits Of(int value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _type = TypeInteger
            };
        }

        public static Bits Of(float value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _type = TypeFloat
            };
        }

        public static Bits Of(long value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _type = TypeLong
            };
        }

        public static Bits Of(double value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _type = TypeDouble
            };
        }
        
        public static Bits Of(decimal value)
        {
            var bytes = new List<byte>();
            decimal.GetBits(value).ForEach(integer =>
            {
                bytes.AddRange(BitConverter.GetBytes(integer));
            });

            return new Bits(bytes.ToArray())
            {
                _type = TypeDecimal
            };
        }

        public static int GetExponent(float floatValue)
        {
            if (IsDenormalized(floatValue))
            {
                return -126;
            }

            return ((FloatToIntBits(floatValue) >> 23) & 0x000000ff) - 127;
        }

        public static bool IsNaN(float floatValue)
        {
            var intRawValue = BitConverter.ToInt32(BitConverter.GetBytes(floatValue), 0);
            return (intRawValue & FloatExponentMask) == FloatExponentMask 
                && (intRawValue & FloatSignificantMask) != FloatSignificantMask;
        }

        public static bool IsNaN(double doubleValue)
        {
            var intRawValue = BitConverter.ToInt64(BitConverter.GetBytes(doubleValue), 0);
            return (intRawValue & DoubleExponentMask) == DoubleExponentMask
                && (intRawValue & DoubleSignificantMask) != 0;
        }

        public static bool IsDenormalized(float floatValue)
        {
            var intRawValue = BitConverter.ToInt32(BitConverter.GetBytes(floatValue), 0);
            return (intRawValue & FloatExponentMask) == 0     // exponent must be '0'
                && (intRawValue & FloatSignificantMask) != 0; // significant must not be '0'
        }

        public static bool IsDenormalized(double doubleValue)
        {
            var intRawValue = BitConverter.ToInt64(BitConverter.GetBytes(doubleValue), 0);
            return (intRawValue & DoubleExponentMask) == 0
                && (intRawValue & DoubleSignificantMask) != 0;
        }

        private static int FloatToRawBits(float floatValue)
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(floatValue), 0);
        }

        public static int FloatToIntBits(float floatValue)
        {
            var intRawValue = FloatToRawBits(floatValue);

            // Pick up a NAN Number to represent all NAN Numbers
            if((intRawValue & FloatExponentMask) == FloatExponentMask && (intRawValue & FloatSignificantMask) != 0)
            {
                return FloatNanBits;
            }

            return intRawValue;
        }

        private static long DoubleToRawBits(double doubleValue)
        {
            return BitConverter.ToInt64(BitConverter.GetBytes(doubleValue), 0);
        }

        public static long DoubleToLongBits(double doubleValue)
        {
            var longRawValue = DoubleToRawBits(doubleValue);

            // Pick up a NAN Number to represent all NAN Numbers
            if((longRawValue & DoubleExponentMask) == DoubleExponentMask 
            && (longRawValue & DoubleSignificantMask) != 0)
            {
                return DoubleNanBits;
            }

            return longRawValue;
        }
        


    }

}