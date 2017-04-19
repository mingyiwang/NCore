using System;
using System.Collections.Generic;
using System.Linq;
using Core.Collection;

namespace Core.Primitive
{

    public sealed class IntBits : IEquatable<IntBits>
    {

        public static IntBits Of(IEnumerable<int> values)
        {
            Checks.NotNull(values);
            return Of(values.ToArray());
        }

        public static IntBits Of(params int[] values)
        {
            Checks.NotNull(values);
            return new IntBits(values);
        }

        // Bits from lower to higher
        private readonly int[] _bits;

        private IntBits(int[] bits)
        {
            Checks.NotNull(bits);
            _bits = bits;
        }

        public int Bytes
        {
            get
            {
                return Bits.BytesPerInt32 * _bits.Length;
            }
        }

        public int Last()
        {
            return _bits.Last();
        }

        public int First()
        {
            return _bits.First();
        }

        public byte[] GetBytes()
        {
            var bytes = new List<byte>(Bytes);
            foreach (var bit in _bits)
            {
                bytes.AddRange(BitConverter.GetBytes(bit));
            }
            return bytes.ToArray();
        }

        public IntBits Reverse()
        {
            var reversed = _bits.Select(BitConverter.GetBytes).Reverse().ToArray();
            return Of(0);
        }

        public string ToBinaryString()
        {
            return Joiner.On("-").Join(Arrays.Reverse(GetBytes()), bytes => bytes.ToBinaryString());
        }

        public bool Equals(IntBits other)
        {
            return Arrays.Equals(_bits, other._bits);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            var bits = obj as IntBits;
            return bits != null && Equals(bits);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return _bits.Aggregate(0, (current, bit) => current * 37 ^ bit);
            }
        }

        public override string ToString()
        {
            return ToBinaryString();
        }

    }

}