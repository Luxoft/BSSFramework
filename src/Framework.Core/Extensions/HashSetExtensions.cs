using System;
using System.Collections.Generic;

namespace Framework.Core
{
    public static class HashSetExtensions
    {
        public static List<T> ToList<T>(this HashSet<T> hashSet)
        {
            if (hashSet == null) throw new ArgumentNullException(nameof(hashSet));

            var list = new List<T>(hashSet.Count);

            list.AddRange(hashSet);

            return list;
        }
    }
}
