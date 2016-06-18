using System;
using System.Globalization;

namespace Core.Expr.Parse
{

    public class Location : IEquatable<Location>, IComparable<Location>
    {
        public static readonly Location Nil = new Location(-1, -1);
        public static readonly Location Zero = new Location(0, 0);

        private readonly int _startIndex;
        private readonly int _endIndex;
        private readonly int _length;

        public Location(int start, int end)
        {
            _startIndex = start;
            _endIndex = end;
            _length = end - start;
        }

        public int Length
        {
            get
            {
                return _length;
            }
        }

        public int EndIndex
        {
            get
            {
                return _endIndex;
            }
        }

        public int StartIndex
        {
            get
            {
                return _startIndex;
            }
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, "({0}:{1} - {2})", Length, StartIndex, EndIndex);
        }

        public override bool Equals(object obj)
        {
            return (obj is Location) && Equals((Location)obj);
        }

        public override int GetHashCode()
        {
            return StartIndex;
        }

        public bool Equals(Location other)
        {
            return StartIndex == other.StartIndex && EndIndex == other.EndIndex;
        }

        public int CompareTo(Location other)
        {
            return StartIndex.CompareTo(other.StartIndex) & EndIndex.CompareTo(other.EndIndex);
        }

    }

}