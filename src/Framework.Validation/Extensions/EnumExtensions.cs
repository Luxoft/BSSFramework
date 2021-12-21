using System;
using System.Reflection;

namespace Framework.Validation
{
    public static class EnumExtensions
    {
        public static bool HasFlag(this int source, int target)
        {
            return (source & target) == target;
        }

        public static bool IsIntersected(this int source, int target)
        {
            return (source & target) != 0;
        }
    }
}