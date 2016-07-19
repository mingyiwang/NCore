using System;
using System.Collections.Generic;

namespace Core
{

    public sealed class Check
    {
        public static void NotNull(object obj)
        {
            NotNull(obj, $"[Precondition Check Failed] - Object{obj.GetType()} can not be null.");
        }

        public static void NotNull(object obj, string message)
        {
            NotNull<NullReferenceException>(obj, message);
        }

        public static void NotNull<T>(object obj, string message) where T : Exception
        {
            if(obj == null)
            {
                Fail<T>(message);
            }
        }

        public static void NotEmpty<TC>(ICollection<TC> collection)
        {
            NotEmpty<ArgumentException, TC>(collection, "[Precondition Check Failed] - Collection can not be emtpy.");
        }

        public static void NotEmpty<TC>(ICollection<TC> collection, string message)
        {
            NotEmpty<ArgumentException, TC>(collection, message);
        }

        public static void NotEmpty<TE, TC>(ICollection<TC> collection, string message) where TE : Exception
        {
            if(collection == null || collection.Count == 0)
            {
                Fail<TE>(message);
            }
        }

        public static void NotBlank(string s)
        {
            NotBlank(s, "[Precondition Check Failed] - String can not be empty.");
        }

        public static void NotBlank(string s, string message)
        {
            NotBlank<ArgumentException>(s, message);
        }

        public static void NotBlank<T>(string s, string message) where T : Exception
        {
            if(string.IsNullOrEmpty(s))
            {
                Fail<T>(message);
            }
        }

        public static void Equals(int expected, int actual)
        {
            Equals<ArgumentException>(expected, actual, "[Precondition Check Failed] - value must be equal.");
        }

        public static void Equals(string expected, string actual)
        {
            Equals<ArgumentException>(expected, actual, "[Precondition Check Failed] - value must be equal.");
        }

        public static void Equals<T>(T expected, T actual)
        {
            if(!expected.Equals(actual))
            {
                Fail<ArgumentException>($"[Precondition Check Failed] - value must be equal. expected {expected} but was {actual}");
            }
        }

        public static void Equals(int expected, int actual, string message){
            if (expected != actual) {
                Fail<ArgumentException>(message);
            }
        }

        public static void Equals<T>(object expected, object actual, string message) where T : Exception
        {
            if(!expected.Equals(actual))
            {
                Fail<T>(message);
            }
        }

        public static void NotEquals(int expected, int actual, string message)
        {
            NotEquals<ArgumentException>(expected, actual, message);
        }

        public static void NotEquals(object expected, object actual)
        {
            NotEquals<ArgumentException>(expected, actual, "[Precondition Check Failed] - value must not be equal.");
        }

        public static void NotEquals<T>(object expected, object actual, string message) where T : Exception
        {
            if(ReferenceEquals(expected, actual) || expected.Equals(actual))
            {
                Fail<T>(message);
            }
        }

        public static void NotTrue(bool value)
        {
            NotTrue(value, "[Precondition Check Failed] - Value must be false");
        }

        public static void NotTrue(bool value, string message)
        {
            NotTrue<ArgumentException>(value, message);
        }

        public static void NotTrue<T>(bool? value, string message) where T : Exception
        {
            if(value != false)
            {
                Fail<T>(message);
            }
        }

        public static void IsTrue(bool value)
        {
            IsTrue(value, "[Precondition Check Failed] - Value must be true.");
        }

        public static void IsTrue(bool value, string message)
        {
            IsTrue<ArgumentException>(value, message);
        }

        public static void IsTrue<T>(bool value, string message) where T : Exception
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