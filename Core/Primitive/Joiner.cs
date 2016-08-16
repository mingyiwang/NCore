using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Core.Primitive
{
    public sealed class Joiner
    {

        private readonly string _seperator;

        private Joiner(string seperator)
        {
            _seperator = seperator;
        }

        public static Joiner On(char character)
        {
            return new Joiner(char.ToString(character));
        }

        public static Joiner On(string texts)
        {
            return new Joiner(Strings.Of(texts));
        }

        public string Join(IList items, Func<object, string> generator = null)
        {
            
            return null;
        }

        public string Join<T>(IEnumerable<T> enumerable, Func<T, string> generator = null)
        {
            if (enumerable == null)
            {
                return string.Empty;
            }

            var builder = new StringBuilder();
            var enumerator = enumerable.GetEnumerator();
            if (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                builder.Append(generator == null ? Strings.Of(item) : generator(item));
            }

            while (enumerator.MoveNext())
            {
                var item = enumerator.Current;
                builder.Append(generator == null ? Strings.Of(item) : generator(item));
            }
            return builder.ToString();
        }

        

    }


}