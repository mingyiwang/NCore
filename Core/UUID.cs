using System;

namespace Core
{
    public sealed class UUID
    {
        public static string NGuid => Guid.NewGuid().ToString("N");
        public static string DGuid => Guid.NewGuid().ToString("");

    }

}