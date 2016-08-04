using System;
using System.Collections.Generic;
using System.Linq;
using Core.Collection;
using Core.Concurrent;

namespace Core.Primitive
{

    /// <summary>
    /// This class is used to working with 
    ///     int, short, long, float, double, decimal, char, bool
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

        public int ToInt()
        {
            if (IsInt32)
            {
                unsafe
                {
                    fixed (byte* p = _bytes)
                    {
                        return *(int*) p;
                    }
                }
            }
            return 0;
        }

        public Bits Or(Bits bits)
        {
            unsafe
            {
                fixed (byte* p1 = _bytes, p2 = bits._bytes)
                {
//                    var i1 = *((int*)p1);
//                    var i2 = *((int*)p2);
//                    return new Bits(i1 | i2);
                    return null;
                }
            }
        }

        public Bits And(Bits bits)
        {
            unsafe
            {
                fixed (byte* p1 = _bytes, p2 = bits._bytes)
                {
//                    var i1 = *((int*)p1);
//                    var i2 = *((int*)p2);
                    return null;
                }
            }
        }

        public Bits Xor(Bits bits)
        {
            unsafe
            {
                fixed (byte* p1 = _bytes, p2 = bits._bytes)
                {
//                    var i1 = *((int*)p1);
//                    var i2 = *((int*)p2);
                    return null;
                }
            }
        }

        public Bits Not()
        {
            unsafe
            {
                fixed (byte* p1 = _bytes)
                {
                    return null;
                }
            }
        }

        public Bits Plus(Bits bits)
        {
            unsafe
            {
                fixed (byte* p1 = _bytes, p2 = bits._bytes)
                {
                    var i1 = *((int*)p1);
                    var i2 = *((int*)p2);
                    checked
                    {
                        return null;
                    }
                }
            }
        }

        public Bits Minus(Bits bits)
        {
            unsafe
            {
                fixed (byte* p1 = _bytes, p2 = bits._bytes)
                {
                    var i1 = *((int*)p1);
                    var i2 = *((int*)p2);
                    checked
                    {
                        return null;
                    }
                }
            }
        }

        public Bits Set(int index)
        {
            return null;
            //return Or(new Bits(1 << (index % 32)));
        }

        public Bits Clear(int index)
        {
            Checks.True(index < 32 && index > 0, "index is out of range.");
            //return And(new Bits(1 << (index % 32)).Not());
            return null;
        }

        public override int GetHashCode()
        {
            return ToInt();
        }

        public bool Equals(Bits other)
        {
            unsafe
            {
                fixed (byte* p1 = _bytes, p2 = other._bytes)
                {
                    var i1 = *((int*)p1);
                    var i2 = *((int*)p2);
                    return i1 == i2;
                }
            }
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

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(Bits other)
        {
            // comparing bytes should have the same length and the same type
            unsafe
            {
                fixed (byte* p1 = _bytes, p2 = other._bytes)
                {
                    var i1 = *((int*)p1);
                    var i2 = *((int*)p2);
                    return i1.CompareTo(i2);
                }
            }
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

        public static string ToBinaryString(float value)
        {
            return string.Empty;
        }

        public static string ToBinaryString(double value)
        {
            return string.Empty;
        }

        public static string ToBinaryString(bool value)
        {
            return string.Empty;
        }

        public static string ToBinaryString(char value)
        {
            return string.Empty;
        }

        public static string ToBinaryString(int value)
        {
            // joining 4 bytes respresentation of integer
            return string.Empty;
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