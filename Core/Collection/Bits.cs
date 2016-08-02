using Core.Concurrent;
using System;
using System.Linq;
using Core.Primitive;

namespace Core.Collection
{

    /// <summary>
    /// This class is used to working with int, long, float, double ,decimal, char data type
    /// int     : 32  bits which is 4  bytes
    /// float   : 32  bits which is 4  bytes
    /// double  : 64  bits which is 8  bytes
    /// decimal : 128 bits which is 16 bytes;
    /// char    : 16  bits which is 2  bytes
    /// long    : 64  bits which is 4  bytes
    /// This class intend to be immutable 
    /// and operation will yield a new bit class
    /// </summary>
    public sealed class Bits : IEquatable<Bits>, IComparable<Bits>
    {

        public const int BitsPerInteger = 32;
        public const int BitsPerFloat   = 32;
        public const int BitsPerDouble  = 64;
        public const int BitsPerDecimal = 128;
        public const int BitsPerChar    = 16;
        public const int BitsPerLong    = 64;

        private readonly byte[] _bytes;

        public bool IsIneger
        {
            get;
        }
        public bool IsFloat
        {
            get;
        }
        public bool IsDouble
        {
            get;
        }

        public Bits() : this(0)
        {
        }

        private Bits(int value) : this(BitConverter.GetBytes(value))
        {
            IsIneger = true;
            IsFloat  = false;
            IsDouble = false;
        }

        private Bits(byte[] bytes)
        {
            _bytes = bytes;
        }

        public int ToInt()
        {
            unsafe
            {
                fixed (byte* p = _bytes)
                {
                    return *((int*)p);
                }
            }
        }

        public Bits Or(Bits bits)
        {
            unsafe
            {
                fixed (byte* p1 = _bytes, p2 = bits._bytes)
                {
                    var i1 = *((int*)p1);
                    var i2 = *((int*)p2);
                    return new Bits(i1 | i2);
                }
            }
        }

        public Bits And(Bits bits)
        {
            unsafe
            {
                fixed (byte* p1 = _bytes, p2 = bits._bytes)
                {
                    var i1 = *((int*)p1);
                    var i2 = *((int*)p2);
                    return new Bits(i1 & i2);
                }
            }
        }

        public Bits Xor(Bits bits)
        {
            unsafe
            {
                fixed (byte* p1 = _bytes, p2 = bits._bytes)
                {
                    var i1 = *((int*)p1);
                    var i2 = *((int*)p2);
                    return new Bits(i1 ^ i2);
                }
            }
        }

        public Bits Not()
        {
            unsafe
            {
                fixed (byte* p1 = _bytes)
                {
                    return new Bits(~(*((int*)p1)));
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
                        return new Bits(i1 + i2);
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
                        return new Bits(i1 - i2);
                    }
                }
            }
        }

        public Bits Set(int index)
        {
            return Or(new Bits(1 << (index % 32)));
        }

        public Bits Remove(int index)
        {
            Checks.True(index < 32 && index > 0, "index is out of range.");
            return And(new Bits(1 << (index % 32)).Not());
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

        public int CompareTo(Bits other)
        {
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

        public string AsString(bool isLittleEndian)
        {
            return isLittleEndian
                 ? Arrays.Reverse(_bytes).Select(Numbers.BinaryOf).AsString()
                 : Arrays.CopyOf(_bytes).Select(Numbers.BinaryOf).AsString();
        }

        public override string ToString()
        {
            return AsString(OS.IsLittleEndian);
        }

        public static Bits From(int value)
        {
            return new Bits(BitConverter.GetBytes(value));
        }

        public static Bits From(double value)
        {
            return new Bits(BitConverter.GetBytes(value));
        }

        public static Bits From(float value)
        {
            return new Bits(BitConverter.GetBytes(value));
        }

    }

}