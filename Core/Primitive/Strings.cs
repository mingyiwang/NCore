using System;
using Core.Collection;

namespace Core.Primitive
{

    public static class Strings
    {

        public static string Of(string value, string defaultIfNull)
        {
            return value ?? defaultIfNull;
        }

        public static string Of(string value)
        {
            return Of(value, string.Empty);
        }

        public static string Of<T>(T obj, string defaultIfNull)
        {
            return obj == null ? defaultIfNull : Of(obj.ToString(), defaultIfNull);
        }

        public static string Of<T>(T obj)
        {
            return Of(obj, string.Empty);
        }

        public static string Between(string input, int startIndex, int endIndex)
        {
            if (startIndex == endIndex)
            {
                return string.Empty;
            }

            return Substring(input, startIndex + 1, endIndex - 1);
        }

        public static string Substring(string input, int startIndex, int endIndex)
        {
            if (startIndex < 0)
            {
                throw new IndexOutOfRangeException($"Start Index[{startIndex}] is out of range.");
            }

            if (startIndex >= input.Length)
            {
                throw new IndexOutOfRangeException($"Start Index[{startIndex}] is out of range.");
            }

            if (endIndex >= input.Length)
            {
                throw new IndexOutOfRangeException($"End Index[{endIndex}] is out of range.");
            }

            var subLength = endIndex - startIndex + 1;
            if (subLength < 0)
            {
                throw new IndexOutOfRangeException($"SubString Length[{subLength}] can not be negative.");
            }

            return (startIndex == 0) && (endIndex == input.Length - 1)
                 ? input
                 : input.Substring(startIndex, subLength)
                 ;
        }

        public static string[] Split(string input, params char[] characters)
        {
            return Split(input, StringSplitOptions.RemoveEmptyEntries, characters);
        }

        public static string[] Split(string input, StringSplitOptions options, params char[] characters)
        {
            Check.NotNull(characters, "Seperator can not be null.");
            return string.IsNullOrEmpty(input) ? Arrays.Empty<string>() : input.Split(characters, options);
        }
    }

}
