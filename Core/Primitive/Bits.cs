using System;
using System.Collections.Generic;
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
        public bool IsShort   => _type == TYPE_SHORT_INTEGER;
        public bool IsInt32   => _type == TYPE_INTEGER;
        public bool IsFloat   => _type == TYPE_FLOAT;
        public bool IsDouble  => _type == TYPE_DOUBLE;
        public bool IsDecimal => _type == TYPE_DECIMAL;
        public bool IsChar    => _type == TYPE_CHAR;
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
                throw new OverflowException("");
            }
            return (byte) integer;
        }

        public int ToInt(MidpointRounding rounding = MidpointRounding.ToEven)
        {
            if (IsShort)
            {
                return BitConverter.ToInt16(_bytes, 0);
            }

            if (IsInt32)
            {
                return BitConverter.ToInt32(_bytes, 0);
            }

            if(IsChar)
            {
                return BitConverter.ToInt16(_bytes, 0);
            }

            if(IsBoolean)
            {
                return BitConverter.ToBoolean(_bytes, 0) ? 1 : 0;
            }

            if (IsLong)
            {
                var longInt = BitConverter.ToInt64(_bytes, 0);
                if (longInt < 0x80000000L || longInt > 0x7fffffffL)
                {
                    throw new OverflowException($"64 bits Integer [{longInt}] can not be converted to 32 bits Integer");
                }

                return (int) longInt;
            }

            if (IsFloat)
            {
                var value = BitConverter.ToSingle(_bytes, 0);
            }

            if (IsDouble)
            {

            }

            if (IsDecimal)
            {
                 
            }

            if (Length > BytesPerInt32)
            {
                throw new OverflowException("");
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
            
            if (IsDecimal)
            {

            }

            if (IsDouble)
            {

            }

            if (Length > BytesPerLong)
            {
                throw new OverflowException("");
            }

            return BitConverter.ToInt64(_bytes, 0);
        }

        public float ToFloat()
        {
            if (IsByte  || IsChar || IsBoolean || IsInt32 || IsShort)
            {
                return ToInt(); // Integer can always cast to float and stored correctly
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
            if (IsLong)
            {
                return Convert.ToDouble(ToLong());
            }

            if(Length > BytesPerDouble)
            {
                throw new OverflowException("");
            }

            return BitConverter.ToDouble(_bytes, 0);
        }

        public decimal ToDecimal()
        {
            if (Length > BytesPerDecimal)
            {
                throw new OverflowException();
            }
            return 0;
        }

        public char ToChar()
        {
            return  IsChar ? BitConverter.ToChar(_bytes, 0) : '0';
        }

        public bool ToBool()
        {
            return IsBoolean && BitConverter.ToBoolean(_bytes, 0);
        }

        /// <summary>
        /// Print the real presentation of primitive value
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (IsInt32 || IsShort || IsByte || IsShort)
            {
                return Convert.ToString(ToInt());
            }

            if (IsChar)
            {
                return Convert.ToString(ToChar());
            }

            if (IsBoolean)
            {
                return Convert.ToString(ToBool());
            }

            if (IsLong)
            {
                return Convert.ToString(ToLong());
            }

            if (IsDecimal)
            {

            }

            if (IsFloat)
            {

            }

            if (IsDouble)
            {

            }

            return string.Empty;
        }

        /***
         * Bitwise operator can only be applied to Integers
         */
        /// <summary>
        /// '&' Operator:
        ///  When two bits are all '1', the result bit is 1 otherwise '0'
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public Bits And(Bits bits)
        {
            return Of(ToInt() | bits.ToInt());
        }

        /// <summary>
        /// '|' Operator: when two bits are all '0', the result is '0' otherwise '1'
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public Bits Or(Bits bits)
        {
            return Of(ToInt() | bits.ToInt());
        }

        /// <summary>
        /// '^' Operator: when two bits are the same then '1' otherwise '0'
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public Bits Xor(Bits bits)
        {
            return Of(ToInt() ^ bits.ToInt());
        }

        /// <summary>
        /// '~' Operator: '1' to '0' and '0' to '1'
        /// </summary>
        /// <returns></returns>
        public Bits Not()
        {
            return Of(~ToInt());
        }

        // '<<' left shift operator
        public Bits LeftShift(int number)
        {
            return Of(ToInt() << number);
        }

        // '>>' right shift operator
        public Bits RightShift(int number)
        {
            return Of(ToInt() >> number);
        }

        public override int GetHashCode()
        {
            if (IsInt32 | IsByte | IsBoolean | IsChar | IsShort)
            {
                return ToInt();
            }

            if(IsFloat)
            {
                return BitConverter.ToInt32(_bytes, 0);
            }

            if (IsLong)
            {
                
            }

            if (IsDouble)
            {

                return (int) BitConverter.ToInt64(_bytes, 0);
            }

            return 0;
        }

        public bool Equals(Bits other)
        {
            return ToBinaryString().Equals(other.ToBinaryString()) 
                && _type == other._type;
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

        /// <summary>
        /// Convert byte array to binary string from high bit to low bit
        /// Windows is using Litter Edianess so we need to reverse the bytes
        /// </summary>
        /// <returns></returns>
        public string ToBinaryString(ByteOrder order = ByteOrder.LowerFirst)
        {
            return order == ByteOrder.LowerFirst 
                 ? Arrays.CopyOf(_bytes).Select(b => b.ToBinaryString()).AsString() 
                 : Arrays.Reverse(_bytes).Select(b => b.ToBinaryString()).AsString();
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

    }

}