using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

namespace Framework.Core
{
    public static class ActionExtensions
    {
        public static Action<T1, T2> Composite<T1, T2>([NotNull] this IEnumerable<Action<T1, T2>> source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            var cachedSource = source.ToArray();

            return (v1, v2) => cachedSource.Foreach(action => action(v1, v2));
        }

        public static Action<T1, T2> Composite<TSource, T1, T2>([NotNull] this IEnumerable<TSource> source, [NotNull] Func<TSource, Action<T1, T2>> selector)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (selector == null) throw new ArgumentNullException(nameof(selector));

            return source.Select(selector).Composite();
        }
    }
}
