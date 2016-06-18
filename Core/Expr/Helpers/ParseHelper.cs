using System.Globalization;

namespace Core.Expr.Helpers
{
    public class ParseHelper
    {
        public static bool IsSingleQuote(char character)
        {
            return character == '\'';
        }

        public static bool IsIdentifierStart(char character)
        {
            return char.IsLetter(character)
                || character == '_'
                || char.GetUnicodeCategory(character) == UnicodeCategory.LetterNumber;
        }

        public static bool IsPartOfIdentifier(char character)
        {
            return char.IsDigit(character)
                || IsIdentifierStart(character);
        }

        public static bool IsWhitespace(char character)
        {
            return char.IsWhiteSpace(character);
        }

        public static bool IsQuestionMark(char character)
        {
            return character == '?';
        }

        public static bool IsSemiColon(char character)
        {
            return character == ':';
        }

        public static bool IsNumber(char character)
        {
            return char.IsDigit(character);
        }

        public static bool IsPartOfNumber(char character)
        {
            return char.IsDigit(character) || character == '.';
        }

        public static bool IsEndOfNumber(char character)
        {
            return character == 'l' || character == 'L'
                                    || character == 'u'
                                    || character == 'U'
                                    || character == 'd'
                                    || character == 'D'
                                    || character == 'f'
                                    || character == 'F'
                                    || character == 'm'
                                    || character == 'M';

        }

        public static bool IsFullStop(char character)
        {
            return character == '.';
        }

        public static bool IsArgumentSeperator(char character)
        {
            return character == ',';
        }

        public static bool IsLeftParent(char character)
        {
            return character == '(';
        }

        public static bool IsRightParent(char character)
        {
            return character == ')';
        }
    }

}