using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Core
{
    [Flags]
    public enum TypeSearchMode
    {
        Name =  1,

        FullName = 2,

        Both = Name + FullName
    }

    public static class TypeSearchModeExtensions
    {
        public static Func<Type, string, bool> ToFilter(this TypeSearchMode searchMode)
        {
            return (type, ident) => searchMode.ToFilters().Any(f => f(type, ident));
        }

        private static IEnumerable<Func<Type, string, bool>> ToFilters(this TypeSearchMode searchMode)
        {
            if (searchMode.HasFlag(TypeSearchMode.Name))
            {
                yield return (type, name) => type.Name == name;
            }

            if (searchMode.HasFlag(TypeSearchMode.FullName))
            {
                yield return (type, fullName) => type.FullName == fullName;
            }
        }
    }
}