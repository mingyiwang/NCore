using System;
using System.Collections.Generic;
using System.Linq;
using Core.Collection;
using Core.Concurrent;

namespace Core.Primitive
{

    /// <summary>
    /// This class is used to working with 
    ///  int, short, long, float, double, decimal, char, bool
    /// int     : 32  bits which is 4  bytes
    /// short   : 16  bits which is 4  bytes
    /// float   : 32  bits which is 4  bytes
    /// double  : 64  bits which is 8  bytes
    /// decimal : 128 bits which is 16 bytes
    /// char    : 16  bits which is 2  bytes
    /// long    : 64  bits which is 4  bytes
    /// bool    : 8   bits which is 1  byte
    /// This class intend to be immutable 
    /// and operation will yield a new bit class
    /// </summary>
    public sealed class Bits : IEquatable<Bits>, IComparable<Bits>, IComparable
    {

        public const int BitsPerByte    = 8;
        public const int BitsPerInt32   = 32;
        public const int BitsPerInt16   = 16;
        public const int BitsPerFloat   = 32;
        public const int BitsPerDouble  = 64;
        public const int BitsPerDecimal = 128;
        public const int BitsPerChar    = 16;
        public const int BitsPerLong    = 64;
        public const int BitsPerBoolean = 8;

        private const short TYPE_BYTE          = 0;
        private const short TYPE_SHORT_INTEGER = 1;
        private const short TYPE_INTEGER       = 1 << 1;
        private const short TYPE_LONG          = 1 << 2;
        private const short TYPE_FLOAT         = 1 << 3;
        private const short TYPE_DOUBLE        = 1 << 4;
        private const short TYPE_DECIMAL       = 1 << 5;
        private const short TYPE_CHAR          = 1 << 6;
        private const short TYPE_BOOLEAN       = 1 << 7;

        public bool IsByte    {
            get
            {
                return _typeInfo == TYPE_BYTE;
            } 
        }

        public bool IsLong    {
            get
            {
                return _typeInfo == TYPE_LONG;
            }
        }

        public bool IsShort   {
            get
            {
                return _typeInfo == TYPE_SHORT_INTEGER;
            }
        }

        public bool IsInt32   {
            get
            {
                return _typeInfo == TYPE_INTEGER;
            }
        }

        public bool IsFloat   {
            get
            {
                return _typeInfo == TYPE_FLOAT;
            }
        }

        public bool IsDouble
        {
            get
            {
                return _typeInfo == TYPE_DOUBLE;
            }
        }

        public bool IsDecimal {
            get
            {
                return _typeInfo == TYPE_DECIMAL;
            }
        }

        public bool IsChar    {
            get
            {
                return _typeInfo == TYPE_CHAR;
            }
        }

        public bool IsBoolean {
            get
            {
                return _typeInfo == TYPE_BOOLEAN;
            }
        }

        private short _typeInfo;
        private readonly byte[] _bytes;

        private Bits(byte[] bytes)
        {
            _bytes = bytes;
            _typeInfo = 0; // means nothing
        }

        public byte ToByte()
        {
            return 0;
        }

        public int ToInt()
        {
            // 1. For Float, Double and decimal we round to a integer
            // 2. For Long, we return integer or overflow 
            // 3. For char and boolean we can safely return the integer
            // 4. For Short we return or overflow
            return BitConverter.ToInt32(_bytes, 0);
        }

        public long ToLong()
        {
            return 0;
        }

        public float ToFloat()
        {
            //1. For Int, Float, Short, and char, boolean we can safely returns float
            //2. For double and long we return float or overflow
            return BitConverter.ToSingle(_bytes, 0);
        }

        public double ToDouble()
        {
            return BitConverter.ToSingle(_bytes, 0);
        }

        public decimal ToDecimal()
        {
            // we can convert to long then decimal
            return 0;
        }

        public char ToChar()
        {
            return (char) 0;
        }

        public bool ToBool()
        {
            return true;
        }

        public Bits Or(Bits bits)
        {
            return null;
        }

        public Bits And(Bits bits)
        {
            return null;
        }

        public Bits Xor(Bits bits)
        {
            return null;
        }

        public Bits Not()
        {
            return null;
        }

        public Bits Plus(Bits bits)
        {
            return null;
        }

        public Bits Minus(Bits bits)
        {
            return null;
        }

        public Bits Set(int index)
        {
            return null;
        }

        public Bits Clear(int index)
        {
            Checks.True(index < 32 && index > 0, "index is out of range.");
            return null;
        }

        public override int GetHashCode()
        {
            return ToInt();
        }

        public bool Equals(Bits other)
        {
            return true;
        }

        public override bool Equals(object obj)
        {
            if(ReferenceEquals(obj, null))
            {
                return false;
            }

            if(ReferenceEquals(this, obj))
            {
                return true;
            }

            var b = obj as Bits;
            return b != null && Equals(b);
        }

        public int CompareTo(Bits other)
        {
            return 0;
        }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return OS.IsLittleEndian
                 ? Arrays.Reverse(_bytes).Select(ToBinaryString).AsString()
                 : Arrays.CopyOf(_bytes).Select(ToBinaryString).AsString();
        }

        public static Bits Of(byte value)
        {
            return new Bits(null)
            {
                _typeInfo = TYPE_BYTE
            };
        }

        public static Bits Of(short value)
        {
            return new Bits(null)
            {
                _typeInfo = TYPE_SHORT_INTEGER
            };
        }

        public static Bits Of(int value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _typeInfo = TYPE_INTEGER
            };
        }

        public static Bits Of(long value)
        {
            var bytes = BitConverter.GetBytes(value);
            return new Bits(bytes)
            {
                _typeInfo = TYPE_LONG
            };
        }

        public static Bits Of(float value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _typeInfo = TYPE_FLOAT
            };
        }

        public static Bits Of(double value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _typeInfo = TYPE_DOUBLE
            };
        }
        
        public static Bits Of(decimal value)
        {
            var bytes = new List<byte>();
            var bits  = decimal.GetBits(value);
            foreach(var integral in bits)
            {
                bytes.AddRange(BitConverter.GetBytes(integral));
            }

            return new Bits(bytes.ToArray())
            {
                _typeInfo = TYPE_DECIMAL
            };
        }

        public static Bits Of(char value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _typeInfo = TYPE_CHAR
            };
        }

        public static Bits Of(bool value)
        {
            return new Bits(BitConverter.GetBytes(value))
            {
                _typeInfo = TYPE_BOOLEAN
            };
        }

        public static string ToBinaryString(byte value)
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

}