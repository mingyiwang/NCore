using System;
using Core.Collection;

namespace Core.Primitive {

    public static class Strings {

        public static string Of(string value) {
            return Of(value, string.Empty);
        }

        public static string Of(string value, string defaultValue) {
            return value ?? defaultValue;
        }

        public static string Of<T>(T obj) {
            return obj == null ? string.Empty : Of(obj.ToString(), string.Empty);
        }

        public static string Of<T>(T obj, string defaultValue) {
            return obj == null ? defaultValue : Of(obj.ToString(), defaultValue);
        }

        public static string Between(string input, int startIndex, int endIndex) {
            return SubString(input, (startIndex + 1), (endIndex - 1));
        }

        public static string SubString(string input, int startIndex, int endIndex) {
            if (startIndex < 0) {
                throw new IndexOutOfRangeException($"Start Index[{startIndex}] is out of range.");
            }

            if (endIndex > input.Length) {
                throw new IndexOutOfRangeException($"End Index[{endIndex}] is out of range.");
            }

            var subLength = endIndex - startIndex;
            if(subLength < 0) {
                throw new IndexOutOfRangeException($"String Length[{subLength}] is out of range.");
            }

            return ((startIndex == 0) && (endIndex == input.Length))
                 ? input
                 : input.Substring(startIndex, subLength)
                 ;
        }

        public static string[] Split(string input, params char[] characters) {
            return Split(input, StringSplitOptions.RemoveEmptyEntries, characters);
        }

        public static string[] Split(string input, StringSplitOptions options, params char[] characters) {
            Preconditions.CheckNotNull(characters, "Seperator can not be null.");
            return string.IsNullOrEmpty(input) ? Arrays.Empty<string>() : input.Split(characters, options);
        }
    }

}
