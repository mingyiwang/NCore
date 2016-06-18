using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Core.Primitive;

namespace Core.Collection
{

    public sealed class Collections
    {

        public static string Join<T>(IEnumerable<T> collection)
        {
            var builder = new StringBuilder();
            var enumerator = collection.GetEnumerator();
            enumerator.MoveNext();
            do
            {
                builder.Append(Strings.Of(enumerator.Current));
            }
            while(enumerator.MoveNext());
            return builder.ToString();
        }

        public static string Join<T>(IEnumerable<T> collection, Func<T, string> convert)
        {
            var builder = new StringBuilder();
            var enumerator = collection.GetEnumerator();
            enumerator.MoveNext();
            do
            {
                builder.Append(Strings.Of(convert(enumerator.Current)));
            }
            while(enumerator.MoveNext());
            return builder.ToString();
        }

        public static string Join<T>(char character, IEnumerable<T> collection)
        {
            return Join<char, T>(character, collection);
        }

        public static string Join<T>(char character, IEnumerable<T> collection, Func<T, string> convert)
        {
            return Join<char, T>(character, collection, convert);
        }

        public static string Join<TJ, T>(TJ character, IEnumerable<T> collection)
        {
            var joiner = Strings.Of(character);
            var builder = new StringBuilder();

            var enumerator = collection.GetEnumerator();
            if(enumerator.MoveNext())
            {
                builder.Append(Strings.Of(enumerator.Current));
            }

            while(enumerator.MoveNext())
            {
                builder.Append(joiner);
                builder.Append(Strings.Of(enumerator.Current));
            }

            return builder.ToString();
        }

        public static string Join<TJ, T>(TJ character, IEnumerable<T> collection, Func<T, string> convert)
        {
            var joiner = Strings.Of(character);
            var builder = new StringBuilder();

            var enumerator = collection.GetEnumerator();
            if(enumerator.MoveNext())
            {
                builder.Append(Strings.Of(convert(enumerator.Current)));
            }

            while(enumerator.MoveNext())
            {
                builder.Append(joiner);
                builder.Append(Strings.Of(convert(enumerator.Current)));
            }

            return builder.ToString();
        }

        public static IReadOnlyCollection<TModel> AsReadOnly<TModel>(IList<TModel> collection)
        {
            return new ReadOnlyCollection<TModel>(collection);
        }

        public static IReadOnlyDictionary<TKey, TValue> AsReadOnly<TKey, TValue>(IDictionary<TKey, TValue> enumrable)
        {
            return new ReadOnlyDictionary<TKey, TValue>(enumrable);
        }

        public static bool Equals<TKey, TValue>(IDictionary<TKey, TValue> dic1, IDictionary<TKey, TValue> dic2)
        {
            if(ReferenceEquals(dic1, dic2))
            {
                return true;
            }

            // 1. Found any key not exist in dic2 or value is not equal in dic2 then return true then false
            // 2. Can not found any then return false then true
            return dic1.Count == dic2.Count &&
                 !(dic1.Keys.AsParallel().Any(key => (!dic2.ContainsKey(key) || !dic2[key].Equals(dic1[key]))));
        }


    }
}