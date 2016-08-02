using System.Collections.Generic;
using Core.Collection;

namespace Core.Time.Format
{

    public abstract class DateTimeFormatToken
    {
        
        private static readonly ReadWriteHashSet<char> ReservedChars = new ReadWriteHashSet<char>
        {
            'F','H','K','M',
            'd','f','g','h',
            'm','s','t','y',
            'z','%',':','/',
            '"','\'','\\'
        };
                
        public static readonly DateTimeFormatToken Year       = new DateTimeValueFormatToken("y",  4);
        public static readonly DateTimeFormatToken Month      = new DateTimeValueFormatToken("M",  2);
        public static readonly DateTimeFormatToken Day        = new DateTimeValueFormatToken("d",  2);
        public static readonly DateTimeFormatToken Hour       = new DateTimeValueFormatToken("h",  2);
        public static readonly DateTimeFormatToken Minute     = new DateTimeValueFormatToken("m",  2);
        public static readonly DateTimeFormatToken Second     = new DateTimeValueFormatToken("s",  2);
        public static readonly DateTimeFormatToken MillSecond = new DateTimeValueFormatToken("F",  3);

        private readonly int    _baseKind;
        private readonly string _baseCode;

        public bool IsLiteral { get; private set; }
        public bool IsValue   { get; private set; }

        public abstract string GetCode();

        private DateTimeFormatToken(string baseCode, int baseKind)
        {
            _baseKind = baseKind;
            _baseCode = baseCode;
            IsLiteral = false;
            IsValue   = false;
        }

        public DateTimeFormatToken WithKind(int kind)
        {
            return new DateTimeLiteralFormatToken(_baseCode, kind);
        }

        public override string ToString()
        {
            return GetCode();
        }

        public static DateTimeFormatToken Of(char literal)
        {
            return new DateTimeLiteralFormatToken(ConvertToString(literal), 0);
        }

        private static string ConvertToString(char literal)
        {
            if (!ReservedChars.Contains(literal))
            {
                return char.ToString(literal);
            }
            return "\\" + char.ToString(literal);
        }

        private sealed class DateTimeLiteralFormatToken : DateTimeFormatToken
        {
            public DateTimeLiteralFormatToken(string baseCode, int baseKind) 
                : base(baseCode, baseKind)
            {
                IsLiteral = true;
            }

            public override string GetCode()
            {
                return _baseCode;
            }
        }

        private sealed class DateTimeValueFormatToken : DateTimeFormatToken
        {
            public DateTimeValueFormatToken(string baseCode, int baseKind) 
                : base(baseCode, baseKind)
            {
                IsValue = true;
            }

            public override string GetCode()
            {
                return _baseCode.PadRight(_baseKind, char.Parse(_baseCode));
            }

        }

    }
}