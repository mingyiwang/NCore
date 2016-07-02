using System;
using System.Collections.Generic;

namespace Core
{

    public sealed class Preconditions
    {

        public static void CheckNotNull(object obj)
        {
            CheckNotNull(obj, $"[Precondition Check Failed] - Object{obj.GetType()} can not be null.");
        }

        public static void CheckNotNull(object obj, string message)
        {
            CheckNotNull<NullReferenceException>(obj, message);
        }

        public static void CheckNotNull<T>(object obj, string message) where T : Exception
        {
            if(obj == null)
            {
                Fail<T>(message);
            }
        }

        public static void CheckNotEmpty<TC>(ICollection<TC> collection)
        {
            CheckNotEmpty<ArgumentException, TC>(collection, "[Precondition Check Failed] - Collection can not be emtpy.");
        }

        public static void CheckNotEmpty<TC>(ICollection<TC> collection, string message)
        {
            CheckNotEmpty<ArgumentException, TC>(collection, message);
        }

        public static void CheckNotEmpty<TE, TC>(ICollection<TC> collection, string message) where TE : Exception
        {
            if(collection == null || collection.Count == 0)
            {
                Fail<TE>(message);
            }
        }

        public static void CheckNotBlank(string s)
        {
            CheckNotBlank(s, "[Precondition Check Failed] - String can not be empty.");
        }

        public static void CheckNotBlank(string s, string message)
        {
            CheckNotBlank<ArgumentException>(s, message);
        }

        public static void CheckNotBlank<T>(string s, string message) where T : Exception
        {
            if(string.IsNullOrEmpty(s))
            {
                Fail<T>(message);
            }
        }

        public static void CheckEquals(int expected, int actual)
        {
            CheckEquals<ArgumentException>(expected, actual, "[Precondition Check Failed] - value must be equal.");
        }

        public static void CheckEquals(string expected, string actual)
        {
            CheckEquals<ArgumentException>(expected, actual, "[Precondition Check Failed] - value must be equal.");
        }

        public static void CheckEquals<T>(T expected, T actual)
        {
            if(!expected.Equals(actual))
            {
                Fail<ArgumentException>($"[Precondition Check Failed] - value must be equal. expected {expected} but was {actual}");
            }
        }

        public static void CheckEquals(int expected, int actual, string message){
            if (expected != actual) {
                Fail<ArgumentException>(message);
            }
        }

        public static void CheckEquals<T>(object expected, object actual, string message) where T : Exception
        {
            if(!expected.Equals(actual))
            {
                Fail<T>(message);
            }
        }

        public static void CheckNotEquals(int expected, int actual, string message)
        {
            CheckNotEquals<ArgumentException>(expected, actual, message);
        }

        public static void CheckNotEquals(object expected, object actual)
        {
            CheckNotEquals<ArgumentException>(expected, actual, "[Precondition Check Failed] - value must not be equal.");
        }

        public static void CheckNotEquals<T>(object expected, object actual, string message) where T : Exception
        {
            if(ReferenceEquals(expected, actual) || expected.Equals(actual))
            {
                Fail<T>(message);
            }
        }

        public static void CheckFalse(bool value)
        {
            CheckFalse(value, "[Precondition Check Failed] - Value must be false");
        }

        public static void CheckFalse(bool value, string message)
        {
            CheckFalse<ArgumentException>(value, message);
        }

        public static void CheckFalse<T>(bool? value, string message) where T : Exception
        {
            if(value != false)
            {
                Fail<T>(message);
            }
        }

        public static void CheckTrue(bool value)
        {
            CheckTrue(value, "[Precondition Check Failed] - Value must be true.");
        }

        public static void CheckTrue(bool value, string message)
        {
            CheckTrue<ArgumentException>(value, message);
        }

        public static void CheckTrue<T>(bool value, string message) where T : Exception
        {
            if(value != true)
            {
                Fail<T>(message);
            }
        }

        private static void Fail<T>(string message) where T : Exception
        {
            var type = typeof(T);
            var constructor = type.GetConstructor(new[]{
                typeof(string)
            });
            
            if(constructor == null)
            {
                throw new ArgumentException("Exception[" + type.FullName + "] must have a constructor of a single string parameter");
            }

            throw (T) constructor.Invoke(new object[] {
                   message
            });
        }

    }

}