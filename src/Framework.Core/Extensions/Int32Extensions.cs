using System;

namespace Framework.Core
{
    public static class Int32Extensions
    {
        [Obsolete("v10 For Remove")]
        public static int MinMax(this int value, int min, int max)
        {
            return Math.Min(Math.Max(value, min), max);
        }
    }
}
