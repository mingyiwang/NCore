using System;
using System.Collections.Generic;

namespace Core
{

    public sealed class Checks
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

        public static void Null(object obj)
        {
            Null(obj, $"[Precondition Check Failed] - Object must be null.");
        }

        public static void Null(object obj, string message)
        {
            Null<ArgumentException>(obj, message);
        }

        public static void Null<T>(object obj, string message) where T : Exception
        {
            if (obj != null)
            {
                Fail<T>(message);
            }
        }

        public static void NotEmpty<TC>(ICollection<TC> collection)
        {
            NotEmpty<ArgumentException, TC>(collection, "[Precondition Checks Failed] - Collection can not be emtpy.");
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
            NotBlank(s, "[Precondition Checks Failed] - String can not be empty.");
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
            Equals<ArgumentException>(expected, actual, "[Precondition Checks Failed] - value must be equal.");
        }

        public static void Equals(string expected, string actual)
        {
            Equals<ArgumentException>(expected, actual, "[Precondition Checks Failed] - value must be equal.");
        }

        public static void Equals<T>(T expected, T actual)
        {
            if(!expected.Equals(actual))
            {
                Fail<ArgumentException>($"[Precondition Check Failed] - value must be equal. expected {expected} but was {actual}");
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
            NotEquals<ArgumentException>(expected, actual, "[Precondition Checks Failed] - value must not be equal.");
        }

        public static void NotEquals<T>(object expected, object actual, string message) where T : Exception
        {
            if (ReferenceEquals(expected, actual) || expected.Equals(actual))
            {
                Fail<T>(message);
            }
        }

        public static void False(bool value)
        {
            False(value, "[Precondition Checks Failed] - Value must be false");
        }

        public static void False(bool value, string message)
        {
            False<ArgumentException>(value, message);
        }

        public static void False<T>(bool? value, string message) where T : Exception
        {
            if(value != false)
            {
                Fail<T>(message);
            }
        }

        public static void True(bool value)
        {
            True(value, "[Precondition Checks Failed] - Value must be true.");
        }

        public static void True(bool value, string message)
        {
            True<ArgumentException>(value, message);
        }

        public static void True<T>(bool value, string message) where T : Exception
        {
            if(value != true)
            {
                Fail<T>(message);
            }
        }

        public static void InRange(int min, int max, int actual, string message)
        {
            if (actual <= min || actual >= max)
            {
                Fail<ArgumentOutOfRangeException>(message);
            }
        }

        public static void LessThan<T>(int expected, int actual, string message) where T : Exception
        {
            if (actual >= expected)
            {
                Fail<T>(message);
            }
        }

        public static void LessThanOrEqual<T>(int expected, int actual, string message) where T : Exception
        {
            if(actual > expected)
            {
                Fail<T>(message);
            }
        }

        public static void GreaterThan<T>(int expected, int actual, string message) where T : Exception
        {
            if (actual <= expected)
            {
                Fail<T>(message);
            }
        }

        public static void GreaterThanOrEqual<T>(int expected, int actual, string message) where T : Exception
        {
            if(actual < expected)
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