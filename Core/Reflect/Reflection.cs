namespace Core.Reflect
{

    public sealed class Reflection
    {

        public static bool IsString<T>(T vlaue) { return typeof(T) == typeof(string); }
        public static bool IsInt<T>(T value)    { return typeof(T) == typeof(int);    }
        public static bool IsDouble<T>(T value) { return typeof(T) == typeof(double); }
        public static bool IsLong<T>(T value)   { return typeof(T) == typeof(long);   }

    }

}